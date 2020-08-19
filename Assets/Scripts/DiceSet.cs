using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New dice set", menuName = "Dice set", order = 51)]
public class DiceSet : ScriptableObject
{
    [SerializeField] int quantity;
    [SerializeField] int type;
}
