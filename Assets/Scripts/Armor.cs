using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : Equipment
{
    [SerializeField] private int armorClass;

    public int ArmorClass { get { return armorClass; } }

    public Armor(string name, int weight, int value, EquipmentType type, int armorClass) : base(name, weight, value, type)
    {
        this.armorClass = armorClass;
    }
}
