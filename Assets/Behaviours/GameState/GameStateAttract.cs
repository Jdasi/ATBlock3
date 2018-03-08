using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateAttract : GameState
{
    [SerializeField] PlaytestDungeon playtest_dungeon;
    [SerializeField] string map_to_load = "MenuScene";
    [SerializeField] Slider brightness_slider;
    

    public void BtnPlay()
    {
        GameManager.endless_playtest = true;
        GameManager.GoToPlaytest();
    }


    public void BtnEditor()
    {
        GameManager.GoToEditor();
    }


    public void BtnQuit()
    {
        Application.Quit();
    }


    public void BrightnessChanged(float _brightness)
    {
        GameManager.UpdateBrightness(_brightness);
    }


    void Start()
    {
        var map = FileIO.LoadMap(map_to_load);
        playtest_dungeon.InitialiseDungeon(map);

        brightness_slider.value = GameManager.brightness;
    }


    void Update()
    {

    }

}
