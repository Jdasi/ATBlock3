using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaytestPanel : AppPanel
{
    [Header("Buttons")]
    [SerializeField] Button btn_single;
    [SerializeField] Button btn_endless;

    [Header("Text")]
    [SerializeField] Text map_invalid_display;


    public override void OnActivate()
    {

    }


    public override void OnDeactivate()
    {
        map_invalid_display.enabled = false;
    }


    public override void OnUpdate()
    {
        bool map_valid = GameManager.scene.map_manager.MapValid();

        btn_single.interactable = map_valid;
        btn_endless.interactable = map_valid;

        map_invalid_display.enabled = !map_valid;
    }


    public void PlaytestSingleButton()
    {
        GameManager.endless_playtest = false;
        StartPlaytest();
    }


    public void PlaytestEndlessButton()
    {
        GameManager.endless_playtest = true;
        StartPlaytest();
    }


    void StartPlaytest()
    {
        GameManager.StartPlaytest(GameManager.scene.map_manager.GetPackedMap());
    }

}
