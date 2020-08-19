using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    //Команда - определяет, какие персонажи дружественные, какие - враждебные
    [SerializeField] private int team;

    //Скорость перемещения в футах (исходная)
    [SerializeField] private int speed;
    
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
    
    [SerializeField] private Armor armor;
    [SerializeField] private Weapon onMainHand;
    [SerializeField] private Equipment onOffHand;

    //Вычисляемые параметры
    private int currentSpeed;
    private int currentMaxHitPoints;
    private int currentHitPoints;
    private int armorClass;

    public int CurrentSpeed { get { return currentSpeed; } }
    public bool isCrawling { get { return crawling; } }
    public bool isFlying { get { return flying; } }
    public int Team { get { return team; } }
    public int ArmorClass { get { return armorClass; } }

    void Start()
    {
        RenewParameters();
        CalculateArmorClass();
        Debug.Log(this.name + "'s AC = " + ArmorClass);
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

    private int GetAbilityModifier(int abilityValue)
    {
        return (abilityValue - 10) / 2;
    }
}
