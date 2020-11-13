using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterWindow : MonoBehaviour
{
    [SerializeField] private Text characterName;
    [SerializeField] private Text hpValue;
    [SerializeField] private Text movementSpeed;
    [SerializeField] private Text attackInfo;

    public void DisplayCharacterData(string name, string hp, string movement, string attack)
    {
        characterName.text = name;
        hpValue.text = hp;
        movementSpeed.text = movement;
        attackInfo.text = attack;
    }
}
