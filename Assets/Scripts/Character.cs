using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private int speed;
    [SerializeField] private int team;
    [SerializeField] private bool crawling;
    [SerializeField] private bool flying;

    private int currentSpeed;

    public int CurrentSpeed
    {
        get { return currentSpeed; }
    }

    public bool isCrawling
    {
        get { return crawling; }
    }

    public bool isFlying
    {
        get { return flying; }
    }

    public int Team
    {
        get { return team; }
    }

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
