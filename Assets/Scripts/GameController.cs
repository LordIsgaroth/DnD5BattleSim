using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    private List<Character> allCharacters;
    private LinkedList<Character> initiativeTracker;
    private int currentRound;
    private bool gamePaused = false;
    [SerializeField] private DiceSet d20;

    private StringEvent logEvent;    

    //Переменные для элементов интерфейса
    //Модальное окно окончания игры
    private ModalPanel modalPanel;
    private UnityAction quitAction;    

    public bool GamePaused { get { return gamePaused; } }

    public DiceSet D20 { get { return d20; } }

    void Awake()
    {
        Library.initializeGameData();

        modalPanel = ModalPanel.Instance();
        quitAction = new UnityAction(QuitAction);
        logEvent = new StringEvent();
    }

    void Start()
    {
        allCharacters = new List<Character>();
        initiativeTracker = new LinkedList<Character>();

        GameObject[] allCharacterObjects = GameObject.FindGameObjectsWithTag("Character");

        foreach(GameObject characterObject in allCharacterObjects)
        {
            Character character = characterObject.GetComponent<Character>();

            allCharacters.Add(character);
            addCharacterToInitiativeTracker(character);            
        }

        LogWindow logWindow = GameObject.Find("LogWindow").GetComponent<LogWindow>();

        logEvent.AddListener(logWindow.DisplayIntoLogWindow);

        //Для тестирования выведем на экран трекер инициативы
        foreach (Character character in initiativeTracker)
        {
            logEvent.Invoke(character.name + "'s initiative is " + character.Initiative);
        }

        currentRound = 1;

        logEvent.Invoke("Round " + currentRound);
    }

    public void CheckVictoriousTeam()
    {
        int previousTeam = 1;
        int victoriousTeam = 0;
        string result;

        foreach (Character character in allCharacters)
        {
            if (character.Conscious)
            {
                if(previousTeam != character.Team)
                {
                    return;
                }

                previousTeam = character.Team;
                victoriousTeam = previousTeam;
            }
        }

        if(victoriousTeam != 0)
        {
            result = "Team " + victoriousTeam + " wins";
        }
        else
        {
            result = "It's a draw!";
        }

        EndGame("Battle is over! " + result);
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
            return initiativeTracker.First.Value;
        }

        LinkedListNode<Character> currentNode = initiativeTracker.Find(currentCharacter);

        //Если есть следующий в списке - вернуть его
        if(currentNode.Next != null)
        {
            nextCharacter = currentNode.Next.Value;
        }
        //Если нет - начинается следующий раунд. Вернуть первого в списке инициативы
        else
        {
            currentRound++;
            NewRound();
            logEvent.Invoke("Round " + currentRound);
            logEvent.Invoke(initiativeTracker.First.Value.ToString());
            nextCharacter = initiativeTracker.First.Value;
        }

        //Если следующий персонаж без сознания - вернуть следующего за ним
        if (!nextCharacter.Conscious)
        {
            nextCharacter = GetNextCharacter(nextCharacter);
        }

        return nextCharacter;
    }

    private void EndGame(string endGameText)
    {
        gamePaused = true;
        modalPanel.Choise(endGameText, quitAction);
    }

    void QuitAction()
    {
        gamePaused = false;
    }
}
