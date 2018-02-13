using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GenerationSettings
{
    [Range(1, 100)] public int columns = 20;
    [Range(1, 100)] public int rows = 20;

    [Space]
    [Range(4, 100)] public int min_leaf_size = 5;
    [Range(4, 100)] public int max_leaf_size = 15;

    [Space]
    [Range(0, 100)] public float door_density = 75;
    [Range(0, 100)] public float empty_room_chance = 10;
}
