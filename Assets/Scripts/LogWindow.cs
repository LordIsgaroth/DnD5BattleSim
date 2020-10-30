using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogWindow : MonoBehaviour
{
    [SerializeField] private Text logText;

    public void DisplayIntoLogWindow(string message)
    {

        logText.text += message + '\n';
    }
}
