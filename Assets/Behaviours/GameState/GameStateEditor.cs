using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateEditor : GameState
{

    void Start()
    {
        if (GameManager.playtest_map != null)
        {
            GameManager.scene.map_manager.LoadMap(GameManager.playtest_map);
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.GoToMenu();
        }
    }

}
