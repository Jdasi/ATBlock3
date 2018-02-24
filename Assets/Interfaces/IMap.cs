using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMap
{
    int columns { get; }
    int rows { get; }

    TerrainType GetTerrainType(int _index);
    void SetTerrainType(int _index, TerrainType _type);

    EntityType GetEntityType(int _index);
    void SetEntityType(int _index, EntityType _entity_type);

}
