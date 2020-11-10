using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack
{
    private int hitModifier;
    private int damageModifier;
    private readonly DiceSet damageDiceSet;
    private readonly DamageType damageType;
    private bool criticalSuccess;
    private bool criticalFail;

    public bool isCriticalSuccess { get { return criticalSuccess; } }
    public bool isCriticalFail { get { return criticalFail; } }

    public Attack(int hitModifier, int damageModifier, DamageType damageType, DiceSet diceSet)
    {
        this.hitModifier = hitModifier;
        this.damageModifier = damageModifier;
        this.damageDiceSet = diceSet;
        this.damageType = damageType;
    }

    public int HitRoll(Roll20Type type)
    {
        int hitValue = 0;

        DiceSet D20 = DiceSet.GetDiceSet("1d20");

        switch (type)
        {
            case Roll20Type.Normal:
                {
                    hitValue = D20.Roll();                
                    break;
                }

            case Roll20Type.Advantage:
                {
                    int firstRoll = D20.Roll();
                    int secondRoll = D20.Roll();

                    hitValue = Mathf.Max(firstRoll, secondRoll);

                    break;
                }

            case Roll20Type.Disadvantage:
                {
                    int firstRoll = D20.Roll();
                    int secondRoll = D20.Roll();

                    hitValue = Mathf.Max(firstRoll, secondRoll);

                    break;
                }            
        }

        if (hitValue == 1)
        {
            criticalFail = true;
        }
        else if(hitValue == 20)
        {
            criticalSuccess = true;
        }        

        return hitValue + hitModifier;
    }

    public int DamageRoll(bool criticalHit = false)
    {
        int damage;

        if(criticalHit)
        {
            damage = damageDiceSet.Roll() + damageDiceSet.Roll() + damageModifier;
        }
        else
        {
            damage =  damageDiceSet.Roll() + damageModifier;
        }

        if (damage < 1) damage = 1; //Урон не может быть менее 1

        return damage;
    }

    public DamageType DamageType { get { return damageType; } }
}
