using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSet
{
    private static Dictionary<string, DiceSet> diceSets = new Dictionary<string, DiceSet>();

    private string name;
    private int quantity;
    private int type;

    public string Name { get { return name; } }

    public DiceSet(int quantity, int type)
    {
        name = quantity + "d" + type;
        this.quantity = quantity;
        this.type = type;

        diceSets.Add(name, this);
    }

    public int Roll()
    {
        int result = 0;

        for (int i = 1; i <= quantity; i++)
        {
            result += UnityEngine.Random.Range(1, type + 1);
        }

        return result;
    }

    public static DiceSet GetDiceSet(string name)
    {
        if (diceSets.ContainsKey(name))
        {
            return diceSets[name];
        }
        else
        {
            Tuple<int, int> numbersFromName = GetNumbersFromName(name);

            if (numbersFromName == null)
            {
                return null;
            }
                       
            return new DiceSet(numbersFromName.Item1, numbersFromName.Item2);
        }        
    }

    private static Tuple<int, int> GetNumbersFromName(string name)
    {
        string[] parts = name.Split('d');

        if (parts.Length != 2)
            return null;

        int quantity;
        int type;

        if(int.TryParse(parts[0], out quantity) && int.TryParse(parts[1], out type))
        {
            //Количество должно быть больше нуля, а значение должно быть равно числу граней одного из используемых в dnd кубов, либо 1 для жёстко заданного урона
            if (quantity > 0 && (type == 1 ||type == 4 || type == 6 || type == 8 || type == 10 || type == 12 || type == 20 || type == 100))
            {
                return new Tuple<int, int>(quantity, type);                
            }
        }

        return null;
    }
}
