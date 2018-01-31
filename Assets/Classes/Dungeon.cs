using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon
{
    private IMapManager imap_manager;
    private GenerationSettings settings;


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
        Leaf.min_leaf_size = settings.min_leaf_size;
        Leaf.max_leaf_size = settings.max_leaf_size;
        Leaf.map_columns = settings.columns;

        Leaf root = new Leaf(0, 0, settings.columns, settings.rows);

        List<Leaf> leaves = new List<Leaf>();
        leaves.Add(root);

        while (leaves.Count > 0)
        {
            Leaf leaf = leaves[0];
            leaves.Remove(leaf);

            if (leaf.HasChildren())
                continue;

            bool randomly_split = Random.Range(0, 1.0f) >= 0.75f;
            if (!leaf.BiggerThanMax() && !randomly_split)
                continue;

            if (!leaf.Split())
                continue;

            leaves.Add(leaf.left);
            //imap_manager.AddPartitionVisualisation(leaf.left.start_tile, leaf.left.end_tile);

            leaves.Add(leaf.right);
            //imap_manager.AddPartitionVisualisation(leaf.right.start_tile, leaf.right.end_tile);
        }

        root.CreateRooms(imap_manager);
        imap_manager.RefreshAutoTileIDs();
    }


    private void NystromGeneration()
    {
        // Do stuff ..
    }

}
