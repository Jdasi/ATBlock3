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

    public Leaf parent { get; private set; }
    public Leaf left { get; private set; }
    public Leaf right { get; private set; }

    public Room room;
    public List<Room> halls;

    public bool is_root { get { return parent == null; } }
    public bool is_branch { get { return left != null || right != null; } }


    public Leaf(Leaf _parent, int _x, int _y, int _width, int _height)
    {
        parent = _parent;

        x = _x;
        y = _y;
            
        width = _width;
        height = _height;

        start_tile = JHelper.CalculateIndex(x, y, map_columns);
        end_tile = JHelper.CalculateIndex(x + (width - 1), y + (height - 1), map_columns);
    }


    public bool BiggerThanMax()
    {
        return (width > max_leaf_size || height > max_leaf_size);
    }


    public bool Split()
    {
        // Prevent erroneous splitting.
        if (is_branch)
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
            // Above/Below Children.
            left = new Leaf(this, x, y, width, split);
            right = new Leaf(this, x, y + split, width, height - split);
        }
        else
        {
            // Side-by-Side Children.
            left = new Leaf(this, x, y, split, height);
            right = new Leaf(this, x + split, y, width - split, height);
        }

        return true;
    }


    public void CreateRooms(IMapManager _imap_manager)
    {
        if (is_branch)
        {
            // Cascade room creation down to children.
            left.CreateRooms(_imap_manager);
            right.CreateRooms(_imap_manager);
        }
        else
        {
            // We are a leaf, so we can create a room.
            _imap_manager.VisualisePartition(start_tile, end_tile);

            // Define room size.
            int room_size_x = Random.Range(3, width - 1);
            int room_size_y = Random.Range(3, height - 1);

            // Define room position.
            int room_pos_x = x + Random.Range(1, width - room_size_x - 1);
            int room_pos_y = y + Random.Range(1, height - room_size_y - 1);

            // Create room.
            room = new Room(room_pos_x, room_pos_y, room_size_x, room_size_y, _imap_manager);
        }
    }

    


}
