using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditor : MonoBehaviour
{
    [Header("Grid Lines")]
    [SerializeField] float line_thickness = 0.25f;
    [SerializeField] Color line_color = Color.green;
    [SerializeField] Material line_material;

    [Space]
    [SerializeField] string sorting_layer = "Default";
    [SerializeField] int order_in_layer;

    [Space]
    [SerializeField] GameObject grid_lines_container;

    [Header("References")]
    [SerializeField] GameObject highlight_tile;
    [SerializeField] GameObject underlay;

    private IMapManager imap_manager;
    private Vector3 mouse_pos;
    private int sorting_layer_id;

    bool can_click { get { return highlight_tile.activeSelf; } }


    public void Init(IMapManager _imap_manager)
    {
        imap_manager = _imap_manager;

        highlight_tile.transform.localScale = _imap_manager.tile_size;
        underlay.transform.position = _imap_manager.map_center;
        underlay.transform.localScale = _imap_manager.map_size;

        InitGridLines();
    }


    void Start()
    {
        if (imap_manager == null)
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
            int index = imap_manager.PosToTileIndex(mouse_pos);
            Debug.Log("Click at: " + index);
        }
    }


    void InitGridLines()
    {
        sorting_layer_id = SortingLayer.GetLayerValueFromName(sorting_layer);
        Vector3 origin = imap_manager.map_bounds_min;

        // Horizontal lines.
        for (int i = 0; i <= imap_manager.map_rows; ++i)
        {
            Vector3 start = new Vector3(origin.x, origin.y + (i * imap_manager.tile_size.y));
            Vector3 end = new Vector3(imap_manager.map_bounds_max.x, start.y);

            CreateGridLine(start, end);
        }

        // Vertical lines.
        for (int i = 0; i <= imap_manager.map_columns; ++i)
        {
            Vector3 start = new Vector3(origin.x + (i * imap_manager.tile_size.x), origin.y );
            Vector3 end = new Vector3(start.x, imap_manager.map_bounds_max.y);

            CreateGridLine(start, end);
        }
    }


    void CreateGridLine(Vector3 _start, Vector3 _end)
    {
        GameObject obj = new GameObject("GridLine");
        obj.transform.SetParent(grid_lines_container.transform);

        LineRenderer line = obj.AddComponent<LineRenderer>();
        line.receiveShadows = false;
        line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        line.positionCount = 2;
        line.SetPosition(0, _start);
        line.SetPosition(1, _end);

        line.startWidth = line_thickness;
        line.endWidth = line_thickness;

        line.startColor = line_color;
        line.endColor = line_color;
        line.material = line_material;

        line.sortingLayerID = sorting_layer_id;
        line.sortingOrder = order_in_layer;
    }


    void TrackMouse()
    {
        mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }


    void UpdateTileHighlight()
    {
        highlight_tile.SetActive(imap_manager.PosInMapBounds(mouse_pos));

        if (!can_click)
            return;

        highlight_tile.transform.position = imap_manager.PosToTileCenter(mouse_pos);
    }

}
