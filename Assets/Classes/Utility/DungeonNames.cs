using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class DungeonNames
{
    public List<string> name_prefixes = new List<string>();
    public List<string> name_suffixes = new List<string>();


    public DungeonNames(JsonData _data)
    {
        EnumerateList(_data["prefixes"], name_prefixes);
        EnumerateList(_data["suffixes"], name_suffixes);
    }


    void EnumerateList(JsonData _jsonlist, List<string> _namelist)
    {
        for (int i = 0; i < _jsonlist.Count; ++i)
        {
            _namelist.Add((string)_jsonlist[i]);
        }
    }

}
