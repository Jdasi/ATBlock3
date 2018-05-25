using System.IO;
using UnityEngine;
using LitJson;

public static class FileIO
{
    private static string maps_path = Application.streamingAssetsPath + "/Maps/";
    private static string settings_path = Application.streamingAssetsPath + "/game_settings.json";


    public static GameSettings LoadSettings()
    {
        GameSettings settings = new GameSettings();
        JsonData settings_json = JsonMapper.ToObject(File.ReadAllText(settings_path));

        settings.brightness = (double)settings_json["brightness"];

        return settings;
    }


    public static void SaveSettings(GameSettings _settings)
    {
        JsonData settings_json = JsonMapper.ToJson(_settings);
        File.WriteAllText(settings_path, settings_json.ToString());
    }


    public static DungeonNames LoadDungeonNames()
    {
        string file_path = Application.streamingAssetsPath + "/dungeon_names.json";

        if (!File.Exists(file_path))
            return null;

        JsonData names_json = JsonMapper.ToObject(File.ReadAllText(file_path));
        return new DungeonNames(names_json);
    }


    public static void DeleteMap(string _mapname)
    {
        File.Delete(maps_path + _mapname + ".json");
    }


    public static bool MapExists(string _mapname)
    {
        return File.Exists(maps_path + _mapname + ".json");
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
