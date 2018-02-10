using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPopulator
{
    private class PopulationSettings
    {
        public PopulationSettings(float _enemy_easy_chance, float _enemy_hard_chance,
            float _potion_health_chance, float _potion_mana_chance,
            float _treasure_chance, int _max_placements)
        {
            enemy_easy_chance = _enemy_easy_chance;
            enemy_hard_chance = _enemy_hard_chance;

            potion_health_chance = _potion_health_chance;
            potion_mana_chance = _potion_mana_chance;

            treasure_chance = _treasure_chance;

            max_placements = _max_placements;
        }

        public float enemy_easy_chance = 0;
        public float enemy_hard_chance = 0;
        
        public float potion_health_chance = 0;
        public float potion_mana_chance = 0;

        public float treasure_chance = 0;

        public int max_placements = 0;
    }

    private GenerationSettings settings;
    private RoomGrid dungeon_grid;

    private PopulationSettings enemies_easy_settings;
    private PopulationSettings enemies_hard_settings;

    private PopulationSettings treasure_sparse_settings;
    private PopulationSettings treasure_hoard_settings;

    private PopulationSettings guarded_treasure_settings;


    public void Init()
    {
        enemies_easy_settings = new PopulationSettings(100, 0, 0, 0, 0, 5);
        enemies_hard_settings = new PopulationSettings(100, 50, 0, 0, 0, 5);

        treasure_sparse_settings = new PopulationSettings(0, 0, 50, 30, 10, 7);
        treasure_hoard_settings = new PopulationSettings(0, 0, 100, 80, 60, 7);

        guarded_treasure_settings = new PopulationSettings(50, 10, 20, 15, 30, 5);
    }


    public void PopulateDungeon(GenerationSettings _settings, RoomGrid _dungeon_grid)
    {
        settings = _settings;
        dungeon_grid = _dungeon_grid;

        PopulateRooms();
    }


    void PopulateRooms()
    {
        foreach (RoomGrid room in dungeon_grid.rooms)
        {
            if (room.type != RoomType.NONE)
                continue;

            bool empty_room = Random.Range(0, 100) <= settings.empty_room_chance;
            if (empty_room)
                continue;

            room.type = (RoomType)Random.Range((int)RoomType.ENEMIES_EASY, (int)RoomType.COUNT);

            switch (room.type)
            {
                case RoomType.ENEMIES_EASY:     PopulateRoom(room, enemies_easy_settings    ); break;
                case RoomType.ENEMIES_HARD:     PopulateRoom(room, enemies_hard_settings    ); break;
                
                case RoomType.TREASURE_SPARSE:  PopulateRoom(room, treasure_sparse_settings ); break;
                case RoomType.TREASURE_HOARD:   PopulateRoom(room, treasure_hoard_settings  ); break;
                
                case RoomType.GUARDED_TREASURE: PopulateRoom(room, guarded_treasure_settings); break;
            }
        }
    }


    void PopulateRoom(RoomGrid _room, PopulationSettings _psettings)
    {
        int placement_attempts = (_room.width * _room.height) / 4;
        int placements = 0;

        for (int i = 0; i < placement_attempts; ++i)
        {
            int x = Random.Range(0, _room.width);
            int y = Random.Range(0, _room.height);

            int index = RoomGrid.ConvertIndexAtoB(x, y, _room, dungeon_grid);
            if (dungeon_grid.data[index] != DataType.ROOM)
                continue;

            float rand = Random.Range(0, 100);

            if (rand < _psettings.enemy_easy_chance)
                dungeon_grid.data[index] = DataType.ENEMY_EASY;
            
            if (rand < _psettings.enemy_hard_chance)
                dungeon_grid.data[index] = DataType.ENEMY_HARD;
            
            if (rand < _psettings.potion_health_chance)
                dungeon_grid.data[index] = DataType.TREASURE_HEALTH;
            
            if (rand < _psettings.potion_mana_chance)
                dungeon_grid.data[index] = DataType.TREASURE_MANA;
            
            if (rand < _psettings.treasure_chance)
                dungeon_grid.data[index] = DataType.TREASURE;

            if (dungeon_grid.data[index] != DataType.ROOM)
                ++placements;

            if (placements >= _psettings.max_placements)
                break;
        }
    }

}
