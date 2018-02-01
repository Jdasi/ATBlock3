using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Range(4, 100)] public int min_leaf_size = 5;
    [Range(4, 100)] public int max_leaf_size = 15;

    [Range(1, 100)] public float random_split_chance = 0.75f;
}
