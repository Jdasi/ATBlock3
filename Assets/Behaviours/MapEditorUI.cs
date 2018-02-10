using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapEditorUI : MonoBehaviour
{
    public EntityType selected_entity_type { get; private set; }

    [SerializeField] List<EntityType> paintable_types;

    [Header("References")]
    [SerializeField] RectTransform menu_panel;
    [SerializeField] Button btn_paint_mode;

    private bool menu_visible = true;


    public void OnButtonClick()
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
    }


    public void ToggleMenuVisible()
    {
        if (menu_visible)
        {
            HideMenu();
        }
        else
        {
            ShowMenu();
        }
    }


    public void PaintModeChanged(MapEditor.PaintMode _mode)
    {
        if (_mode == MapEditor.PaintMode.TILES)
        {
            string str = "Switch to Entities Mode";
            btn_paint_mode.GetComponentInChildren<Text>().text = str;
        }
        else if (_mode == MapEditor.PaintMode.ENTITIES)
        {
            string str = "Switch to Tiles Mode";
            btn_paint_mode.GetComponentInChildren<Text>().text = str;
        }
    }


    void ShowMenu()
    {
        if (menu_visible)
            return;

        menu_panel.localPosition -= new Vector3(menu_panel.rect.width, 0);

        menu_visible = true;
    }


    void HideMenu()
    {
        if (!menu_visible)
            return;

        menu_panel.localPosition += new Vector3(menu_panel.rect.width, 0);

        menu_visible = false;
    }


    void Start()
    {
        HideMenu();
        CreateButtons();
    }


    void CreateButtons()
    {
        foreach (EntityType type in paintable_types)
        {
            //string str = type.ToString();
        }
    }

}
