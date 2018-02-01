using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMapManager
{
    bool PosInMapBounds(Vector2 _pos);
    Vector2 PosToTileCenter(Vector2 _pos);
    int PosToTileIndex(Vector2 _pos);

    void Paint(Vector2 _pos, TerrainType _terrain_type, bool _update_autoids = true);
    void Paint(int _tile_index, TerrainType _terrain_type, bool _update_autoids = true);
    void VisualisePartition(int _from_index, int _to_index);

    void RefreshAutoTileIDs();

    int map_columns { get; }
    int map_rows { get; }

    Vector2 tile_size { get; }
    Vector2 half_tile_size { get; }

    Vector2 map_center { get; }
    Vector2 map_size { get; }

    Vector3 map_bounds_min { get; }
    Vector3 map_bounds_max { get; }
}
