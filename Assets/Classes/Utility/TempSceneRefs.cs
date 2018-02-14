using System.Collections;
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

    private MapManager map_manager_;

}
