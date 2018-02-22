using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaytestDungeon : MonoBehaviour
{
    [Header("Dungeon Stuff")]
    [SerializeField] List<GameObject> dungeon_tile_prefabs;
    [SerializeField] Transform tile_container;

    [Header("Player Stuff")]
    [SerializeField] GameObject player;

    [Header("Debug")]
    [SerializeField] bool debug;

    private List<GameObject> dungeon_tiles = new List<GameObject>();
    private int columns;
    private int rows;

    private float model_scale = 2;
    private float half_model_scale;


    public void InitialiseDungeon(PackedMap _pmap)
    {
        if (_pmap == null)
            return;

        CleanUp();

        columns = _pmap.columns;
        rows = _pmap.rows;

        ParseTileData(_pmap);
    }


    void ParseTileData(PackedMap _pmap)
    {
        for (int row = 0; row < _pmap.rows; ++row)
        {
            for (int col = 0; col < _pmap.columns; ++col)
            {
                int index = JHelper.CalculateIndex(col, row, _pmap.columns);
                if (_pmap.tile_terraintype_ids[index] != (int)TerrainType.STONE)
                    continue;

                Vector3 tile_pos = CoordsToTilePos(col, row);

                int autotile_id = _pmap.tile_autoids[index];
                var clone = Instantiate(dungeon_tile_prefabs[autotile_id], tile_container);

                clone.name = "DungeonTile" + index;
                clone.transform.position = tile_pos;

                ParseEntityType(col, row, index, _pmap.tile_entitytype_ids[index]);

                dungeon_tiles.Add(clone);
            }
        }
    }


    void ParseEntityType(int _x, int _y, int _index, int _entityid)
    {
        if (_entityid == (int)EntityType.NONE)
            return;

        switch ((EntityType)_entityid)
        {
            case EntityType.PLAYER_SPAWN:
            {
                Vector3 start_pos = CoordsToTilePos(_x, _y);
                player.transform.position = start_pos;
            } break;
        }
    }


    void CleanUp()
    {
        foreach (var obj in dungeon_tiles)
            Destroy(obj);

        dungeon_tiles.Clear();
    }


    void Awake()
    {
        half_model_scale = model_scale / 2;

        if (debug)
        {
            if (FileIO.MapExists("Map1"))
            {
                GameManager.playtest_map = FileIO.LoadMap("Map1");
                InitialiseDungeon(GameManager.playtest_map);
            }
        }
    }


    Vector3 IndexToTilePos(int _index)
    {
        int x = _index % columns;
        int y = _index / columns;

        return CoordsToTilePos(x, y);
    }


    Vector3 CoordsToTilePos(int _x, int _y)
    {
        return new Vector3(_x, 0, -_y) * model_scale;
    }

}
