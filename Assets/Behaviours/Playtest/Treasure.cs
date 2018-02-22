using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    public enum TreasureType
    {
        POTION_HEALTH,
        POTION_MANA,
        GOLD
    }

    [SerializeField] TreasureType type;
    [SerializeField] int min_value = 5;
    [SerializeField] int max_value = 10;

    private int value
    {
        get
        {
            return Random.Range(min_value, max_value);
        }
    }


    void OnTriggerEnter(Collider _other)
    {
        if (!_other.CompareTag("Player"))
            return;

        var inventory = _other.GetComponent<PlayerInventory>();

        switch (type)
        {
            case TreasureType.POTION_HEALTH: inventory.AddHealthPotion(); break;
            case TreasureType.POTION_MANA:   inventory.AddManaPotion(); break;
            case TreasureType.GOLD:          inventory.ModifyGold(value); break;
        }

        Destroy(this.gameObject);
    }

}
