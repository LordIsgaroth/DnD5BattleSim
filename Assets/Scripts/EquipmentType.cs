using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentType
{
    private static Dictionary<string, EquipmentType> equipmentTypes = new Dictionary<string, EquipmentType>();

    private string shortcut;
    private string name;

    public string Name
    {
        get { return name; }
    }

    public EquipmentType(string shortcut, string name)
    {
        this.shortcut = shortcut;
        this.name = name;
        equipmentTypes.Add(shortcut, this);
    }

    public static EquipmentType FindByShortcut(string shortcut)
    {
        if (equipmentTypes.ContainsKey(shortcut))
            return equipmentTypes[shortcut];
       
        return null;
    }

    /*public static bool operator ==(EquipmentType type1, EquipmentType type2)
    {
        return type1.shortcut == type2.shortcut;
    }*/
}
