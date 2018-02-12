using System.IO;
using UnityEngine;
using LitJson;

public static class FileIO
{

    public static void ExportMap(Map _map)
    {
        PackedMap pmap = new PackedMap(_map);
        JsonData map_json = JsonMapper.ToJson(pmap);

        File.WriteAllText(Application.streamingAssetsPath + "/map.json", map_json.ToString());
    }


    public static PackedMap LoadMap(string _name)
    {
        string file_name = Application.streamingAssetsPath + "/" + _name;
        JsonData map_json = JsonMapper.ToObject(File.ReadAllText(file_name));

        return new PackedMap(map_json);
    }

}
