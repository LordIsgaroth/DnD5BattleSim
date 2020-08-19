using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Equipment : ScriptableObject
{
    [SerializeField] protected int weight;
    [SerializeField] protected int value;
    [SerializeField] protected EquipmentType type;

    public Equipment(string name, int weight, int value, EquipmentType type)
    {
        this.name = name;
        this.weight = weight;
        this.value = value;
        this.type = type;
    }

    public string Name { get { return name; } }

}
