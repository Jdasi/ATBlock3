using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadPanel : AppPanel
{
    [Header("Dropdown")]
    [SerializeField] Dropdown dropdown_menu;

    [Header("Buttons")]
    [SerializeField] Button btn_load;

    [Header("Text")]
    [SerializeField] Text name_display;
    [SerializeField] Text description_display;
    [SerializeField] Text columns_display;
    [SerializeField] Text rows_display;

    private PackedMap selected_map;


    public override void OnActivate()
    {
        EnumerateOptions();
        UpdateInteractables();
    }


    public override void OnDeactivate()
    {

    }


    public void DropdownChanged(int _newindex)
    {
        selected_map = FileIO.LoadMap(dropdown_menu.options[dropdown_menu.value].text);
        UpdateInfoDisplay();
    }


    public void LoadButton()
    {
        GameManager.scene.map_manager.LoadMap(selected_map);
        Deactivate();
    }


    void EnumerateOptions()
    {
        dropdown_menu.ClearOptions();

        var info = new DirectoryInfo(Application.streamingAssetsPath + "/Maps");
        var file_info = info.GetFiles();

        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();

        foreach (var file in file_info)
        {
            if (file.Name.Contains(".meta"))
                continue;

            var od = new Dropdown.OptionData();
            od.text = file.Name.Split('.')[0];

            options.Add(od);
        }

        dropdown_menu.AddOptions(options);
    }


    void UpdateInteractables()
    {
        bool options_exist = dropdown_menu.options.Count > 0;

        dropdown_menu.interactable = options_exist;
        btn_load.interactable = options_exist;

        if (options_exist)
        {
            DropdownChanged(0);
        }
    }


    void UpdateInfoDisplay()
    {
        if (selected_map == null)
            return;

        name_display.text = selected_map.name;
        description_display.text = selected_map.description;

        columns_display.text = selected_map.columns.ToString();
        rows_display.text = selected_map.rows.ToString();
    }

}
