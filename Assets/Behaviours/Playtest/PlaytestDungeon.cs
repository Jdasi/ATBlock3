using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaytestDungeon : MonoBehaviour
{
    [SerializeField] List<GameObject> dungeon_tile_prefabs;
    [SerializeField] Transform tile_container;

    private List<GameObject> dungeon_tiles = new List<GameObject>();
    private float model_scale = 2;


    public void InitialiseDungeon(PackedMap _pmap)
    {
        CleanUp();

        for (int row = 0; row < _pmap.rows; ++row)
        {
            for (int col = 0; col < _pmap.columns; ++col)
            {
                int index = JHelper.CalculateIndex(col, row, _pmap.columns);
                if (_pmap.tile_terraintype_ids[index] != (int)TerrainType.STONE)
                    continue;

                Vector3 tile_pos = new Vector3(col, 0, -row) * model_scale;

                int autotile_id = _pmap.tile_autoids[index];
                var clone = Instantiate(dungeon_tile_prefabs[autotile_id], tile_container);

                clone.name = "DungeonTile" + index;
                clone.transform.position = tile_pos;

                dungeon_tiles.Add(clone);
            }
        }
    }


    void CleanUp()
    {
        foreach (var obj in dungeon_tiles)
            Destroy(obj);

        dungeon_tiles.Clear();
    }

}
