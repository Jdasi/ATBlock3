using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateAttract : GameState
{
    [SerializeField] PlaytestDungeon playtest_dungeon;
    [SerializeField] string map_to_load = "MenuScene";
    
    private PocketGenerator pocket_generator;


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


    void Awake()
    {
        pocket_generator = new PocketGenerator();
    }


    void Start()
    {
        var map = FileIO.LoadMap(map_to_load);
        playtest_dungeon.InitialiseDungeon(map);
    }


    void Update()
    {

    }

}
