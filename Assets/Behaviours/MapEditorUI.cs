using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapEditorUI : MonoBehaviour
{
    [SerializeField] RectTransform menu_panel;

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
    }

}
