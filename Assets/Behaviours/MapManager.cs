using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour, IMapManager
{
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
    [SerializeField] GenerationSettings settings;

    [Header("Map Texture")]
    [SerializeField] List<Texture2D> map_textures;
    [SerializeField] List<Sprite> entity_sprites;
    [SerializeField] Vector2 tile_size_ = new Vector2(16, 16);

    [Header("Prefabs")]
    [SerializeField] GameObject tile_prefab;
    [SerializeField] GameObject editor_prefab;
    [SerializeField] GameObject dungeon_entity;

    [Header("References")]
    [SerializeField] Transform map_container;
    [SerializeField] Transform partition_lines;

    [Header("Partition Lines")]
    [SerializeField] string sorting_layer = "Default";
    [SerializeField] int order_in_layer = 76;
    [SerializeField] Material line_material;
    [SerializeField] Color line_color;
    [SerializeField] bool draw_partition_lines = true;

    private Map map;
    private Dungeon dungeon;
    private MapEditor map_editor;

    private List<Sprite[]> sprites_list = new List<Sprite[]>();
    private List<SpriteRenderer> sprite_tiles = new List<SpriteRenderer>();
    private List<DungeonEntity> dungeon_entities = new List<DungeonEntity>();

    private int prev_map_columns = 0;
    private int prev_map_rows = 0;

    private bool map_different
    {
        get
        {
            return settings.columns != prev_map_columns ||
                settings.rows != prev_map_rows;
        }
    }


    public void CreateMap()
    {
        CleanUp();

        map.CreateMap(settings.columns, settings.rows);

        if (map_different)
        {
            CreateBounds();
            CreateGrid();

            prev_map_columns = map_columns;
            prev_map_rows = map_rows;
        }

        dungeon.GenerateDungeon(settings);
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
                    var sprite_list = sprites_list[(int)map.TileTerrainType(_tile_index) - 1];
                    tile.sprite = sprite_list[map.GetAutoTileID(_tile_index)];
                }
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
        bool entity_placed = false;

        foreach (DungeonEntity entity in dungeon_entities)
        {
            if (entity.tile_index != _tile_index)
                continue;

            entity.SetEntity(_entity_type, null, _tile_index);
            entity_placed = true;

            break;
        }

        if (!entity_placed)
        {
            Vector3 pos = TileIndexToTileCenter(_tile_index);
            var clone = Instantiate(dungeon_entity, pos, Quaternion.identity);
            var entity = clone.GetComponent<DungeonEntity>();
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
        foreach (DungeonEntity entity in dungeon_entities)
        {
            if (entity.tile_index != _tile_index)
                continue;

            Destroy(entity.gameObject);
            break;
        }
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
                var sprite_list = sprites_list[(int)map.TileTerrainType(i) - 1];
                tile.sprite = sprite_list[map.GetAutoTileID(i)];
            }
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

        var container = new GameObject("PartitionVisualisation");
        container.transform.SetParent(partition_lines);

        var line = container.AddComponent<LineRenderer>();
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
            if (_grid.data[i] == 0)
                continue;

            int x = _grid.x + (i % _grid.width);
            int y = _grid.y + (i / _grid.width);

            int index = JHelper.CalculateIndex(x, y, map_columns);
            Paint(index, TerrainType.STONE, true);
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
        partition_lines.gameObject.SetActive(draw_partition_lines);
    }


    void CleanUp()
    {
        if (map_different)
        {
            ClearMapContainer();
            ClearSpriteTiles();
        }

        ClearPartitionLines();
        ClearMapEditor();
    }


    void ClearMapContainer()
    {
        foreach (Transform child in map_container)
            Destroy(child.gameObject);
    }


    void ClearPartitionLines()
    {
        foreach (Transform child in partition_lines)
            Destroy(child.gameObject);
    }


    void ClearSpriteTiles()
    {
        sprite_tiles.Clear();
    }


    void ClearMapEditor()
    {
        if (map_editor != null)
        {
            Destroy(map_editor.gameObject);
            map_editor = null;
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

            if (map.TileEmpty(i))
            {
                tile.sprite = null;
            }
            else
            {
                var sprite_list = sprites_list[(int)map.TileTerrainType(i) - 1];
                tile.sprite = sprite_list[map.GetAutoTileID(i)];
            }

            sprite_tiles.Add(tile);
        }
    }

}
