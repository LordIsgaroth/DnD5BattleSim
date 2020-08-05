using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private int speed;
    [SerializeField] private bool crawling;

    private int currentSpeed;

    public int CurrentSpeed
    {
        get { return currentSpeed; }
    }

    public bool isCrawling
    {
        get { return crawling; }
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
