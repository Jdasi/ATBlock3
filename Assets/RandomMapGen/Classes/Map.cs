﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    EMPTY = -1,
    FILLED = 15
}

public enum TerrainType
{
    NONE,
    GRASS,
    ROCK
}

public class Map
{
    public int columns
    {
        get { return columns_; }
        set { columns_ = value; area = columns * rows; }
    }

    public int rows
    {
        get { return rows_; }
        set { rows_ = value; area = columns * rows; }
    }

    public int area { get; private set; }

    public Tile[] tiles;

    private int columns_;
    private int rows_;


    public void CreateMap(int _width, int _height)
    {
        columns = _width;
        rows = _height;

        tiles = new Tile[_width * _height];
        CreateTiles();
    }


    private void CreateTiles()
    {
        for (int i = 0; i < area; ++i)
        {
            Tile tile = new Tile(i);
            tiles[i] = tile;
        }

        FindNeighbours();
    }


    private void FindNeighbours()
    {
        for (int i = 0; i < area; ++i)
        {
            CalculateTileNeighbours(tiles[i]);
        }
    }


    private void CalculateTileNeighbours(Tile _tile)
    {
        int x = _tile.id % columns;
        int y = _tile.id / columns;

        if (y + 1 < rows)
        {
            int i = ((y + 1) * rows) + x;
            //Debug.Log("Adding bottom neighbour: " + i + " to tile: " + _tile.id);

            _tile.AddNeighbour(Sides.BOTTOM, tiles[i]);
        }

        if (x + 1 < columns)
        {
            int i = (y * rows) + (x + 1);
            //Debug.Log("Adding right neighbour: " + i + " to tile: " + _tile.id);

            _tile.AddNeighbour(Sides.RIGHT, tiles[i]);
        }

        if (x - 1 >= 0)
        {
            int i = (y * rows) + (x - 1);
            //Debug.Log("Adding left neighbour: " + i + " to tile: " + _tile.id);

            _tile.AddNeighbour(Sides.LEFT, tiles[i]);
        }

        if (y - 1 >= 0)
        {
            int i = ((y - 1) * rows) + x;
            //Debug.Log("Adding top neighbour: " + i + " to tile: " + _tile.id);

            _tile.AddNeighbour(Sides.TOP, tiles[i]);
        }
    }

}
