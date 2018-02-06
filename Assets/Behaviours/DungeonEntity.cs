using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEntity : MonoBehaviour
{
    public int tile_index { get; private set; }

    [Header("References")]
    [SerializeField] SpriteRenderer sprite_renderer;

    private EntityType entity_type;


    public void SetEntity(EntityType _type, Sprite _sprite, int _tile_index)
    {
        tile_index = _tile_index;

        entity_type = _type;
        sprite_renderer.sprite = _sprite;

        // TODO: attach some module based on the new entity_type ..
    }

}
