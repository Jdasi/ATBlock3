﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dungeon
{
    private IMapManager imap_manager;
    private GenerationSettings settings;

    private List<Leaf> leaves = new List<Leaf>();


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


    private void BSPGeneration()
    {
        leaves.Clear();

        Leaf.min_leaf_size = settings.min_leaf_size;
        Leaf.max_leaf_size = settings.max_leaf_size;
        Leaf.map_columns = settings.columns;

        Leaf root = new Leaf(null, 0, 0, settings.columns, settings.rows);
        leaves.Add(root);

        List<Leaf> working_list = new List<Leaf>();
        working_list.Add(root);

        while (working_list.Count > 0)
        {
            Leaf leaf = working_list[0];
            working_list.Remove(leaf);

            if (leaf.is_branch)
                continue;

            bool randomly_split = Random.Range(0, 1.0f) >= 0.75f;
            if (!leaf.BiggerThanMax() && !randomly_split)
                continue;

            if (!leaf.Split())
                continue;

            working_list.Add(leaf.left);
            working_list.Add(leaf.right);

            leaves.Add(leaf.left);
            leaves.Add(leaf.right);
        }

        root.CreateRooms(imap_manager);

        List<Leaf> parents = leaves.Where(node => !node.is_branch).ToList();
        parents.Reverse(); // Start from the lowest level first.

        foreach (Leaf parent in parents)
        {
            // Connect the parent's child rooms and store the result.
        }

        imap_manager.RefreshAutoTileIDs();
    }


    private void NystromGeneration()
    {
        // Do stuff ..
    }

}
