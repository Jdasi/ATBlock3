using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatePlay : GameState
{
    [SerializeField] PlaytestDungeon pdungeon;


    void Start()
    {
        pdungeon.InitialiseDungeon(GameManager.playtest_map);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.ExitPlaytest();
            return;
        }
    }

}
