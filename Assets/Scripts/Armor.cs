using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Armor : Equipment
{
    [SerializeField] private int armorClass;

    public int ArmorClass { get { return armorClass; } }

    public Armor(string name, int weight, int value, EquipmentType type, int armorClass) : base(name, weight, value, type)
    {
        this.armorClass = armorClass;
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/New armor item", false, 1)]
    public static void CreateImageData()
    {
        Armor armor = ScriptableObject.CreateInstance<Armor>();
        AssetDatabase.CreateAsset(armor, "Assets/Equipment/New armor item.asset");
        AssetDatabase.Refresh();
    }
    
#endif
}
