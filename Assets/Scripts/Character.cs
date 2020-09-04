using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    //Команда - определяет, какие персонажи дружественные, какие - враждебные
    [SerializeField] private int team;

    //Скорость перемещения в футах (исходная)
    [SerializeField] private int speed;

    //Бонус мастерства
    [SerializeField] private int masteryBonus;

    //Состояния, влияющие на передвижение
    [SerializeField] private bool crawling;
    [SerializeField] private bool flying;

    //Основные характеристики
    [SerializeField] private int strenght;
    [SerializeField] private int dexterity;

    //Текущие значения основных характеристик
    private int currentStrenght;
    private int currentDexterity;

    [SerializeField] private int maxHitPoints;

    //Для тестирования выставлен модификатор public. Далее вернуть private
    public Armor armor;
    public Weapon onMainHand;
    public Equipment onOffHand;
    /*[SerializeField] private Armor armor;
    [SerializeField] private Weapon onMainHand;
    [SerializeField] private Equipment onOffHand;*/

    //Вычисляемые параметры
    private int currentSpeed;
    private int currentMaxHitPoints;
    private int currentHitPoints;
    private int armorClass;
    private int attackRange;

    public int CurrentSpeed { get { return currentSpeed; } }
    public bool isCrawling { get { return crawling; } }
    public bool isFlying { get { return flying; } }
    public int Team { get { return team; } }
    public int ArmorClass { get { return armorClass; } }
    public int MasteryBonus { get { return masteryBonus; } }
    public int AttackRange { get { return attackRange; } }

    void Start()
    {
        RenewParameters();
        CalculateArmorClass();
        DefineAttackRange();
        //Debug.Log(this.name + "'s AC = " + ArmorClass);
    }
    
    public void Move(Vector3Int newPosition, int cost)
    {
        Transform transform = GetComponent<Transform>();
        transform.position = newPosition;
    }

    private void RenewParameters()
    {
        currentStrenght = strenght;
        currentDexterity = dexterity;

        currentMaxHitPoints = maxHitPoints;
        currentHitPoints = currentMaxHitPoints;

        RenewMovementSpeed();
    }

    public void RenewMovementSpeed()
    {
        currentSpeed = speed;
    }

    public void ChangeCurrentSpeedByCost(int cost)
    {
        currentSpeed -= cost;
    }

    private void CalculateArmorClass()
    {
        if (armor != null)
        {
            armorClass = armor.ArmorClass;
        }
        else
        {
            armorClass = 10 + GetAbilityModifier(currentDexterity);
        }
    }

    private void DefineAttackRange()
    {
        if(onMainHand != null)
        {
            attackRange = onMainHand.Range;
        }
        else
        {
            attackRange = 5;
        }
    }

    private int GetAbilityModifier(int abilityValue)
    {
        return (abilityValue - 10) / 2;
    }

    public Attack PerformAttack(int targetArmorClass)
    {
        int damage = 0;
        DamageType type = DamageType.FindByShortcut("B");

        DiceSet D20 = DiceSet.GetDiceSet("1d20");

        int D20Roll = 0;
        if (D20 != null) D20Roll = D20.Roll();

        if (onMainHand != null)
        {
            int strenghtModifier = GetAbilityModifier(currentStrenght);
            int hitValue = D20Roll + masteryBonus + strenghtModifier;

            Debug.Log("Hit value: " + hitValue);

            if (hitValue >= targetArmorClass)
            {
                damage = onMainHand.DealDamage() + strenghtModifier;
                type = onMainHand.DamageType;               
            }
            else
            {
                return new Attack(false);
            }
        }
        else
        {
            //Совершение рукопашной атаки
            int strenghtModifier = GetAbilityModifier(currentStrenght);
            int hitValue = D20Roll + strenghtModifier;

            Debug.Log("Hit value: " + hitValue);

            if (hitValue >= targetArmorClass)
            {
                damage = 1 + strenghtModifier;
            }
            else
            {
                return new Attack(false);
            }
        }

        //Урон не может быть меньше 1
        if (damage < 1) damage = 1;

        Debug.Log("Damage: " + damage);

        return new Attack(true, damage, type);
    }

    public void TakeDamage(Attack attack)
    {
        //В будущем здесь должна быть проверка на сопротивляемость/неуязвимость к типам урона

        currentHitPoints -= attack.DamageValue;
        Debug.Log(gameObject.name + " current HP is " + currentHitPoints);
    }
}
