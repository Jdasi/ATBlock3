using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaytestDungeon : MonoBehaviour
{
    [Header("Dungeon Stuff")]
    [SerializeField] List<GameObject> dungeon_tile_prefabs;
    [SerializeField] GameObject door_prefab;
    [SerializeField] Transform tile_container;

    [Header("Player Stuff")]
    [SerializeField] GameObject player;

    [Header("Debug")]
    [SerializeField] bool debug;

    private PackedMap pmap;

    private List<GameObject> dungeon_tiles = new List<GameObject>();
    private List<GameObject> dungeon_entities = new List<GameObject>();

    private float model_scale = 2;
    private float half_model_scale;


    public void InitialiseDungeon(PackedMap _pmap)
    {
        if (_pmap == null)
            return;

        CleanUp();

        pmap = _pmap;
        ParseTileData();
    }


    void ParseTileData()
    {
        for (int row = 0; row < pmap.rows; ++row)
        {
            for (int col = 0; col < pmap.columns; ++col)
            {
                int index = JHelper.CalculateIndex(col, row, pmap.columns);
                if (pmap.tile_terraintype_ids[index] != (int)TerrainType.STONE)
                    continue;

                Vector3 tile_pos = CoordsToTilePos(col, row);

                int autotile_id = pmap.tile_autoids[index];
                var clone = Instantiate(dungeon_tile_prefabs[autotile_id], tile_container);

                clone.name = "DungeonTile" + index;
                clone.transform.position = tile_pos;

                dungeon_tiles.Add(clone);

                ParseEntityType(col, row, index, pmap.tile_entitytype_ids[index]);
            }
        }
    }


    void ParseEntityType(int _x, int _y, int _index, int _entityid)
    {
        if (_entityid == (int)EntityType.NONE)
            return;

        Vector3 entity_pos = CoordsToTilePos(_x, _y);

        switch ((EntityType)_entityid)
        {
            case EntityType.PLAYER_SPAWN:
            {
                player.transform.position = entity_pos;
            } break;

            case EntityType.DOOR:
            {
                int left_index = JHelper.CalculateIndex(_x + 1, _y, pmap.columns);
                int right_index = JHelper.CalculateIndex(_x - 1, _y, pmap.columns);

                bool horizontal = pmap.tile_terraintype_ids[left_index] != (int)TerrainType.STONE &&
                    pmap.tile_terraintype_ids[right_index] != (int)TerrainType.STONE;

                GameObject clone = Instantiate(door_prefab, entity_pos, Quaternion.identity);

                if (!horizontal)
                {
                    clone.transform.Rotate(0, 90, 0);
                }

                dungeon_entities.Add(clone);
            } break;
        }
    }


    void CleanUp()
    {
        foreach (var obj in dungeon_tiles)
            Destroy(obj);

        dungeon_tiles.Clear();

        foreach (var obj in dungeon_entities)
            Destroy(obj);

        dungeon_entities.Clear();
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
        int x = _index % pmap.columns;
        int y = _index / pmap.columns;

        return CoordsToTilePos(x, y);
    }


    Vector3 CoordsToTilePos(int _x, int _y)
    {
        return new Vector3(_x, 0, -_y) * model_scale;
    }

}
