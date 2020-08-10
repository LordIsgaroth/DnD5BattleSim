
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
    private static GameController gameController;
    private static int counter;

    static CharacterMovement()
    {
        normalTerrain = FindObjectByName("NormalTerrain").GetComponent<Tilemap>();
        difficultTerrain = FindObjectByName("DifficultTerrain").GetComponent<Tilemap>();
        impassableTerrain = FindObjectByName("ImpassableTerrain").GetComponent<Tilemap>();
        gameController = FindObjectByName("GameManager").GetComponent<GameController>();
    }    

    public static Hashtable GetAvalibleTiles(Character character)//Vector3Int position, int speed, int multiplier)
    {
        Hashtable avalibleTiles = new Hashtable();
        HashSet<Vector3Int> unavailibeTiles = new HashSet<Vector3Int>();

        counter = 0;

        Vector3Int position = Vector3Int.FloorToInt(character.transform.position);

        //Тайл, где стоит персонаж, должен остаться недоступным
        unavailibeTiles.Add(position);

        //Расчет стоимости передвижения по доступным тайлам
        calculateNeighborCost(avalibleTiles, unavailibeTiles, position, position, character.CurrentSpeed, character, 0);

        Debug.Log(counter);

        return avalibleTiles;
    }

    private static void calculateNeighborCost(Hashtable avalibleTiles, HashSet<Vector3Int> unavailibeTiles, Vector3Int currentPosition, Vector3Int prevPosition, int remainingSpeed, Character character, int totalCost)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (!(x == 0 && y == 0) && (Mathf.Abs(x) != Mathf.Abs(y)))
                {                    
                    Vector3Int neighbor = new Vector3Int(currentPosition.x + x, currentPosition.y + y, currentPosition.z);

                    if (unavailibeTiles.Contains(neighbor) || neighbor == prevPosition)
                    {
                        continue;
                    }

                    if (tilemapContainsPosition(impassableTerrain, neighbor) || !tilemapContainsPosition(normalTerrain, neighbor))
                    {
                        unavailibeTiles.Add(neighbor);
                        continue;
                    }

                    Character characterAtPosition = gameController.CharacterAtPosition(neighbor);
                    bool friendlyCharacterAtPosition = false;

                    if (characterAtPosition)
                    {
                        unavailibeTiles.Add(neighbor);

                        if(characterAtPosition.Team == character.Team)
                        {
                            friendlyCharacterAtPosition = true;
                        }
                        else
                        {
                            continue;
                        }                        
                    }

                    counter++;

                    int currentMultiplier = 1;

                    if(!character.isFlying)
                    {
                        if (character.isCrawling)
                        {
                            currentMultiplier++;
                        }

                        if (tilemapContainsPosition(difficultTerrain, neighbor) | friendlyCharacterAtPosition)
                        {
                            currentMultiplier++;
                        }
                    }                    

                    int neighborCost = tileCost * currentMultiplier;
                    int currentTotalCost = totalCost + neighborCost;

                    if (neighborCost <= remainingSpeed)
                    {
                        if (!avalibleTiles.Contains(neighbor))
                        {
                            if(!friendlyCharacterAtPosition)
                                avalibleTiles.Add(neighbor, currentTotalCost);                            
                        }
                        else if ((int)avalibleTiles[neighbor] > currentTotalCost)
                        {
                            if (!friendlyCharacterAtPosition)
                                avalibleTiles[neighbor] = currentTotalCost;
                        }
                        else
                        {
                            continue;
                        }

                        calculateNeighborCost(avalibleTiles, unavailibeTiles, neighbor, currentPosition, remainingSpeed - neighborCost, character, currentTotalCost);
                    }                    
                }
            }
        }
    }

    private static bool tilemapContainsPosition(Tilemap tilemap, Vector3Int position)
    {
        if(tilemap.GetTile(position))
        {
            return true;
        }
        else
        {
            return false;
        }       
    }

    private static GameObject FindObjectByName(string name)
    {
        return GameObject.Find(name);
    }
}
