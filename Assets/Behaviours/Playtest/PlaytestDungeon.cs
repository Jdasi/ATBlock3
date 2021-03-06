﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlaytestDungeon : MonoBehaviour
{
    public UnityEvent dungeon_initialised_events;
    public ExitPortal exit_portal { get; private set; }
    
    public string map_name
    {
        get
        {
            if (pmap == null)
                return "";

            return pmap.name;
        }
    }

    [Header("Dungeon Stuff")]
    [SerializeField] List<GameObject> dungeon_tile_prefabs;
    [SerializeField] Transform tile_container;
    [SerializeField] Transform entity_container;

    [Space]
    [SerializeField] GameObject door_prefab;
    [SerializeField] GameObject easy_enemy_prefab;
    [SerializeField] GameObject hard_enemy_prefab;
    [SerializeField] GameObject potion_health_prefab;
    [SerializeField] GameObject potion_mana_prefab;
    [SerializeField] GameObject treasure_prefab;
    [SerializeField] GameObject exit_prefab;

    private PackedMap pmap;
    private float model_scale = 2;


    public void InitialiseDungeon(PackedMap _pmap)
    {
        if (_pmap == null)
            return;

        CleanUp();

        pmap = _pmap;
        ParseTileData();

        dungeon_initialised_events.Invoke();
    }


    void ParseTileData()
    {
        int tile_count = 0;
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

                clone.name = "DungeonTile" + tile_count++;
                clone.transform.position = tile_pos;
                clone.isStatic = true;

                ParseEntityType(col, row, index, pmap.tile_entitytype_ids[index]);
            }
        }
    }


    void ParseEntityType(int _x, int _y, int _index, int _entityid)
    {
        if (_entityid == (int)EntityType.NONE)
            return;

        GameObject entity = null;
        Vector3 entity_pos = CoordsToTilePos(_x, _y);

        switch ((EntityType)_entityid)
        {
            case EntityType.PLAYER_SPAWN:
            {
                GameManager.scene.player.transform.position = entity_pos;
            } break;

            case EntityType.DOOR:
            {
                int left_index = JHelper.CalculateIndex(_x + 1, _y, pmap.columns);
                int right_index = JHelper.CalculateIndex(_x - 1, _y, pmap.columns);

                bool horizontal = pmap.tile_terraintype_ids[left_index] != (int)TerrainType.STONE &&
                    pmap.tile_terraintype_ids[right_index] != (int)TerrainType.STONE;

                entity = Instantiate(door_prefab, entity_pos, Quaternion.identity);

                if (!horizontal)
                    entity.transform.Rotate(0, 90, 0);
            } break;

            case EntityType.ENEMY_EASY:
            {
                entity = Instantiate(easy_enemy_prefab, entity_pos, Quaternion.identity);
            } break;

            case EntityType.ENEMY_HARD:
            {
                entity = Instantiate(hard_enemy_prefab, entity_pos, Quaternion.identity);
            } break;

            case EntityType.POTION_HEALTH:
            {
                entity = Instantiate(potion_health_prefab, entity_pos, Quaternion.identity);
            } break;

            case EntityType.POTION_MANA:
            {
                entity = Instantiate(potion_mana_prefab, entity_pos, Quaternion.identity);
            } break;

            case EntityType.TREASURE:
            {
                entity = Instantiate(treasure_prefab, entity_pos, Quaternion.identity);
            } break;

            case EntityType.STAIRS:
            {
                entity = Instantiate(exit_prefab, entity_pos, Quaternion.identity);
                exit_portal = entity.GetComponent<ExitPortal>();
            } break;
        }

        if (entity != null)
        {
            entity.transform.SetParent(entity_container);
        }
    }


    void CleanUp()
    {
        foreach (Transform child in tile_container)
            Destroy(child.gameObject);

        foreach (Transform child in entity_container)
            Destroy(child.gameObject);
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
