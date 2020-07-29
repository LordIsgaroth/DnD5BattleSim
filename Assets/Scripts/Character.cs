using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private int speed;
    private int currentSpeed;

    Character()
    {
        RenewMovementSpeed();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
