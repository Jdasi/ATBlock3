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
            
            case GenerationSettings.GenerationMethod.NYSTROM:
            {
                NystromGeneration();
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

            bool randomly_split = Random.Range(0, 1.0f) >= settings.random_split_chance;
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
        BSPCreateDungeonEntities();

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


    void BSPCreateDungeonEntities()
    {
        root.CreateDoors();
    }


    void NystromGeneration()
    {
        // Do stuff ..
    }

}
