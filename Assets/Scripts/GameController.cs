using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Hashtable charactersAndPositions;

    void Start()
    {
        Debug.Log("1");
        charactersAndPositions = new Hashtable();

        GameObject[] allCharacters = GameObject.FindGameObjectsWithTag("Character");

        foreach(GameObject characterObject in allCharacters)
        {
            Character character = characterObject.GetComponent<Character>();
            charactersAndPositions.Add(character, Vector3Int.FloorToInt(characterObject.transform.position));
        }
    }
        
    public void NewRound()
    {
        foreach(Character caracter in charactersAndPositions.Keys)
        {
            caracter.RenewMovementSpeed();
        }
    }

    public void ChangeCharacterPosition(Character character, Vector3Int position)
    {
        charactersAndPositions[character] = position;
    }

    public bool CharacterAtPosition(Vector3Int position)
    {
        Debug.Log("2");
        return false;            
        //return charactersAndPositions.ContainsValue(position);
    }
}
