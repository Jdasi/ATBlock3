using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PocketGenerator
{
    private GenerationSettings settings;
    private Map map;
    private Dungeon dungeon;


    public PocketGenerator()
    {
        settings = new GenerationSettings();
        map = new Map();
        dungeon = new Dungeon();
    }


    public PackedMap GeneratePackedMap()
    {
        settings.RandomiseDimensions();

        map.CreateMap(settings.columns, settings.rows);
        dungeon.GenerateDungeon(settings, map);

        return new PackedMap(map);
    }

}
