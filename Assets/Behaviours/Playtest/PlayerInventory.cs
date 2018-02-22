using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int gold { get; private set; }
    public int health_potions { get; private set; }
    public int mana_potions { get; private set; }


    public void ResetInventory()
    {
        gold = 0;
        health_potions = 0;
        mana_potions = 0;
    }


    public void ModifyGold(int _value)
    {
        gold += _value;
    }


    public void AddHealthPotion()
    {
        ++health_potions;
    }


    public void RemoveHealthPotion()
    {
        --health_potions;
    }


    public void AddManaPotion()
    {
        ++mana_potions;
    }


    public void RemoveManaPotion()
    {
        --mana_potions;
    }

}
