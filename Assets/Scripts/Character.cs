using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    private bool actionAvailable;
    private bool conscious;
    private int currentSpeed;
    private int currentMaxHitPoints;
    private int currentHitPoints;
    private int armorClass;
    private int attackRange;
    private int initiative;

    //Событие при потере сознания персонажем
    public UnityEvent onUnconsiousEvent;

    public bool ActionAvailable { get { return actionAvailable; } }
    public bool Conscious { get { return conscious; } }
    public int CurrentSpeed { get { return currentSpeed; } }
    public bool isCrawling { get { return crawling; } }
    public bool isFlying { get { return flying; } }
    public int Team { get { return team; } }
    public int ArmorClass { get { return armorClass; } }
    public int MasteryBonus { get { return masteryBonus; } }
    public int AttackRange { get { return attackRange; } }
    public int Initiative { get { return initiative; } }

    void Start()
    {
        //Для тестирования установим персонажам некоторую экипировку
        if (name == "Warrior")
        {
            onMainHand = new Weapon("Longsword", 3, 15, EquipmentType.FindByShortcut("M"), DiceSet.GetDiceSet("1d8"), DamageType.FindByShortcut("S"), 5);
            armor = new Armor("Chain Mail", 55, 75, EquipmentType.FindByShortcut("HA"), 16);
        }
        else if (name == "Wizard")
        {
            onMainHand = new Weapon("Quarterstaff", 2, 4, EquipmentType.FindByShortcut("M"), DiceSet.GetDiceSet("1d6"), DamageType.FindByShortcut("B"), 5);
        }
        //Если зомби - установим оружие "когти зомби"
        else if (name.Contains("Zombie"))
        {
            onMainHand = new Weapon("Zombie's claws", 0, 0, EquipmentType.FindByShortcut("M"), DiceSet.GetDiceSet("1d6"), DamageType.FindByShortcut("B"), 5);
        }

        InitializeParameters();
        CalculateArmorClass();
        CalculateInitiative();
        DefineAttackRange();
        //Debug.Log(this.name + "'s AC = " + ArmorClass);

        GameController gameController = GameObject.Find("GameManager").GetComponent<GameController>();

        onUnconsiousEvent.AddListener(gameController.CheckVictoriousTeam);
    }
    
    public void Move(Vector3Int newPosition, int cost)
    {
        Transform transform = GetComponent<Transform>();
        transform.position = newPosition;
    }

    private void InitializeParameters()
    {
        conscious = true;

        currentStrenght = strenght;
        currentDexterity = dexterity;

        currentMaxHitPoints = maxHitPoints;
        currentHitPoints = currentMaxHitPoints;

        RenewParameters();
    }

    public void RenewParameters()
    {
        currentSpeed = speed;
        actionAvailable = true;
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

        Debug.Log(armorClass);
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
        //Отметим, что действие в этот ход уже совершалось
        actionAvailable = false;

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

        if (currentHitPoints <= 0)
        {
            SetUnconsious();
        }
    }

    private void SetUnconsious()
    {
        conscious = false;

        //изменение отрисовки спрайта, чтобы персонаж "упал". В дальнейшем вынести в отдельную фунуцию
        Transform spritePosition = transform.Find("Sprite").transform;//gameObject.GetComponentInChildren<Transform>();
        spritePosition.Rotate(0, 0, -90);
        spritePosition.position = new Vector3(spritePosition.position.x, spritePosition.position.y - 0.5f, spritePosition.position.z);

        onUnconsiousEvent.Invoke();
    }

    private void CalculateInitiative()
    {
        //В будущем здесь должен рассчитываться модификатор инициативы, например от черты или особенности класса
        int initiativeModifier = 0;

        //Бросок d20 на инициативу
        DiceSet D20 = DiceSet.GetDiceSet("1d20");
        int D20Roll = 0;
        if (D20 != null) D20Roll = D20.Roll();

        initiative = D20Roll + GetAbilityModifier(currentDexterity) + initiativeModifier;
    }
}
