using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentType : TypeWithShortcut 
{
    private static Dictionary<string, EquipmentType> equipmentTypes = new Dictionary<string, EquipmentType>();

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
}
