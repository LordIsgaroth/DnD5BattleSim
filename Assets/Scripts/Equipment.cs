using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Equipment
{
    protected string name;
    protected int weight;
    protected int value;
    protected EquipmentType type;

    public Equipment(string name, int weight, int value, EquipmentType type)
    {
        this.name = name;
        this.weight = weight;
        this.value = value;
        this.type = type;
    }

    public string Name { get { return name; } }

}
