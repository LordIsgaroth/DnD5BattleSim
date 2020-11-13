using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Library
{
    public static void initializeGameData()
    {
        initializeEquipmentTypes();
        initializeDamageTypes();
        //Debug.Log(EquipmentType.FindByShortcut("HA").Name);
        //Debug.Log(DamageType.FindByShortcut("S").Name);
        //Debug.Log(DiceSet.GetDiceSet("10d10"));
    }

    private static void initializeEquipmentTypes()
    {
        new EquipmentType("LA", "Light armor");
        new EquipmentType("MA", "Medium armor");
        new EquipmentType("HA", "Heavy armor");
        new EquipmentType("S", "Shield");
        new EquipmentType("M", "Melee weapon");
        new EquipmentType("R", "Ranged weapon");
    }

    private static void initializeDamageTypes()
    {
        new DamageType("B", "Bludgeoning");
        new DamageType("P", "Piersing");
        new DamageType("S", "Slashing");
        new DamageType("F", "Fire");
    }
}
