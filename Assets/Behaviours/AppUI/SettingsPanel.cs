using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPanel : AppPanel
{
    [Header("Sliders")]
    [SerializeField] SettingsSlider slider_columns;
    [SerializeField] SettingsSlider slider_rows;
    [SerializeField] SettingsSlider slider_min_leaf_size;
    [SerializeField] SettingsSlider slider_max_leaf_size;
    [SerializeField] SettingsSlider slider_door_density;
    [SerializeField] SettingsSlider slider_empty_room_chance;


    public override void OnActivate()
    {

    }


    public override void OnDeactivate()
    {

    }


    public void SettingChanged()
    {
        GameManager.scene.map_manager.settings.columns = slider_columns.GetValue();
        GameManager.scene.map_manager.settings.rows = slider_rows.GetValue();
        GameManager.scene.map_manager.settings.min_leaf_size = slider_min_leaf_size.GetValue();
        GameManager.scene.map_manager.settings.max_leaf_size = slider_max_leaf_size.GetValue();
        GameManager.scene.map_manager.settings.door_density = slider_door_density.GetValue();
        GameManager.scene.map_manager.settings.empty_room_chance = slider_empty_room_chance.GetValue();
    }

}
