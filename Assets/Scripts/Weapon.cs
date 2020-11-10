using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Equipment
{
    private DiceSet damageDice;
    private DamageType damageType;
    private int range;

    public Weapon(string name, int weight, int value, EquipmentType type, DiceSet damageDice, DamageType damageType, int range) : base(name, weight, value, type)
    {
        this.damageDice = damageDice;
        this.damageType = damageType;
        this.range = range;
    }

    /*public int DealDamage()
    {
        return damageDice.Roll();
    */

    public int Range { get { return range; } }
    public DamageType DamageType { get { return damageType; } }
    public DiceSet DamageDice { get { return damageDice; } }
}
