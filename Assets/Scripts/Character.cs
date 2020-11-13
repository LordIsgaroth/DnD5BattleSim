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
    public int MaxSpeed { get { return speed; } }
    public int CurrentSpeed { get { return currentSpeed; } }
    public bool isCrawling { get { return crawling; } }
    public bool isFlying { get { return flying; } }
    public int Team { get { return team; } }
    public int ArmorClass { get { return armorClass; } }
    public int MasteryBonus { get { return masteryBonus; } }
    public int AttackRange { get { return attackRange; } }
    public int Initiative { get { return initiative; } }
    public int CurrentHp { get { return currentHitPoints; } }
    public int MaxHp { get { return currentMaxHitPoints; } }

    public string AttackData
    {
        get
        {
            string attackData = "";

            DamageType damageType = DamageType.FindByShortcut("B"); // Если оружия в руках нет - тип урона рукопашной атаки по умолчанию дробящий

            if (onMainHand != null)
            {
                int strenghtModifier = GetAbilityModifier(currentStrenght);

                string modStr = "";

                if(strenghtModifier > 0)
                {
                    modStr = "+" + strenghtModifier;
                }
                else if(strenghtModifier < 0)
                {
                    modStr = strenghtModifier.ToString();
                }

                attackData = onMainHand.DamageDice.Name + " " + modStr + " " + onMainHand.DamageType.Name.ToLower();
            }
            else
            {
                attackData = 1 + " " + damageType.Name.ToLower();
            }
             
            return attackData;
        }
    }

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
            //onMainHand = new Weapon("Quarterstaff", 2, 4, EquipmentType.FindByShortcut("M"), DiceSet.GetDiceSet("1d6"), DamageType.FindByShortcut("B"), 5);
            onMainHand = new Weapon("Fire bolt", 0, 0, EquipmentType.FindByShortcut("R"), DiceSet.GetDiceSet("1d10"), DamageType.FindByShortcut("F"), 30);
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

    public Attack PerformMainHandAttack(int targetArmorClass)
    {
        //Отметим, что действие в этот ход уже совершалось
        actionAvailable = false;

        int hitModifier = 0;
        int damageModifier = 0;
        DamageType damageType = DamageType.FindByShortcut("B"); // Если оружия в руках нет - тип урона рукопашной атаки по умолчанию дробящий
        DiceSet diceSet = DiceSet.GetDiceSet("1d1"); //Рукопашная атака наносит 1 повреждение

        if (onMainHand != null)
        {
            int strenghtModifier = GetAbilityModifier(currentStrenght);

            hitModifier = strenghtModifier + masteryBonus;
            damageModifier = strenghtModifier;
            damageType = onMainHand.DamageType;
            diceSet = onMainHand.DamageDice;
        }

        return new Attack(hitModifier, damageModifier, damageType, diceSet);
    }

    public void TakeDamage(int damage)
    {
        //В будущем здесь должна быть проверка на сопротивляемость/неуязвимость к типам урона

        currentHitPoints -= damage;

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
