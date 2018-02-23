using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavePanel : AppPanel
{
    [Header("Buttons")]
    [SerializeField] Button btn_save;

    [Header("Input Fields")]
    [SerializeField] InputField name_field;
    [SerializeField] InputField description_field;

    [Header("Text")]
    [SerializeField] Text columns_display;
    [SerializeField] Text rows_display;


    public override void OnActivate()
    {
        UpdateText();
        UpdateInteractables();

        InvokeRepeating("UpdateInteractables", 0, 0.05f);
    }


    public override void OnDeactivate()
    {
        CancelInvoke();
    }


    public void SaveButton()
    {
        GameManager.scene.map_manager.ExportMap(name_field.text, description_field.text);
        Deactivate();
    }


    public void MapChanged()
    {
        UpdateText();
        UpdateInteractables();
    }


    void UpdateText()
    {
        name_field.text = GameManager.scene.map_manager.map_name;
        description_field.text = GameManager.scene.map_manager.map_description;

        columns_display.text = GameManager.scene.map_manager.map_columns.ToString();
        rows_display.text = GameManager.scene.map_manager.map_rows.ToString();
    }


    void UpdateInteractables()
    {
        btn_save.interactable = FieldsValid();
    }


    bool FieldsValid()
    {
        if (name_field.text == "")
            return false;

        if (description_field.text == "")
            return false;

        return true;
    }

}
