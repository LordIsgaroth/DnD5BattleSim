
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public static class CharacterMovement
{
    private const int tileCost = 5;

    public static Hashtable GetAvalibleTiles(Vector3Int position, int speed, int multiplier)
    {
        Hashtable avalibleTiles = new Hashtable();
        HashSet<Vector3Int> unavailibeTiles = new HashSet<Vector3Int>();

        //Тайл, где стоит персонаж, должен остаться недоступным
        unavailibeTiles.Add(position);

        //Расчет стоимости передвижения по доступным тайлам
        calculateNeighborCost(avalibleTiles, position, speed, multiplier);

        return avalibleTiles;
    }

    private static void calculateNeighborCost(Hashtable avalibleTiles, Vector3Int currentPosition, int remainingSpeed, int multiplier)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (!(x == 0 && y == 0) && (Mathf.Abs(x) != Mathf.Abs(y)))
                {
                    Vector3Int neighbor = new Vector3Int(currentPosition.x + x, currentPosition.y + y, currentPosition.z);

                    int currentMultiplier = multiplier;
                    int neighborCost = tileCost * currentMultiplier;                    

                    if(!avalibleTiles.Contains(neighbor))
                    {
                        avalibleTiles.Add(neighbor, neighborCost);
                    }

                    remainingSpeed -= neighborCost;
                }
            }
        }
    }
}
