﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public int map_width { get { return map.columns; } }
    public int map_height { get { return map.rows; } }
    public int map_area { get { return map.area; } }

    public Vector2 tile_size { get { return tile_size_; } }
    public Vector2 half_tile_size { get; private set; }

    public Bounds map_bounds { get; private set; }

    [Header("Map Dimensions")]
    [SerializeField] int map_width_ = 20;
    [SerializeField] int map_height_ = 20;

    [Header("Map Texture")]
    [SerializeField] Texture2D map_texture;
    [SerializeField] Vector2 tile_size_ = new Vector2(16, 16);

    [Header("References")]
    [SerializeField] GameObject map_container;
    [SerializeField] GameObject tile_prefab;
    [SerializeField] GameObject editor_prefab;

    private Map map;
    private MapEditorControls map_editor;
    

    public void CreateMap()
    {
        if (map == null)
            map = new Map();

        CleanUp();

        map.CreateMap(map_width_, map_height_);

        CreateBounds();
        CreateGrid();

        Debug.Log("Created a new " + map.columns + "x" + map.rows + " map");
    }


    public void ToggleMapEditor()
    {
        if (map == null)
            return;

        if (map_editor == null)
        {
            GameObject obj = Instantiate(editor_prefab);

            map_editor = obj.GetComponent<MapEditorControls>();
            map_editor.Init(this);

            Debug.Log("Created a new map editor");
        }
        else
        {
            if (map_editor != null)
            {
                Destroy(map_editor.gameObject);
                map_editor = null;
            }

            Debug.Log("Destroyed map editor");
        }
    }


    public bool PosInMapBounds(Vector2 _pos)
    {
        return map_bounds.Contains(_pos);
    }


    public Vector3 PosToTileCenter(Vector2 _pos)
    {
        int index = PosToTileIndex(_pos);

        int x = index % map.columns;
        int y = index / map.columns;

        return new Vector2(x * tile_size.x, -(y * tile_size.y));
    }


    public int PosToTileIndex(Vector2 _pos)
    {
        float offsetx = _pos.x + half_tile_size.x;
        int ix = (int)(offsetx / tile_size.x);

        float offsety = _pos.y - half_tile_size.y;
        int iy = Mathf.Abs((int)(offsety / tile_size.y));

        return (iy * map.rows) + ix;
    }


    void Start()
    {

    }

    
    void Update()
    {
        half_tile_size = tile_size / 2;
    }


    void CleanUp()
    {
        ClearMapContainer();
        ClearMapEditor();
    }


    void ClearMapContainer()
    {
        foreach (Transform child in map_container.transform)
        {
            Destroy(child.gameObject);
        }
    }


    void ClearMapEditor()
    {
        if (map_editor != null)
        {
            Destroy(map_editor.gameObject);
            map_editor = null;
        }
    }


    void CreateBounds()
    {
        float map_center_x = (half_tile_size.x * map_width) - half_tile_size.x;
        float map_center_y = -((half_tile_size.y * map_height) - half_tile_size.y);

        Vector3 map_center = new Vector2(map_center_x, map_center_y);
        Vector3 map_size = new Vector2(tile_size.x * map_width, tile_size.y * map_height);

        map_bounds = new Bounds(map_center, map_size);
    }


    void CreateGrid()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>(map_texture.name);

        for (int i = 0; i < map.area; ++i)
        {
            int x = i % map.columns;
            int y = i / map.columns;

            Vector2 pos = new Vector2(x * tile_size_.x, -y * tile_size_.y);

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
