using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New equipment type", menuName = "Equipment type", order = 51)]
public class EquipmentType : ScriptableObject
{
    private static Dictionary<string, EquipmentType> equipmentTypes = new Dictionary<string, EquipmentType>();

    [SerializeField] private string shortcut;
    //private string name;

    public string Name
    {
        get { return name; }
    }
    
    public EquipmentType(string shortcut, string name)
    {
        Debug.Log(this.Name);

        this.shortcut = shortcut;
        this.name = name;
        equipmentTypes.Add(shortcut, this);
    }

    void OnEnable()
    {
        equipmentTypes.Add(shortcut, this);
    }

    public static EquipmentType FindByShortcut(string shortcut)
    {
        if (equipmentTypes.ContainsKey(shortcut))
            return equipmentTypes[shortcut];
       
        return null;
    }
}
