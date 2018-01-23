using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMapTester : MonoBehaviour
{
    [Header("Map Dimensions")]
    public int map_width = 20;
    public int map_height = 20;

    [Header("Visualise Map")]
    public GameObject map_container;
    public GameObject tile_prefab;
    public Vector2 tile_size = new Vector2(16, 16);

    [Header("Map Sprites")]
    public Texture2D island_texture;

    public Map map;


    public void CreateMap()
    {
        if (map == null)
            map = new Map();

        ClearMapContainer();

        map.CreateMap(map_width, map_height);

        CreateGrid();

        Debug.Log("Created a new " + map.columns + "x" + map.rows + " map");
    }


    void Start()
    {

    }


    void ClearMapContainer()
    {
        foreach (Transform child in map_container.transform)
        {
            Destroy(child.gameObject);
        }
    }


    void CreateGrid()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>(island_texture.name);

        int area = map.tiles.Length;

        for (int i = 0; i < area; ++i)
        {
            int x = i % map.columns;
            int y = i / map.columns;

            Vector2 pos = new Vector2(x * tile_size.x, -y * tile_size.y);

            GameObject clone = Instantiate(tile_prefab, map_container.transform);
            clone.name = "Tile" + i;
            clone.transform.position = pos;

            Tile tile = map.tiles[i];
            int sprite_id = tile.autotile_id;

            if (sprite_id >= 0)
            {
                SpriteRenderer sr = clone.GetComponent<SpriteRenderer>();
                sr.sprite = sprites[sprite_id];
            }
        }
    }

}
