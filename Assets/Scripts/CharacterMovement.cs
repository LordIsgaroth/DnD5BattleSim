
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class CharacterMovement
{
    private const int tileCost = 5;
    private static Tilemap normalTerrain;
    private static Tilemap difficultTerrain;
    private static Tilemap impassableTerrain;
    private static int counter;

    static CharacterMovement()
    {
        normalTerrain = FindTerrainByName("NormalTerrain").GetComponent<Tilemap>();
        difficultTerrain = FindTerrainByName("DifficultTerrain").GetComponent<Tilemap>();
        impassableTerrain = FindTerrainByName("ImpassableTerrain").GetComponent<Tilemap>();
    }    

    public static Hashtable GetAvalibleTiles(Vector3Int position, int speed, int multiplier)
    {
        Hashtable avalibleTiles = new Hashtable();
        HashSet<Vector3Int> unavailibeTiles = new HashSet<Vector3Int>();

        counter = 0;

        //Тайл, где стоит персонаж, должен остаться недоступным
        unavailibeTiles.Add(position);

        //Расчет стоимости передвижения по доступным тайлам
        calculateNeighborCost(avalibleTiles, unavailibeTiles, position, speed, multiplier, 0);


        Debug.Log(counter);

        return avalibleTiles;
    }

    private static void calculateNeighborCost(Hashtable avalibleTiles, HashSet<Vector3Int> unavailibeTiles, Vector3Int currentPosition, int remainingSpeed, int multiplier, int totalCost)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (!(x == 0 && y == 0) && (Mathf.Abs(x) != Mathf.Abs(y)))
                {
                    counter++;

                    Vector3Int neighbor = new Vector3Int(currentPosition.x + x, currentPosition.y + y, currentPosition.z);

                    if (unavailibeTiles.Contains(neighbor))
                    {
                        continue;
                    }

                    int currentMultiplier = multiplier;
                    int neighborCost = tileCost * currentMultiplier;
                    int currentTotalCost = totalCost + neighborCost;

                    if (neighborCost <= remainingSpeed)
                    {
                        if (!avalibleTiles.Contains(neighbor))
                        {
                            avalibleTiles.Add(neighbor, currentTotalCost);
                        }
                        else if ((int)avalibleTiles[neighbor] > currentTotalCost)
                        {
                            avalibleTiles[neighbor] = currentTotalCost;
                        }

                        calculateNeighborCost(avalibleTiles, unavailibeTiles, neighbor, remainingSpeed - neighborCost, multiplier, currentTotalCost);
                    }                    
                }
            }
        }
    }

    private static GameObject FindTerrainByName(string name)
    {
        return GameObject.Find(name);
    }
}
