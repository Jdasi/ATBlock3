using LitJson;

public class PackedMap
{
    public int columns;
    public int rows;

    public int[] tile_ids;
    public int[] tile_autoids;
    public int[] tile_terraintype_ids;
    public int[] tile_entitytype_ids;

    private int area;


    public PackedMap(JsonData _data)
    {
        Init((int)_data["columns"], (int)_data["rows"]);

        for (int i = 0; i < area; ++i)
        {
            tile_ids[i] = (int)_data["tile_ids"][i];
            tile_autoids[i] = (int)_data["tile_autoids"][i];
            tile_terraintype_ids[i] = (int)_data["tile_terraintype_ids"][i];
            tile_entitytype_ids[i] = (int)_data["tile_entitytype_ids"][i];
        }
    }


    public PackedMap(Map _map)
    {
        Init(_map.columns, _map.rows);

        for (int i = 0; i < area; ++i)
        {
            tile_ids[i] = i;
            tile_autoids[i] = _map.GetAutoTileID(i);
            tile_terraintype_ids[i] = (int)_map.GetTerrainType(i);
            tile_entitytype_ids[i] = (int)_map.GetEntityType(i);
        }
    }


    void Init(int _columns, int _rows)
    {
        columns = _columns;
        rows = _rows;

        area = columns * rows;

        tile_ids = new int[area];
        tile_autoids = new int[area];
        tile_terraintype_ids = new int[area];
        tile_entitytype_ids = new int[area];
    }

}
