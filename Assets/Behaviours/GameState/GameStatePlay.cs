using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatePlay : GameState
{
    [SerializeField] PlaytestDungeon playtest_dungeon;
    [SerializeField] PlayerCanvas player_canvas;
    
    private PocketGenerator pocket_generator;


    void Awake()
    {
        pocket_generator = new PocketGenerator();
        playtest_dungeon.dungeon_initialised_events.AddListener(DungeonInitialised);
    }


    void Start()
    {
        var map = GameManager.playtest_map == null ? pocket_generator.GeneratePackedMap() : GameManager.playtest_map;
        playtest_dungeon.InitialiseDungeon(map);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }


    void DungeonInitialised()
    {
        GameManager.scene.player.life.on_death_event.AddListener(PlayerDied);
        playtest_dungeon.exit_portal.portal_entered_events.AddListener(PortalEntered);
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
        if (GameManager.endless_playtest)
        {
            playtest_dungeon.InitialiseDungeon(pocket_generator.GeneratePackedMap());
        }
        else
        {
            GameManager.ExitPlaytest();
        }
    }

}
