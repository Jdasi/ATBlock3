using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditor : MonoBehaviour
{
    public enum PaintMode
    {
        TILES,
        ENTITIES
    }

    [Header("Grid Lines")]
    [SerializeField] float line_thickness = 0.25f;
    [SerializeField] Color line_color = Color.green;
    [SerializeField] Material line_material;
    [SerializeField] string sorting_layer = "Default";
    [SerializeField] int order_in_layer = 75;

    [Header("Highlight Tile")]
    [SerializeField] Color tile_mode_color;
    [SerializeField] Color entity_mode_color;

    [Header("Underlay")]
    [SerializeField] Color underlay_color = Color.black;

    [Header("Events")]
    [SerializeField] CustomEvents.PaintModeEvent mode_changed_events;

    [Header("References")]
    [SerializeField] GameObject grid_lines_container;
    [SerializeField] GameObject highlight_tile;
    [SerializeField] GameObject underlay;
    [SerializeField] MapEditorUI editor_ui;

    private IMapManager imap_manager;
    private Vector3 mouse_pos;
    private int sorting_layer_id;

    private bool can_paint;
    private bool cursor_over_ui;

    private PaintMode paint_mode = PaintMode.TILES;


    public void Init(IMapManager _imap_manager)
    {
        imap_manager = _imap_manager;

        highlight_tile.transform.localScale = _imap_manager.tile_size;
        underlay.transform.position = _imap_manager.map_center;
        underlay.transform.localScale = _imap_manager.map_size;
        underlay.GetComponent<SpriteRenderer>().color = underlay_color;

        InitGridLines();
    }


    public void TogglePaintMode()
    {
        if (paint_mode == PaintMode.TILES)
        {
            SetPaintModeEntities();
        }
        else
        {
            SetPaintModeTiles();
        }
    }


    public void SetPaintModeTiles()
    {
        paint_mode = PaintMode.TILES;
        highlight_tile.GetComponent<SpriteRenderer>().color = tile_mode_color;

        mode_changed_events.Invoke(paint_mode);
    }


    public void SetPaintModeEntities()
    {
        paint_mode = PaintMode.ENTITIES;
        highlight_tile.GetComponent<SpriteRenderer>().color = entity_mode_color;

        mode_changed_events.Invoke(paint_mode);
    }


    void Start()
    {
        if (imap_manager == null)
        {
            Destroy(this.gameObject);
            return;
        }

        SetPaintModeTiles();
    }


    void Update()
    {
        TrackMouse();
        UpdateTileHighlight();

        HandlePainting();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            editor_ui.ToggleMenuVisible();
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
        obj.isStatic = true;

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
        cursor_over_ui = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();

        can_paint = imap_manager.PosInMapBounds(mouse_pos) && !cursor_over_ui;
    }


    void UpdateTileHighlight()
    {
        highlight_tile.SetActive(can_paint);

        if (can_paint)
        {
            highlight_tile.transform.position = imap_manager.PosToTileCenter(mouse_pos);
        }
    }


    void HandlePainting()
    {
        if (!can_paint)
            return;

        if (paint_mode == PaintMode.TILES)
        {
            if (Input.GetMouseButton(0))
            {
                imap_manager.Paint(mouse_pos, TerrainType.STONE);
            }
            else if (Input.GetMouseButton(2))
            {
                imap_manager.Paint(mouse_pos, TerrainType.ROCK);
            }
        }
        else if (paint_mode == PaintMode.ENTITIES)
        {
            if (Input.GetMouseButton(0))
            {
                imap_manager.AddEntity(mouse_pos, editor_ui.selected_entity_type);
            }
            else if (Input.GetMouseButton(2))
            {
                imap_manager.RemoveEntity(mouse_pos);
            }
        }
    }

}
