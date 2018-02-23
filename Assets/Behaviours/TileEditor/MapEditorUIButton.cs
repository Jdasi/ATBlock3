using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapEditorUIButton : MonoBehaviour
{
    [SerializeField] EntityType related_entity_type;
    [SerializeField] Color selected_color;
    [SerializeField] Color deselected_color;

    [Header("References")]
    [SerializeField] MapEditorUI editor_ui;
    [SerializeField] Button button;


    public void SetOutlineEnabled(bool _enabled)
    {
        ColorBlock block = button.colors;
        block.normalColor = _enabled ? selected_color : deselected_color;

        button.colors = block;
    }
    

    public void Click()
    {
        if (related_entity_type != EntityType.NONE)
        {
            editor_ui.selected_entity_type = related_entity_type;
        }

        SetOutlineEnabled(true);
    }

}
