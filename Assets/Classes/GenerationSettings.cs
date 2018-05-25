using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GenerationSettings
{
    [Range(15, 100)] public int columns = 20;
    [Range(15, 100)] public int rows = 20;

    [Space]
    [Range( 5,  20)] public int min_leaf_size = 5;
    [Range( 5,  20)] public int max_leaf_size = 10;

    [Space]
    [Range( 0, 100)] public float door_density = 50;
    [Range( 0, 100)] public float empty_room_chance = 5;

    public DungeonNames dungeon_names;


    public GenerationSettings()
    {
        dungeon_names = FileIO.LoadDungeonNames();
    }


    public void RandomiseDimensions()
    {
        columns = Random.Range(15, 50);
        rows = Random.Range(15, 50);
    }

}
