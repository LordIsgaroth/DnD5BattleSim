using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack
{
    private bool successful;
    private int damageValue;
    private readonly DamageType damageType;

    public Attack(bool successful, int damageValue = 0, DamageType damageType = null)
    {
        this.successful = successful;
        this.damageValue = damageValue;
        this.damageType = damageType;
    }

    public bool Successfull { get { return successful; } }
    public int DamageValue { get { return damageValue; } }
    public DamageType DamageType { get { return damageType; } }
}
