using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dungeon
{
    private IMapManager imap_manager;
    private GenerationSettings settings;

    private List<Leaf> leaves = new List<Leaf>();
    private Leaf root;


    public Dungeon(IMapManager _imap_manager)
    {
        imap_manager = _imap_manager;
    }


    public void GenerateDungeon(GenerationSettings _settings)
    {
        settings = _settings;

        switch (_settings.method)
        {
            case GenerationSettings.GenerationMethod.BSP:
            {
                BSPGeneration();
            } break;
            
            case GenerationSettings.GenerationMethod.MAZE:
            {
                MazeGeneration();
            } break;
        }
    }


    void BSPGeneration()
    {
        leaves.Clear();

        Leaf.min_leaf_size = settings.min_leaf_size;
        Leaf.max_leaf_size = settings.max_leaf_size;
        Leaf.map_columns = settings.columns;

        root = new Leaf(null, 0, 0, settings.columns, settings.rows);
        leaves.Add(root);

        List<Leaf> working_list = new List<Leaf>();
        working_list.Add(root);

        while (working_list.Count > 0)
        {
            Leaf leaf = working_list[0];
            working_list.Remove(leaf);

            if (leaf.is_branch)
                continue;

            bool randomly_split = Random.Range(0, 100) < settings.random_split_chance;
            if (!leaf.BiggerThanMax() && !randomly_split)
                continue;

            if (!leaf.Split())
                continue;

            // List still to process.
            working_list.Add(leaf.left);
            working_list.Add(leaf.right);

            // Final list.
            leaves.Add(leaf.left);
            leaves.Add(leaf.right);
        }

        BSPCreateRoomsAndCorridors();
        BSPCreateDoors();
        BSPCreateSpawnAndExit();
        BSPCreateEntities();

        imap_manager.VisualiseRoomGrid(root.room_grid);
    }


    void BSPCreateRoomsAndCorridors()
    {
        List<Leaf> lowest_leaves = leaves.Where(node => !node.is_branch).ToList();
        foreach (Leaf leaf in lowest_leaves)
        {
            // Create a visualisation of the leaf's partition area.
            imap_manager.VisualisePartition(leaf.start_tile, leaf.end_tile);

            leaf.CreateRoom();
        }

        List<Leaf> parents = leaves.Where(node => node.is_branch).ToList();
        parents.Reverse(); // Start from the lowest level first.

        foreach (Leaf parent in parents)
        {
            // Group and connect the parent's child rooms.
            parent.room_grid = new RoomGrid(parent.left.room_grid, parent.right.room_grid);
        }
    }


    void BSPCreateDoors()
    {
        var room_grid = root.room_grid;
        for (int row = 0; row < room_grid.height; ++row)
        {
            for (int col = 0; col < room_grid.width; ++col)
            {
                // Random door chance.
                bool random_door = Random.Range(0, 100) < settings.door_density;
                if (!random_door)
                    continue;

                // Doors can't be next to grid edges.
                if (col == 0 || col == room_grid.width - 1 ||
                    row == 0 || row == room_grid.height - 1)
                {
                    continue;
                }

                int center = JHelper.CalculateIndex(col, row, room_grid.width);
                if (room_grid.data[center] != DataType.CORRIDOR)
                    continue;

                int right = JHelper.CalculateIndex(col + 1, row, room_grid.width);
                int left = JHelper.CalculateIndex(col - 1, row, room_grid.width);

                int up = JHelper.CalculateIndex(col, row - 1, room_grid.width);
                int down = JHelper.CalculateIndex(col, row + 1, room_grid.width);

                // Check for single width horizontal corridor.
                if (((room_grid.data[right] == DataType.CORRIDOR && room_grid.data[left] == DataType.ROOM) ||
                     (room_grid.data[right] == DataType.ROOM && room_grid.data[left] == DataType.CORRIDOR)) &&
                    (room_grid.data[up] == DataType.EMPTY && room_grid.data[down] == DataType.EMPTY))
                {
                    room_grid.data[center] = DataType.DOOR;
                }
                // Check for single width vertical corridor.
                else if (((room_grid.data[up] == DataType.CORRIDOR && room_grid.data[down] == DataType.ROOM) ||
                          (room_grid.data[up] == DataType.ROOM && room_grid.data[down] == DataType.CORRIDOR)) &&
                         (room_grid.data[left] == DataType.EMPTY && room_grid.data[right] == DataType.EMPTY))
                {
                    room_grid.data[center] = DataType.DOOR;
                }
            }
        }
    }


    void BSPCreateSpawnAndExit()
    {
        RoomGrid left_room = root.left.room_grid.rooms[Random.Range(0, root.left.room_grid.rooms.Count)];
        RoomGrid right_room = root.right.room_grid.rooms[Random.Range(0, root.right.room_grid.rooms.Count)];

        int left_center = RoomGrid.ConvertIndexAtoB((left_room.width / 2), (left_room.height / 2), left_room, root.room_grid);
        int right_center = RoomGrid.ConvertIndexAtoB((right_room.width / 2), (right_room.height / 2), right_room, root.room_grid);

        left_room.type = RoomType.SPAWN;
        right_room.type = RoomType.EXIT;

        root.room_grid.data[left_center] = DataType.SPAWN;
        root.room_grid.data[right_center] = DataType.EXIT;
    }


    void BSPCreateEntities()
    {
        // TODO: spawn monsters/items ..
    }


    void MazeGeneration()
    {
        // Do stuff ..
    }

}
