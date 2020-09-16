
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class AreaCalculations
{
    private const int tileCost = 5;
    private static Tilemap normalTerrain;
    private static Tilemap difficultTerrain;
    private static Tilemap impassableTerrain;
    private static GameController gameController;
    private static int counter;

    static AreaCalculations()
    {
        normalTerrain = FindObjectByName("NormalTerrain").GetComponent<Tilemap>();
        difficultTerrain = FindObjectByName("DifficultTerrain").GetComponent<Tilemap>();
        impassableTerrain = FindObjectByName("ImpassableTerrain").GetComponent<Tilemap>();
        gameController = FindObjectByName("GameManager").GetComponent<GameController>();
    }    

    public static Hashtable GetTilesAvalibleForMovement(Character character)//Vector3Int position, int speed, int multiplier)
    {
        Hashtable avalibleTiles = new Hashtable();
        HashSet<Vector3Int> unavailibeTiles = new HashSet<Vector3Int>();

        counter = 0;

        Vector3Int position = Vector3Int.FloorToInt(character.transform.position);

        //Тайл, где стоит персонаж, должен остаться недоступным
        unavailibeTiles.Add(position);

        //Расчет стоимости передвижения по доступным тайлам
        CalculateNeighborCost(avalibleTiles, unavailibeTiles, position, position, character.CurrentSpeed, character, 0);

        //Debug.Log(counter);

        return avalibleTiles;
    }

    private static void CalculateNeighborCost(Hashtable avalibleTiles, HashSet<Vector3Int> unavailibeTiles, Vector3Int currentPosition, Vector3Int prevPosition, int remainingSpeed, Character character, int totalCost)
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

                    if (TilemapContainsPosition(impassableTerrain, neighbor) || !TilemapContainsPosition(normalTerrain, neighbor))
                    {
                        unavailibeTiles.Add(neighbor);
                        continue;
                    }

                    Character characterAtPosition = gameController.CharacterAtPosition(neighbor);
                    bool movingThroughCharacter = false;

                    if (characterAtPosition)
                    {
                        unavailibeTiles.Add(neighbor);

                        if(characterAtPosition.Team == character.Team || !characterAtPosition.Conscious)
                        {
                            movingThroughCharacter = true;
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

                        if (TilemapContainsPosition(difficultTerrain, neighbor) | movingThroughCharacter)
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
                            if(!movingThroughCharacter)
                                avalibleTiles.Add(neighbor, currentTotalCost);                            
                        }
                        else if ((int)avalibleTiles[neighbor] > currentTotalCost)
                        {
                            if (!movingThroughCharacter)
                                avalibleTiles[neighbor] = currentTotalCost;
                        }
                        else
                        {
                            continue;
                        }

                        CalculateNeighborCost(avalibleTiles, unavailibeTiles, neighbor, currentPosition, remainingSpeed - neighborCost, character, currentTotalCost);
                    }                    
                }
            }
        }
    }

    private static bool TilemapContainsPosition(Tilemap tilemap, Vector3Int position)
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

    //Функция для определения области поражения - клеток и персонажей, которые попадают в область действия (На данный момент - только сфера)
    public static AreaOfEffect GetAreaOfEffect(Vector3Int source, int range, int team)
    {
        HashSet<Vector3Int> affectedTiles = new HashSet<Vector3Int>();
        HashSet<Vector3Int> unavailibeTiles = new HashSet<Vector3Int>();
        Hashtable affectedCharacters = new Hashtable();

        FindTilesInRange(affectedTiles, unavailibeTiles, affectedCharacters, team, source, range, 0);

        return new AreaOfEffect(affectedTiles, affectedCharacters);
    }

    private static void FindTilesInRange(HashSet<Vector3Int> affectedTiles, HashSet<Vector3Int> unavailibeTiles, Hashtable characters, int team, Vector3Int currentPosition, int totalRange, int currentRange)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (!(x == 0 && y == 0))
                {
                    Vector3Int neighbor = new Vector3Int(currentPosition.x + x, currentPosition.y + y, currentPosition.z);

                    if (unavailibeTiles.Contains(neighbor))
                    {
                        continue;
                    }

                    if (TilemapContainsPosition(impassableTerrain, neighbor) || !TilemapContainsPosition(normalTerrain, neighbor))
                    {
                        unavailibeTiles.Add(neighbor);
                        continue;
                    }                                       

                    int neighborRange = currentRange + tileCost;

                    if (neighborRange <= totalRange)
                    {
                        Character characterAtPosition = gameController.CharacterAtPosition(neighbor);

                        if (characterAtPosition)
                        {
                            unavailibeTiles.Add(neighbor);
                            if (characterAtPosition.Team != team && characterAtPosition.Conscious)
                            {
                                characters.Add(neighbor, characterAtPosition);
                            }                            
                        }
                        else if (!affectedTiles.Contains(neighbor))
                        {
                            affectedTiles.Add(neighbor);
                     
                        }                     

                        FindTilesInRange(affectedTiles, unavailibeTiles, characters, team, neighbor, totalRange, neighborRange);
                    }                    
                }
            }
        }        
    }
}
