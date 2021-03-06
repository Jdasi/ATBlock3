﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TempSceneRefs
{
    public MapManager map_manager
    {
        get
        {
            if (map_manager_ == null)
                map_manager_ = GameObject.FindObjectOfType<MapManager>();
    
            return map_manager_;
        }
    }

    public AppUI app_ui
    {
        get
        {
            if (app_ui_ == null)
                app_ui_ = GameObject.FindObjectOfType<AppUI>();

            return app_ui_;
        }
    }

    public PlayerController player
    {
        get
        {
            if (player_ == null)
                player_ = GameObject.FindObjectOfType<PlayerController>();

            return player_;
        }
    }

    public PlaytestDungeon playtest_dungeon
    {
        get
        {
            if (playtest_dungeon_ == null)
                playtest_dungeon_ = GameObject.FindObjectOfType<PlaytestDungeon>();

            return playtest_dungeon_;
        }
    }

    private MapManager map_manager_;
    private AppUI app_ui_;
    private PlayerController player_;
    private PlaytestDungeon playtest_dungeon_;

}
