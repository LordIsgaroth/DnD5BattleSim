using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New dice set", menuName = "Dice set", order = 51)]
public class DiceSet : ScriptableObject
{
    private static Dictionary<string, DiceSet> diceSets = new Dictionary<string, DiceSet>();

    [SerializeField] int quantity;
    [SerializeField] int type;

    void OnEnable()
    {
        diceSets.Add(name, this);
    }

    public DiceSet(int quantity, int value)
    {
        name = quantity + "d" + value;
        diceSets.Add(name, this);
    }

    public int Roll()
    {
        int result = 0;

        for (int i = 1; i <= quantity; i++)
        {
            result += Random.Range(1, type);
        }

        return result;
    }

    public static DiceSet FindByName(string name)
    {
        Debug.Log(name);

        if (diceSets.ContainsKey(name))
            return diceSets[name];

        Debug.Log("!!");

        return null;
    }
}
