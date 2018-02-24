using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMapManager
{
    bool PosInMapBounds(Vector2 _pos);
    Vector2 PosToTileCenter(Vector2 _pos);
    int PosToTileIndex(Vector2 _pos);

    void Paint(Vector2 _pos, TerrainType _terrain_type);
    void Paint(int _tile_index, TerrainType _terrain_type);

    void AddEntity(Vector2 _pos, EntityType _entity_type);
    void AddEntity(int _tile_index, EntityType _entity_type);
    void RemoveEntity(Vector2 _pos);
    void RemoveEntity(int _tile_index);

    void RefreshAutoTileIDs();

    string map_name { get; }
    string map_description { get; }

    int map_columns { get; }
    int map_rows { get; }

    Vector2 tile_size { get; }
    Vector2 half_tile_size { get; }

    Vector2 map_center { get; }
    Vector2 map_size { get; }

    Vector3 map_bounds_min { get; }
    Vector3 map_bounds_max { get; }
}
