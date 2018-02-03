using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Side
{
    NORTHWEST,
    NORTH,
    NORTHEAST,
    WEST,
    EAST,
    SOUTHWEST,
    SOUTH,
    SOUTHEAST
}

public class Tile
{
    public int id = 0;
    public Tile[] neighbours = new Tile[8];

    public int autotile_id = 0;
    public TerrainType terrain_type = TerrainType.NONE;

    private static Dictionary<int, int> lookup_table = null;


    public Tile(int _id)
    {
        // The first tile creates the lookup table used by all tiles.
        if (lookup_table == null)
            InitLookupTable();

        id = _id;
    }


    public void AddNeighbour(int _side, Tile _tile)
    {
        neighbours[_side] = _tile;

        CalculateAutoTileID();
    }


    public void RemoveNeighbour(Tile _tile)
    {
        for (int i = 0; i < neighbours.Length; ++i)
        {
            Tile neighbour = neighbours[i];
            if (neighbour == null || neighbour.id != _tile.id)
                continue;

            neighbours[i] = null;
        }

        CalculateAutoTileID();
    }


    public void ClearNeighbours()
    {
        for (int i = 0; i < neighbours.Length; ++i)
        {
            Tile neighbour = neighbours[i];
            if (neighbour == null)
                continue;

            neighbour.RemoveNeighbour(this);
            neighbours[i] = null;
        }

        CalculateAutoTileID();
    }


    public void CalculateAutoTileID()
    {
        int sum = 0;
        int bit = 1;

        int side = 0;
        foreach (Tile neighbour in neighbours)
        {
            bool neighbour_valid = neighbour != null &&
                neighbour.terrain_type == terrain_type;

            if (neighbour_valid && (side == 0 || side == 2 || side == 5 || side == 7))
            {
                neighbour_valid &= ValidCornerTile(side);
            }

            sum += neighbour_valid ? bit : 0;

            bit *= 2;
            ++side;
        }

        autotile_id = lookup_table[sum];
    }


    void InitLookupTable()
    {
        lookup_table = new Dictionary<int, int>();

        lookup_table.Add(  0,  0);
        lookup_table.Add(  2,  1);
        lookup_table.Add(  8,  2);
        lookup_table.Add( 10,  3);
        lookup_table.Add( 11,  4);
        lookup_table.Add( 16,  5);
        lookup_table.Add( 18,  6);
        lookup_table.Add( 22,  7);
        lookup_table.Add( 24,  8);
        lookup_table.Add( 26,  9);
        lookup_table.Add( 27, 10);
        lookup_table.Add( 30, 11);
        lookup_table.Add( 31, 12);
        lookup_table.Add( 64, 13);
        lookup_table.Add( 66, 14);
        lookup_table.Add( 72, 15);
        lookup_table.Add( 74, 16);
        lookup_table.Add( 75, 17);
        lookup_table.Add( 80, 18);
        lookup_table.Add( 82, 19);
        lookup_table.Add( 86, 20);
        lookup_table.Add( 88, 21);
        lookup_table.Add( 90, 22);
        lookup_table.Add( 91, 23);
        lookup_table.Add( 94, 24);
        lookup_table.Add( 95, 25);
        lookup_table.Add(104, 26);
        lookup_table.Add(106, 27);
        lookup_table.Add(107, 28);
        lookup_table.Add(120, 29);
        lookup_table.Add(122, 30);
        lookup_table.Add(123, 31);
        lookup_table.Add(126, 32);
        lookup_table.Add(127, 33);
        lookup_table.Add(208, 34);
        lookup_table.Add(210, 35);
        lookup_table.Add(214, 36);
        lookup_table.Add(216, 37);
        lookup_table.Add(218, 38);
        lookup_table.Add(219, 39);
        lookup_table.Add(222, 40);
        lookup_table.Add(223, 41);
        lookup_table.Add(248, 42);
        lookup_table.Add(250, 43);
        lookup_table.Add(251, 44);
        lookup_table.Add(254, 45);
        lookup_table.Add(255, 46);
    }


    bool ValidCornerTile(int _side)
    {
        Tile neighbour_a = null;
        Tile neighbour_b = null;

        switch ((Side)_side)
        {
            case Side.NORTHWEST:
            {
                neighbour_a = neighbours[(int)Side.NORTH];
                neighbour_b = neighbours[(int)Side.WEST];
            } break;

            case Side.NORTHEAST:
            {
                neighbour_a = neighbours[(int)Side.NORTH];
                neighbour_b = neighbours[(int)Side.EAST];
            } break;

            case Side.SOUTHWEST:
            {
                neighbour_a = neighbours[(int)Side.SOUTH];
                neighbour_b = neighbours[(int)Side.WEST];
            } break;

            case Side.SOUTHEAST:
            {
                neighbour_a = neighbours[(int)Side.SOUTH];
                neighbour_b = neighbours[(int)Side.EAST];
            } break;
        }

        bool a_ok = neighbour_a != null && neighbour_a.terrain_type == terrain_type;
        bool b_ok = neighbour_b != null && neighbour_b.terrain_type == terrain_type;

        return a_ok && b_ok;
    }

}
