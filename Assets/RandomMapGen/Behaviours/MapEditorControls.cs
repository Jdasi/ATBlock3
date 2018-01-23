using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditorControls : MonoBehaviour
{
    [SerializeField] GameObject highlight_tile;

    private MapManager map_manager; // TODO: should be a ref to an interface or something really ..
    private Vector3 mouse_pos;

    bool can_click { get { return highlight_tile.activeSelf; } }


    public void Init(MapManager _map_manager)
    {
        map_manager = _map_manager;

        highlight_tile.transform.localScale = _map_manager.tile_size;
    }


    void Start()
    {
        if (map_manager == null)
        {
            Destroy(this.gameObject);
            return;
        }
    }


    void Update()
    {
        TrackMouse();
        UpdateTileHighlight();

        if (can_click && Input.GetMouseButtonDown(0))
        {
            int index = map_manager.PosToTileIndex(mouse_pos);
            Debug.Log("Click at: " + index);
        }
    }


    void TrackMouse()
    {
        mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }


    void UpdateTileHighlight()
    {
        highlight_tile.SetActive(map_manager.PosInMapBounds(mouse_pos));

        if (!can_click)
            return;

        highlight_tile.transform.position = map_manager.PosToTileCenter(mouse_pos);
    }

}
