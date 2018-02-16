using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MapManager : MonoBehaviour, IMapManager
{
    public string map_name { get { return map.name; } }
    public string map_description { get { return map.description; } }

    public int map_columns { get { return map.columns; } }
    public int map_rows { get { return map.rows; } }
    public int map_area { get { return map.area; } }

    public Vector2 tile_size { get { return tile_size_; } }
    public Vector2 half_tile_size { get; private set; }

    public Vector2 map_center { get; private set; }
    public Vector2 map_size { get; private set; }

    public Bounds map_bounds { get; private set; }
    public Vector3 map_bounds_min { get { return map_bounds.min; } }
    public Vector3 map_bounds_max { get { return map_bounds.max; } }

    [Header("Generation Settings")]
    public GenerationSettings settings;

    [Header("Map Texture")]
    [SerializeField] List<Texture2D> map_textures;
    [SerializeField] List<Sprite> entity_sprites;
    [SerializeField] Vector2 tile_size_ = new Vector2(16, 16);

    [Header("Prefabs")]
    [SerializeField] GameObject tile_prefab;
    [SerializeField] GameObject editor_prefab;
    [SerializeField] GameObject dungeon_entity;

    [Header("Containers")]
    [SerializeField] Transform map_container;
    [SerializeField] Transform lines_container;
    [SerializeField] Transform entities_container;

    [Header("Partition Lines")]
    [SerializeField] string sorting_layer = "Default";
    [SerializeField] int order_in_layer = 76;
    [SerializeField] Material line_material;
    [SerializeField] Color line_color;
    [SerializeField] bool draw_partition_lines = true;

    [Header("Events")]
    [SerializeField] UnityEvent map_generated_events;
    [SerializeField] UnityEvent map_loaded_events;
    [SerializeField] UnityEvent map_saved_events;

    private Map map;
    private Dungeon dungeon;
    private MapEditor map_editor;

    private List<Sprite[]> sprites_list = new List<Sprite[]>();
    private List<SpriteRenderer> sprite_tiles = new List<SpriteRenderer>();
    private Dictionary<int, DungeonEntity> dungeon_entities = new Dictionary<int, DungeonEntity>();

    private DungeonEntity player_spawn = null;
    private DungeonEntity level_exit = null;

    private int prev_map_columns = -1;
    private int prev_map_rows = -1;

    private bool map_different
    {
        get
        {
            return map_columns != prev_map_columns ||
                map_rows != prev_map_rows;
        }
    }


    public bool MapValid()
    {
        return map.MapValid();
    }


    public void GenerateMap()
    {
        map.CreateMap(settings.columns, settings.rows);
        VisualiseMap();

        dungeon.GenerateDungeon(settings);

        map_generated_events.Invoke();
    }


    public void LoadMap(PackedMap _pmap)
    {
        if (_pmap == null)
            return;

        map.CreateMap(_pmap);
        VisualiseMap();

        map_loaded_events.Invoke();
    }


    public void LoadMap(string _mapname)
    {
        LoadMap(FileIO.LoadMap(_mapname));
    }


    public void ExportMap(string _name, string _description)
    {
        if (!map.MapValid())
            return;

        map.InitDescriptionData(_name, _description);
        FileIO.ExportMap(map);

        map_saved_events.Invoke();
    }


    public void ToggleMapEditor()
    {
        if (sprite_tiles.Count == 0)
            return;

        if (map_editor == null)
        {
            GameObject obj = Instantiate(editor_prefab);
            obj.name = "MapEditor";

            map_editor = obj.GetComponent<MapEditor>();
            map_editor.Init(this);
        }
        else
        {
            if (map_editor != null)
            {
                Destroy(map_editor.gameObject);
                map_editor = null;
            }
        }
    }


    public bool PosInMapBounds(Vector2 _pos)
    {
        return map_bounds.Contains(_pos);
    }


    public Vector2 PosToTileCenter(Vector2 _pos)
    {
        int index = PosToTileIndex(_pos);
        return TileIndexToTileCenter(index);
    }


    public int PosToTileIndex(Vector2 _pos)
    {
        float offsetx = _pos.x + half_tile_size.x;
        int ix = (int)(offsetx / tile_size.x);

        float offsety = _pos.y - half_tile_size.y;
        int iy = Mathf.Abs((int)(offsety / tile_size.y));

        return (iy * map.columns) + ix;
    }


    public Vector2 TileIndexToTileCenter(int _tile_index)
    {
        int x = _tile_index % map.columns;
        int y = _tile_index / map.columns;

        return new Vector2(x * tile_size.x, -(y * tile_size.y));
    }


    public void Paint(Vector2 _pos, TerrainType _terrain_type, bool _single_sprite)
    {
        if (!PosInMapBounds(_pos))
            return;

        int index = PosToTileIndex(_pos);
        Paint(index, _terrain_type, _single_sprite);
    }


    public void Paint(int _tile_index, TerrainType _terrain_type, bool _single_sprite)
    {
        if (!JHelper.ValidIndex(_tile_index, map_area))
        {
            Debug.Log("Tile Index Out of Bounds: " + _tile_index);
            return;
        }

        map.UpdateTerrainType(_tile_index, _terrain_type);

        if (_terrain_type == TerrainType.ROCK)
            RemoveEntity(_tile_index);

        if (_single_sprite)
            return;

        // Update surrounding sprites ..
        int x = _tile_index % map_columns;
        int y = _tile_index / map_columns;

        for (int row = y - 1; row <= y + 1; ++row)
        {
            for (int col = x - 1; col <= x + 1; ++col)
            {
                if (col < 0 || col >= map_columns ||
                    row < 0 || row >= map_rows)
                {
                    continue;
                }

                _tile_index = JHelper.CalculateIndex(col, row, map_columns);
                SpriteRenderer tile = sprite_tiles[_tile_index];

                if (map.TileEmpty(_tile_index))
                {
                    tile.sprite = null;
                }
                else
                {
                    var sprite_list = sprites_list[(int)map.GetTerrainType(_tile_index) - 1];
                    tile.sprite = sprite_list[map.GetAutoTileID(_tile_index)];
                }

                EntityType residing_entity = map.GetEntityType(_tile_index);
                UpdateAutoRotatingEntity(_tile_index, residing_entity);
            }
        }
    }


    public void AddEntity(Vector2 _pos, EntityType _entity_type)
    {
        if (!PosInMapBounds(_pos))
            return;

        int index = PosToTileIndex(_pos);
        AddEntity(index, _entity_type);
    }


    public void AddEntity(int _tile_index, EntityType _entity_type)
    {
        if (_entity_type == EntityType.NONE)
        {
            RemoveEntity(_tile_index);
            return;
        }
        else if (map.GetTerrainType(_tile_index) == TerrainType.ROCK)
        {
            Paint(_tile_index, TerrainType.STONE, false);
        }

        DungeonEntity entity = null;

        if (!dungeon_entities.ContainsKey(_tile_index))
        {
            Vector3 pos = TileIndexToTileCenter(_tile_index);

            var clone = Instantiate(dungeon_entity, pos, Quaternion.identity);
            clone.transform.SetParent(entities_container);

            entity = clone.GetComponent<DungeonEntity>();

            dungeon_entities.Add(_tile_index, entity);
        }
        else
        {
            entity = dungeon_entities[_tile_index];
        }

        int type_id = (int)_entity_type;
        Sprite spr = type_id >= 0 ? entity_sprites[type_id] : null;
        entity.SetEntity(_entity_type, spr, _tile_index);

        map.SetEntityType(_tile_index, _entity_type);
        UpdateAutoRotatingEntity(_tile_index, _entity_type);

        if (_entity_type == EntityType.PLAYER_SPAWN)
        {
            SetSpawn(_tile_index);
        }
        else if (_entity_type == EntityType.STAIRS)
        {
            SetExit(_tile_index);
        }
    }


    public void RemoveEntity(Vector2 _pos)
    {
        if (!PosInMapBounds(_pos))
            return;

        int index = PosToTileIndex(_pos);
        RemoveEntity(index);
    }


    public void RemoveEntity(int _tile_index)
    {
        if (dungeon_entities.Count == 0 || !dungeon_entities.ContainsKey(_tile_index))
            return;

        Destroy(dungeon_entities[_tile_index].gameObject);
        map.SetEntityType(_tile_index, EntityType.NONE);

        if (player_spawn != null && player_spawn.tile_index == _tile_index)
        {
            player_spawn = null;
        }
        else if (level_exit != null && level_exit.tile_index == _tile_index)
        {
            level_exit = null;
        }

        dungeon_entities.Remove(_tile_index);
    }


    public void RefreshAutoTileIDs()
    {
        map.RefreshAutoTileIDs();

        for (int i = 0; i < map_area; ++i)
        {
            SpriteRenderer tile = sprite_tiles[i];

            if (map.TileEmpty(i))
            {
                tile.sprite = null;
            }
            else
            {
                var sprite_list = sprites_list[(int)map.GetTerrainType(i) - 1];
                tile.sprite = sprite_list[map.GetAutoTileID(i)];
            }

            EntityType residing_entity = map.GetEntityType(i);
            UpdateAutoRotatingEntity(i, residing_entity);
        }
    }


    public void VisualisePartition(int _from_index, int _to_index)
    {
        if (!JHelper.ValidIndex(_from_index, map_area) ||
            !JHelper.ValidIndex(_to_index, map_area))
        {
            Debug.Log("Add Partition Visualisation out of bounds");
            return;
        }

        Vector3 tl = sprite_tiles[_from_index].transform.position - new Vector3(half_tile_size.x, -half_tile_size.y);
        Vector3 br = sprite_tiles[_to_index].transform.position + new Vector3(half_tile_size.x, -half_tile_size.y);

        GameObject obj = new GameObject("PartitionVisualisation");
        obj.transform.SetParent(lines_container);
        obj.isStatic = true;

        LineRenderer line = obj.AddComponent<LineRenderer>();
        line.receiveShadows = false;
        line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        line.positionCount = 5;

        line.SetPosition(0, tl);
        line.SetPosition(1, new Vector3(br.x, tl.y));
        line.SetPosition(2, br);
        line.SetPosition(3, new Vector3(tl.x, br.y));
        line.SetPosition(4, tl);

        line.material = line_material;
        line.startColor = line_color;
        line.endColor = line_color;

        line.sortingLayerID = SortingLayer.GetLayerValueFromName(sorting_layer);
        line.sortingOrder = order_in_layer;
    }


    public void VisualiseRoomGrid(RoomGrid _grid)
    {
        if (_grid == null)
            return;

        for (int i = 0; i < _grid.data.Length; ++i)
        {
            DataType data = _grid.data[i];
            if (data == DataType.EMPTY)
                continue;

            int x = _grid.x + (i % _grid.width);
            int y = _grid.y + (i / _grid.width);

            int index = JHelper.CalculateIndex(x, y, map_columns);
            Paint(index, TerrainType.STONE, true);

            switch (data)
            {
                case DataType.DOOR:             AddEntity(index, EntityType.DOOR            ); break;
                case DataType.SPAWN:            AddEntity(index, EntityType.PLAYER_SPAWN    ); break;
                case DataType.EXIT:             AddEntity(index, EntityType.STAIRS          ); break;
                case DataType.ENEMY_EASY:       AddEntity(index, EntityType.ENEMY_EASY      ); break;
                case DataType.ENEMY_HARD:       AddEntity(index, EntityType.ENEMY_HARD      ); break;
                case DataType.TREASURE_HEALTH:  AddEntity(index, EntityType.POTION_HEALTH   ); break;
                case DataType.TREASURE_MANA:    AddEntity(index, EntityType.POTION_MANA     ); break;
                case DataType.TREASURE:         AddEntity(index, EntityType.TREASURE        ); break;
            }
        }

        RefreshAutoTileIDs();
    }


    void Start()
    {
        for (int i = 0; i < map_textures.Count; ++i)
        {
            sprites_list.Add(Resources.LoadAll<Sprite>(map_textures[i].name));
        }

        map = new Map();
        dungeon = new Dungeon(this);
    }


    void Update()
    {
        half_tile_size = tile_size / 2;
        lines_container.gameObject.SetActive(draw_partition_lines);
    }


    void CleanUp()
    {
        if (map_different)
        {
            ClearSpriteTiles();
        }

        ClearDungeonEntities();
        ClearPartitionLines();
        ClearMapEditor();
    }


    void ClearSpriteTiles()
    {
        foreach (Transform child in map_container)
            Destroy(child.gameObject);

        sprite_tiles.Clear();
    }


    void ClearDungeonEntities()
    {
        foreach (Transform child in entities_container)
            Destroy(child.gameObject);

        dungeon_entities.Clear();
    }


    void ClearPartitionLines()
    {
        foreach (Transform child in lines_container)
            Destroy(child.gameObject);
    }


    void ClearMapEditor()
    {
        if (map_editor != null)
        {
            Destroy(map_editor.gameObject);
            map_editor = null;
        }
    }


    void VisualiseMap()
    {
        CleanUp();

        if (map_different)
        {
            CreateBounds();
            CreateGrid();

            prev_map_columns = map_columns;
            prev_map_rows = map_rows;
        }

        for (int i = 0; i < map_area; ++i)
        {
            if (map.TileEmpty(i))
            {
                sprite_tiles[i].sprite = null;
            }
            else
            {
                var sprite_list = sprites_list[(int)map.GetTerrainType(i) - 1];
                sprite_tiles[i].sprite = sprite_list[map.GetAutoTileID(i)];
            }

            EntityType etype = map.GetEntityType(i);
            if (etype != EntityType.NONE)
            {
                AddEntity(i, etype);
            }
        }
    }


    void CreateBounds()
    {
        float map_center_x = (half_tile_size.x * map_columns) - half_tile_size.x;
        float map_center_y = -((half_tile_size.y * map_rows) - half_tile_size.y);

        map_center = new Vector2(map_center_x, map_center_y);
        map_size = new Vector2(tile_size.x * map_columns, tile_size.y * map_rows);

        map_bounds = new Bounds(map_center, map_size);
    }


    void CreateGrid()
    {
        for (int i = 0; i < map.area; ++i)
        {
            int x = i % map.columns;
            int y = i / map.columns;

            Vector2 pos = new Vector2(x * tile_size_.x, -y * tile_size_.y);

            GameObject clone = Instantiate(tile_prefab, map_container);
            clone.name = "Tile" + i;
            clone.transform.position = pos;

            SpriteRenderer tile = clone.GetComponent<SpriteRenderer>();
            sprite_tiles.Add(tile);
        }
    }


    void UpdateAutoRotatingEntity(int _tile_index, EntityType _entity_type)
    {
        if (!JHelper.ValidIndex(_tile_index, map_area) ||
            !dungeon_entities.ContainsKey(_tile_index))
        {
            return;
        }

        if (_entity_type == EntityType.DOOR ||
            _entity_type == EntityType.LOCKED_DOOR)
        {
            int x = _tile_index % map_columns;
            int y = _tile_index / map_columns;

            int right_index = JHelper.CalculateIndex(x + 1, y, map_columns);
            int left_index = JHelper.CalculateIndex(x - 1, y, map_columns);

            if (map.GetTerrainType(right_index) == TerrainType.ROCK &&
                map.GetTerrainType(left_index) == TerrainType.ROCK)
            {
                Sprite spr = entity_sprites[(int)_entity_type];
                dungeon_entities[_tile_index].SetEntity(_entity_type, spr, _tile_index);
            }
            else
            {
                Sprite spr = entity_sprites[(int)_entity_type + 1];
                dungeon_entities[_tile_index].SetEntity(_entity_type + 1, spr, _tile_index);
            }
        }
    }


    void SetSpawn(int _tile_index)
    {
        if (player_spawn != null && player_spawn.tile_index != _tile_index)
        {
            RemoveEntity(player_spawn.tile_index);
        }

        player_spawn = dungeon_entities[_tile_index];
    }


    void SetExit(int _tile_index)
    {
        if (level_exit != null && level_exit.tile_index != _tile_index)
        {
            RemoveEntity(level_exit.tile_index);
        }

        level_exit = dungeon_entities[_tile_index];
    }

}
