using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageType : TypeWithShortcut
{
    private static Dictionary<string, DamageType> damageTypes = new Dictionary<string, DamageType>();

    public string Name
    {
        get { return name; }
    }

    public DamageType(string shortcut, string name)
    {
        this.shortcut = shortcut;
        this.name = name;
        damageTypes.Add(shortcut, this);
    }
    
    public static DamageType FindByShortcut(string shortcut)
    {
        if (damageTypes.ContainsKey(shortcut))
            return damageTypes[shortcut];

        return null;
    }
}
