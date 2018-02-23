using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int gold { get; private set; }
    public int health_potions { get; private set; }
    public int mana_potions { get; private set; }

    public CustomEvents.IntEvent gold_changed_events = new CustomEvents.IntEvent();
    public CustomEvents.IntEvent health_potions_changed_events = new CustomEvents.IntEvent();
    public CustomEvents.IntEvent mana_potions_changed_events = new CustomEvents.IntEvent();


    public void ResetInventory()
    {
        gold = 0;
        health_potions = 0;
        mana_potions = 0;
    }


    public void ModifyGold(int _value)
    {
        gold += _value;
        gold_changed_events.Invoke(gold);
    }


    public void AddHealthPotion()
    {
        ++health_potions;
        health_potions_changed_events.Invoke(health_potions);
    }


    public void RemoveHealthPotion()
    {
        --health_potions;
        health_potions_changed_events.Invoke(health_potions);
    }


    public void AddManaPotion()
    {
        ++mana_potions;
        mana_potions_changed_events.Invoke(mana_potions);
    }


    public void RemoveManaPotion()
    {
        --mana_potions;
        mana_potions_changed_events.Invoke(mana_potions);
    }

}
