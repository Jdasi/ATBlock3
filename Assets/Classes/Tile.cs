using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sides
{
    // 8 bit: NW
    TOP,
    // 8 bit: NE
    LEFT,
    RIGHT,
    // 8 bit: SW
    BOTTOM
    // 8 bit: SE
}

public class Tile
{
    public int id = 0;
    public Tile[] neighbours = new Tile[4];

    public int autotile_id = 0;
    public TerrainType terrain_type = TerrainType.NONE;


    public Tile(int _id)
    {
        id = _id;
    }


    public void AddNeighbour(Sides _side, Tile _tile)
    {
        neighbours[(int)_side] = _tile;

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
        autotile_id = 0;
        int bit = 1;

        foreach (Tile neighbour in neighbours)
        {
            bool neighbour_valid = neighbour != null &&
                neighbour.terrain_type == terrain_type;

            autotile_id += neighbour_valid ? bit : 0;

            bit *= 2;
        }
    }

}
