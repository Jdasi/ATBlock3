using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] Button btn_save;
    [SerializeField] Button btn_load;
    [SerializeField] Button btn_settings;
    [SerializeField] Button btn_regen;
    [SerializeField] Button btn_play;
    [SerializeField] Button btn_editor;

    [Header("Panels")]
    [SerializeField] AppPanel panel_settings;

    [Header("Text")]
    [SerializeField] Text lbl_tooltip;
    [SerializeField] Text lbl_camera_readout;


    public void OnButtonClick()
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
    }


    public void EnableSave()
    {
        btn_save.interactable = true;
    }


    public void ButtonMouseEnter(string _tooltip)
    {
        lbl_tooltip.transform.parent.gameObject.SetActive(true);
        lbl_tooltip.text = _tooltip;
    }


    public void ButtonMouseExit()
    {
        lbl_tooltip.transform.parent.gameObject.SetActive(false);
        lbl_tooltip.text = "";
    }


    public void OpenPanel(string _panel_name)
    {
        if (_panel_name == "SavePanel")
        {
            return;
        }
        else if (_panel_name == "LoadPanel")
        {
            return;
        }
        else if (_panel_name == "SettingsPanel")
        {
            SmartToggle(panel_settings);
        }
    }


    public void ClosePanels()
    {
        panel_settings.Deactivate();
    }


    void Update()
    {
        UpdateCameraReadout();
    }


    void UpdateCameraReadout()
    {
        lbl_camera_readout.text = "Camera Position: " + JHelper.main_camera.transform.position;
        lbl_camera_readout.text += "\nCamera Zoom: " + JHelper.main_camera.orthographicSize;
    }


    void SmartToggle(AppPanel _panel)
    {
        bool already_open = _panel.IsActive();

        // Deactivate all other panels.
        ClosePanels();

        if (!already_open)
            _panel.Activate();
    }

}
