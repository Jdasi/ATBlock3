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
    STONE,
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

    private TerrainType starting_terrain_type = TerrainType.ROCK;
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


    public void RefreshAutoTileIDs()
    {
        for (int i = 0; i < area; ++i)
        {
            tiles[i].CalculateAutoTileID();
        }
    }


    void CreateTiles()
    {
        for (int i = 0; i < area; ++i)
        {
            Tile tile = new Tile(i);
            tile.terrain_type = starting_terrain_type;

            tiles[i] = tile;
        }

        FindAllNeighbours();
    }


    void FindAllNeighbours()
    {
        for (int i = 0; i < area; ++i)
        {
            FindNeighbours(tiles[i]);
        }
    }


    void FindNeighbours(Tile _tile)
    {
        int x = _tile.id % columns;
        int y = _tile.id / columns;

        int side = 0;
        for (int row = y - 1; row <= y + 1; ++row)
        {
            for (int col = x - 1; col <= x + 1; ++col)
            {
                if (col < 0 || col >= columns ||
                    row < 0 || row >= rows)
                {
                    ++side;
                    continue;
                }

                // A tile can't be its own neighbour.
                if (col == x && row == y)
                    continue;

                int index = JHelper.CalculateIndex(col, row, columns);
                _tile.AddNeighbour(side, tiles[index]);

                ++side;
            }
        }
    }

}
