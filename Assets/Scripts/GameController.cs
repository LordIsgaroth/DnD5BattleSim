using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private List<Character> allCharacters;

    void Start()
    {
        Library.initializeGameData();

        allCharacters = new List<Character>();

        GameObject[] allCharacterObjects = GameObject.FindGameObjectsWithTag("Character");

        foreach(GameObject characterObject in allCharacterObjects)
        {
            Character character = characterObject.GetComponent<Character>();
            allCharacters.Add(character);
        }
    }
        
    public void NewRound()
    {
        foreach(Character caracter in allCharacters)
        {
            caracter.RenewMovementSpeed();
        }
    }

    public Character CharacterAtPosition(Vector3Int position)
    {
        foreach (Character character in allCharacters)
        {
            Vector3Int characterPosition = Vector3Int.FloorToInt(character.transform.position);
                
            if (characterPosition == position)
            {
                return character;
            }
        }

        return null;
    }
}
