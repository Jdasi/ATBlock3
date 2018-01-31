using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf
{
    public static int min_leaf_size;
    public static int max_leaf_size;

    public static int map_columns;

    public int x { get; private set; }
    public int y { get; private set; }

    public int width { get; private set; }
    public int height { get; private set; }

    public int start_tile { get; private set; }
    public int end_tile { get; private set; }

    public Leaf left { get; private set; }
    public Leaf right { get; private set; }

    public Room room;
    public List<Room> halls;


    public Leaf(int _x, int _y, int _width, int _height)
    {
        x = _x;
        y = _y;
            
        width = _width;
        height = _height;

        start_tile = JHelper.CalculateIndex(x, y, map_columns);
        end_tile = JHelper.CalculateIndex(x + (width - 1), y + (height - 1), map_columns);
    }


    public bool HasChildren()
    {
        return (left != null || right != null);
    }


    public bool BiggerThanMax()
    {
        return (width > max_leaf_size || height > max_leaf_size);
    }


    public bool Split()
    {
        // Prevent erroneous splitting.
        if (HasChildren())
            return false;

        // Determine direction of split.
        bool split_hor = Random.Range(0, 1.0f) > 0.5f;

        if (width > height && (float)width / height >= 1.25f)
        {
            split_hor = false;
        }
        else if (height > width && (float)height / width >= 1.25f)
        {
            split_hor = true;
        }

        // Determine if the leaf is large enough to be split.
        int max = (split_hor ? height : width) - min_leaf_size;
        if (max <= min_leaf_size)
            return false;

        // Determine where to split.
        int split = Random.Range(min_leaf_size, max);

        // Create left and right children based on split direction.
        if (split_hor)
        {
            left = new Leaf(x, y, width, split);
            right = new Leaf(x, y + split, width, height - split);
        }
        else
        {
            left = new Leaf(x, y, split, height);
            right = new Leaf(x + split, y, width - split, height);
        }

        Debug.Log("split created");

        return true;
    }


    public void CreateRooms(IMapManager _imap_manager)
    {
        if (HasChildren())
        {
            if (left != null)
            {
                left.CreateRooms(_imap_manager);
            }

            if (right != null)
            {
                right.CreateRooms(_imap_manager);
            }

            if (left != null && right != null)
            {
                // Create a hallway.
            }
        }
        else
        {
            // Create a room.
            _imap_manager.Paint(start_tile, TerrainType.GRASS);
            _imap_manager.Paint(end_tile, TerrainType.GRASS);

            _imap_manager.AddPartitionVisualisation(start_tile, end_tile);
        }
    }

}
