using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private List<Character> allCharacters;
    private LinkedList<Character> initiativeTracker;
    private int currentRound;
    [SerializeField] private DiceSet d20;

    public DiceSet D20 { get { return d20; } }
        
    void Start()
    {
        Library.initializeGameData();

        allCharacters = new List<Character>();
        initiativeTracker = new LinkedList<Character>();

        GameObject[] allCharacterObjects = GameObject.FindGameObjectsWithTag("Character");

        foreach(GameObject characterObject in allCharacterObjects)
        {
            Character character = characterObject.GetComponent<Character>();
  
            addCharacterToInitiativeTracker(character);

            //Для тестирования установим воину некоторую экипировку
            if (characterObject.name == "Warrior")
            {
                character.onMainHand = new Weapon("Longsword", 3, 15, EquipmentType.FindByShortcut("M"), DiceSet.GetDiceSet("1d8"), DamageType.FindByShortcut("S"), 5);
            }           
        }

        //Для тестирования выведем на экран трекер инициативы
        foreach (Character character in initiativeTracker)
        {
            Debug.Log(character.name + "'s initiative is " + character.Initiative);
        }

        currentRound = 1;

        //initiativeTracker.
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

    private void addCharacterToInitiativeTracker(Character newCharacter)
    {          
        foreach (Character character in initiativeTracker)
        {
            //В случае превосходящей инициативы персонаж добавляется выше текущего
            if(newCharacter.Initiative > character.Initiative)
            {
                initiativeTracker.AddBefore(initiativeTracker.Find(character), newCharacter);
                return;
            }
            //В случае равенства инициатив, персонаж добавляется ниже текущего
            else if (newCharacter.Initiative > character.Initiative)
            {
                initiativeTracker.AddAfter(initiativeTracker.Find(character), newCharacter);
                return;
            }
        }

        //Если у всех прочих персонажей инициатива выше (или новый персонаж первый), новый персонаж добавляется в конец списка
        initiativeTracker.AddLast(newCharacter);
    }
}
