using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGrid
{
    public int top { get; private set; }
    public int bottom { get; private set; }
    public int left { get; private set; }
    public int right { get; private set; }

    public int x { get; private set; }
    public int y { get; private set; }

    public int width { get; private set; }
    public int height { get; private set; }
    public int product { get; private set; }

    public int[] data { get; private set; }

    private List<RoomGrid> rooms = new List<RoomGrid>();


    /// <summary>
    /// Creates a new freestanding RoomGrid.
    /// </summary>
    /// <param name="_x"></param>
    /// <param name="_y"></param>
    /// <param name="_width"></param>
    /// <param name="_height"></param>
    public RoomGrid(int _x, int _y, int _width, int _height)
    {
        InitDimensions(_x, _y, _width, _height);

        // Square room generation.
        for (int i = 0; i < data.Length; ++i)
        {
            data[i] = 1;
        }

        // This RoomGrid contains one room.
        rooms.Add(this);
    }


    /// <summary>
    /// Creates a new RoomGrid from the info of lhs and rhs and creates a
    /// corridor that connects them.
    /// </summary>
    /// <param name="_lhs"></param>
    /// <param name="_rhs"></param>
    public RoomGrid(RoomGrid _lhs, RoomGrid _rhs)
    {
        CollateLists(_lhs, _rhs);

        // Determine the new size of the grid.
        int min_x = Mathf.Min(_lhs.x, _rhs.x);
        int min_y = Mathf.Min(_lhs.y, _rhs.y);

        int max_width = Mathf.Max(_lhs.right, _rhs.right) - min_x;
        int max_height = Mathf.Max(_lhs.bottom, _rhs.bottom) - min_y;

        InitDimensions(min_x, min_y, max_width, max_height);

        // Fill in data with info from both grids.
        ExtractData(_lhs);
        ExtractData(_rhs);

        CreateCorridor(_lhs, _rhs);
    }


    void InitDimensions(int _x, int _y, int _width, int _height)
    {
        x = _x;
        y = _y;

        width = _width;
        height = _height;
        product = _width * _height;

        top = _y;
        bottom = _y + _height;

        left = _x;
        right = _x + _width;

        // Represents solid and empty spaces within the grid.
        data = new int[_width * _height];
    }


    void CollateLists(RoomGrid _lhs, RoomGrid _rhs)
    {
        foreach (RoomGrid room in _lhs.rooms)
            rooms.Add(room);

        foreach (RoomGrid room in _rhs.rooms)
            rooms.Add(room);
    }


    void ExtractData(RoomGrid _grid)
    {
        if (_grid == null)
            return;

        for (int row = 0; row < _grid.height; ++row)
        {
            for (int col = 0; col < _grid.width; ++col)
            {
                int index = JHelper.CalculateIndex(col, row, _grid.width);
                int this_index = JHelper.CalculateIndex(col + _grid.left - left, row + _grid.top - top, width);

                data[this_index] = _grid.data[index];
            }
        }
    }


    void CreateCorridor(RoomGrid _lhs, RoomGrid _rhs)
    {
        Coords lhs_point = GetRandomSolidPoint(_lhs);
        Coords rhs_point = GetRandomSolidPoint(_rhs);

        Coords digger = new Coords(lhs_point.x, lhs_point.y);

        while (digger.x != rhs_point.x || digger.y != rhs_point.y)
        {
            if (digger.x != rhs_point.x)
            {
                int move = digger.x > rhs_point.x ? -1 : 1;
                digger.x += move;
            }
            else if (digger.y != rhs_point.y)
            {
                int move = digger.y > rhs_point.y ? -1 : 1;
                digger.y += move;
            }

            int index = JHelper.CalculateIndex(digger.x, digger.y, width);
            data[index] = 1;
        }
    }


    Coords GetRandomSolidPoint(RoomGrid _grid)
    {
        int tile_index = 0;
        int tile_value = 0;

        while (tile_value == 0)
        {
            tile_index = Random.Range(0, _grid.product);
            tile_value = _grid.data[tile_index];
        }

        return new Coords(
            (tile_index % _grid.width) + _grid.left - left,
            (tile_index / _grid.width) + _grid.top - top);
    }

}
