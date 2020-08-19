using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Equipment
{ 
    private string name;
    private int weight;
    private int value;
    private EquipmentType type;

    public Equipment(string name, int weight, int value, EquipmentType type)
    {
        this.name = name;
        this.weight = weight;
        this.value = value;
        this.type = type;
    }
}
