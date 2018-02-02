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

    public int[] data { get; private set; }

    private List<RoomGrid> rooms = new List<RoomGrid>();
    private List<RoomGrid> corridors = new List<RoomGrid>();


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

        foreach (RoomGrid corridor in _lhs.corridors)
            corridors.Add(corridor);

        foreach (RoomGrid corridor in _rhs.corridors)
            corridors.Add(corridor);
    }


    void ExtractData(RoomGrid _grid)
    {
        for (int row = 0; row < _grid.height; ++row)
        {
            for (int col = 0; col < _grid.width; ++col)
            {
                int index = JHelper.CalculateIndex(col, row, _grid.width);
                int this_index = JHelper.CalculateIndex(col + _grid.left - x, row + _grid.top - y, width);

                data[this_index] = _grid.data[index];
            }
        }
    }


    void CreateCorridor(RoomGrid _lhs, RoomGrid _rhs)
    {
        
    }

}
