using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private int speed;
    [SerializeField] private int multiplier;

    private int currentSpeed;

    public int CurrentSpeed
    {
        get { return currentSpeed; }
    }

    Character()
    {
        RenewMovementSpeed();
    }
    
    public void Move(Vector3Int newPosition, int cost)
    {
        Transform transform = GetComponent<Transform>();
        transform.position = newPosition;
    }

    void RenewMovementSpeed()
    {
        currentSpeed = speed;
    }
}
