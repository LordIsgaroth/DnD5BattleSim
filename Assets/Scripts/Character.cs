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

    [SerializeField] private int maxHitPoints;
    [SerializeField] private int armorClass;
    [SerializeField] private Armor armor;

    private int currentSpeed;
    private int currentHitPoints;
    private int currentArmorClass;

    public int CurrentSpeed { get { return currentSpeed; } }

    public bool isCrawling { get { return crawling; } }

    public bool isFlying { get { return flying; } }

    public int Team { get { return team; } }

    void Start()
    {        
        RenewMovementSpeed();        
    }
    
    public void Move(Vector3Int newPosition, int cost)
    {
        Transform transform = GetComponent<Transform>();
        transform.position = newPosition;
    }

    public void RenewMovementSpeed()
    {
        currentSpeed = speed;
    }

    public void ChangeCurrentSpeedByCost(int cost)
    {
        currentSpeed -= cost;
    }
}
