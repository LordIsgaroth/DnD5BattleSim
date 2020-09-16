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

            allCharacters.Add(character);
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

        Debug.Log("Round " + currentRound);
    }
        
    private void NewRound()
    {
        foreach(Character character in allCharacters)
        {
            character.RenewParameters();
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

    //Получает персонажа, который должен ходить следующим
    public Character GetNextCharacter(Character currentCharacter)
    {
        Character nextCharacter;

        //Текущего персонажа нет - первый ход в партии. Вернуть первого в списке инициативы
        if (currentCharacter == null)
        {
            Debug.Log(initiativeTracker.First.Value);
            return initiativeTracker.First.Value;
        }

        LinkedListNode<Character> currentNode = initiativeTracker.Find(currentCharacter);

        //Если есть следующий в списке - вернуть его
        if(currentNode.Next != null)
        {
            Debug.Log(currentNode.Next.Value);
            nextCharacter = currentNode.Next.Value;
        }
        //Если нет - начинается следующий раунд. Вернуть первого в списке инициативы
        else
        {
            currentRound++;
            NewRound();
            Debug.Log("Round " + currentRound);
            Debug.Log(initiativeTracker.First.Value);
            nextCharacter = initiativeTracker.First.Value;
        }

        //Если следующий персонаж без сознания - вернуть следующего за ним
        if (!nextCharacter.Conscious)
        {
            Debug.Log(nextCharacter);
            nextCharacter = GetNextCharacter(nextCharacter);
        }

        return nextCharacter;
    }
}
