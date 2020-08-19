using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Library
{
    public static void initializeGameData()
    {
        initializeEquipmentTypes();
        Debug.Log(EquipmentType.FindByShortcut("HA").Name);
    }

    private static void initializeEquipmentTypes()
    {
        /*new EquipmentType("LA", "Light armor");
        new EquipmentType("MA", "Medium armor");
        new EquipmentType("HA", "Heavy armor");
        new EquipmentType("S", "Shield");
        new EquipmentType("M", "Melee weapon");
        new EquipmentType("R", "Ranged weapon");*/
    }
}
