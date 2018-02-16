using LitJson;

public class PackedMap
{
    public string name;
    public string description;

    public int columns;
    public int rows;

    public int[] tile_autoids;
    public int[] tile_terraintype_ids;
    public int[] tile_entitytype_ids;

    private int area;


    public PackedMap(JsonData _data)
    {
        InitDescriptionData((string)_data["name"], (string)_data["description"]);
        InitTileData((int)_data["columns"], (int)_data["rows"]);

        for (int i = 0; i < area; ++i)
        {
            tile_autoids[i] = (int)_data["tile_autoids"][i];
            tile_terraintype_ids[i] = (int)_data["tile_terraintype_ids"][i];
            tile_entitytype_ids[i] = (int)_data["tile_entitytype_ids"][i];
        }
    }


    public PackedMap(Map _map)
    {
        InitDescriptionData(_map.name, _map.description);
        InitTileData(_map.columns, _map.rows);

        for (int i = 0; i < area; ++i)
        {
            tile_autoids[i] = _map.GetAutoTileID(i);
            tile_terraintype_ids[i] = (int)_map.GetTerrainType(i);
            tile_entitytype_ids[i] = (int)_map.GetEntityType(i);
        }
    }


    void InitDescriptionData(string _name, string _description)
    {
        name = _name;
        description = _description;
    }


    void InitTileData(int _columns, int _rows)
    {
        columns = _columns;
        rows = _rows;

        area = columns * rows;

        tile_autoids = new int[area];
        tile_terraintype_ids = new int[area];
        tile_entitytype_ids = new int[area];
    }

}
