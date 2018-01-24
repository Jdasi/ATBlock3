using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AutoIDType
{
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

    private Tile[] tiles;
    private int columns_;
    private int rows_;


    public void CreateMap(int _width, int _height)
    {
        columns = _width;
        rows = _height;

        tiles = new Tile[_width * _height];
        CreateTiles();
    }


    public TerrainType TileTerrainType(int _tile_index)
    {
        if (!JHelper.ValidIndex(_tile_index, area))
            return TerrainType.NONE;

        return tiles[_tile_index].terrain_type;
    }

    
    public bool TileEmpty(int _tile_index)
    {
        if (!JHelper.ValidIndex(_tile_index, area))
            return true;

        return TileTerrainType(_tile_index) == TerrainType.NONE;
    }


    public int GetAutoTileID(int _tile_index)
    {
        if (!JHelper.ValidIndex(_tile_index, area))
            return 0;

        return tiles[_tile_index].autotile_id;
    }


    public void UpdateTerrainType(int _tile_index, TerrainType _type)
    {
        if (!JHelper.ValidIndex(_tile_index, area))
            return;

        Tile tile = tiles[_tile_index];
        tile.terrain_type = _type;

        int x = _tile_index % columns;
        int y = _tile_index / columns;

        for (int row = y - 1; row <= y + 1; ++row)
        {
            for (int col = x - 1; col <= x + 1; ++col)
            {
                if (col < 0 || col >= columns ||
                    row < 0 || row >= rows)
                {
                    continue;
                }

                int index = JHelper.CalculateIndex(col, row, columns);
                tiles[index].CalculateAutoTileID();
            }
        }
    }


    private void CreateTiles()
    {
        for (int i = 0; i < area; ++i)
        {
            Tile tile = new Tile(i);
            tiles[i] = tile;
        }

        FindAllNeighbours();
    }


    private void FindAllNeighbours()
    {
        for (int i = 0; i < area; ++i)
        {
            FindNeighbours(tiles[i]);
        }
    }


    private void FindNeighbours(Tile _tile)
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
