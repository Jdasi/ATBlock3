using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatePlay : GameState
{
    [SerializeField] PlaytestDungeon pdungeon;
    [SerializeField] PlayerCanvas player_canvas;


    void Start()
    {
        pdungeon.InitialiseDungeon(GameManager.playtest_map);

        GameManager.scene.player.life.on_death_event.AddListener(PlayerDied);
        pdungeon.exit_portal.portal_entered_events.AddListener(PortalEntered);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }


    void TogglePause()
    {
        bool player_alive = GameManager.scene.player.life.IsAlive();

        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            player_canvas.ShowPlaytestMenu(true);

            if (player_alive)
                player_canvas.SetMenuText("Paused");

            GameManager.scene.player.enabled = false;
        }
        else
        {
            if (player_alive)
            {
                Time.timeScale = 1;
                player_canvas.ShowPlaytestMenu(false);

                GameManager.scene.player.enabled = true;
            }
        }
    }


    void PlayerDied(GameObject _obj)
    {
        TogglePause();
        Time.timeScale = 1;

        player_canvas.SetMenuText("You Died");
    }


    void PortalEntered()
    {
        if (GameManager.extended_playtest)
        {
            // TODO: generate a new map and initialise ..
        }
        else
        {
            GameManager.ExitPlaytest();
        }
    }

}
