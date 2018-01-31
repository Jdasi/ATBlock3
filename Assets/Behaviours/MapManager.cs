using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMapManager
{
    bool PosInMapBounds(Vector2 _pos);
    Vector2 PosToTileCenter(Vector2 _pos);
    int PosToTileIndex(Vector2 _pos);

    void Paint(Vector2 _pos, TerrainType _terrain_type, bool _update_autoids = true);
    void Paint(int _tile_index, TerrainType _terrain_type, bool _update_autoids = true);
    void AddPartitionVisualisation(int _from_index, int _to_index);

    void RefreshAutoTileIDs();

    int map_columns { get; }
    int map_rows { get; }

    Vector2 tile_size { get; }
    Vector2 half_tile_size { get; }

    Vector2 map_center { get; }
    Vector2 map_size { get; }

    Vector3 map_bounds_min { get; }
    Vector3 map_bounds_max { get; }
}

[System.Serializable]
public class GenerationSettings
{
    public enum GenerationMethod
    {
        BSP,
        NYSTROM
    }

    [Range(1, 100)] public int columns = 30;
    [Range(1, 100)] public int rows = 30;

    public GenerationMethod method;

    public int min_leaf_size = 5;
    public int max_leaf_size = 15;

    [Range(1, 100)] public float random_split_chance = 0.75f;
}

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
    [SerializeField] Texture2D map_texture;
    [SerializeField] Vector2 tile_size_ = new Vector2(16, 16);

    [Header("Prefabs")]
    [SerializeField] GameObject tile_prefab;
    [SerializeField] GameObject editor_prefab;

    [Header("References")]
    [SerializeField] Transform map_container;
    [SerializeField] Transform partition_lines;

    private Map map;
    private Dungeon dungeon;
    private MapEditor map_editor;

    private Sprite[] sprites;
    private List<SpriteRenderer> sprite_tiles = new List<SpriteRenderer>();


    public void CreateMap()
    {
        if (map == null)
            return;

        CleanUp();

        map.CreateMap(settings.columns, settings.rows);

        CreateBounds();
        CreateGrid();

        dungeon.GenerateDungeon(settings);
    }


    public void ToggleMapEditor()
    {
        if (map == null)
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

        int x = index % map.columns;
        int y = index / map.columns;

        return new Vector2(x * tile_size.x, -(y * tile_size.y));
    }


    public int PosToTileIndex(Vector2 _pos)
    {
        float offsetx = _pos.x + half_tile_size.x;
        int ix = (int)(offsetx / tile_size.x);

        float offsety = _pos.y - half_tile_size.y;
        int iy = Mathf.Abs((int)(offsety / tile_size.y));

        return (iy * map.columns) + ix;
    }


    public void Paint(Vector2 _pos, TerrainType _terrain_type, bool _update_autoids)
    {
        if (!PosInMapBounds(_pos))
            return;

        int index = PosToTileIndex(_pos);
        Paint(index, _terrain_type, _update_autoids);
    }


    public void Paint(int _tile_index, TerrainType _terrain_type, bool _update_autoids)
    {
        if (!JHelper.ValidIndex(_tile_index, map_area))
        {
            Debug.Log("Tile Index Out of Bounds: " + _tile_index);
            return;
        }

        map.UpdateTerrainType(_tile_index, _terrain_type);

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
                sprite_tiles[_tile_index].sprite = map.TileEmpty(_tile_index) ?
                    null : sprites[map.GetAutoTileID(_tile_index)];
            }
        }
    }


    public void RefreshAutoTileIDs()
    {
        map.RefreshAutoTileIDs();
    }


    public void AddPartitionVisualisation(int _from_index, int _to_index)
    {
        if (!JHelper.ValidIndex(_from_index, map_area) ||
            !JHelper.ValidIndex(_to_index, map_area))
        {
            return;
        }

        var container = new GameObject("PartitionVisualisation");
        container.transform.SetParent(partition_lines);

        var line = container.AddComponent<LineRenderer>();
        line.positionCount = 5;

        Vector3 tl = sprite_tiles[_from_index].transform.position - (Vector3)tile_size;
        Vector3 br = sprite_tiles[_to_index].transform.position + (Vector3)tile_size;

        //Debug.Log("From Index: " + _from_index + ", To Index: " + _to_index);

        line.SetPosition(0, tl);
        line.SetPosition(1, new Vector3(br.x, tl.y));
        line.SetPosition(2, br);
        line.SetPosition(3, new Vector3(br.y, tl.x));
        line.SetPosition(4, tl);
    }


    void Start()
    {
        sprites = Resources.LoadAll<Sprite>(map_texture.name);

        map = new Map();
        dungeon = new Dungeon(this);
    }


    void Update()
    {
        half_tile_size = tile_size / 2;
    }


    void CleanUp()
    {
        ClearMapContainer();
        ClearPartitionLines();
        ClearSpriteTiles();
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

            SpriteRenderer sr = clone.GetComponent<SpriteRenderer>();
            sr.sprite = map.TileEmpty(i) ? null : sprites[map.GetAutoTileID(i)];

            sprite_tiles.Add(sr);
        }
    }

}
