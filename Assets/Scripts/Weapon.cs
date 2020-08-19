using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New weapon", menuName = "Weapon", order = 51)]
public class Weapon : Equipment
{
    [SerializeField] DiceSet damage;
    [SerializeField] int range;

    public Weapon(string name, int weight, int value, EquipmentType type) : base(name, weight, value, type)
    {        
    }
}
