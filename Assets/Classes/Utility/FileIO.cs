using System.IO;
using UnityEngine;
using LitJson;

public static class FileIO
{
    private static string maps_path = Application.streamingAssetsPath + "/Maps/";


    public static void DeleteMap(string _mapname)
    {
        File.Delete(maps_path + _mapname + ".json");
    }


    public static bool MapExists(string _mapname)
    {
        return File.Exists(maps_path + _mapname);
    }


    public static void ExportMap(Map _map)
    {
        PackedMap pmap = new PackedMap(_map);
        JsonData map_json = JsonMapper.ToJson(pmap);

        File.WriteAllText(maps_path + _map.name + ".json", map_json.ToString());
    }


    public static PackedMap LoadMap(string _name)
    {
        string file_path = maps_path + _name + ".json";

        if (!File.Exists(file_path))
            return null;

        JsonData map_json = JsonMapper.ToObject(File.ReadAllText(file_path));
        return new PackedMap(map_json);
    }

}
