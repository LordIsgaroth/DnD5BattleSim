using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ModalPanel : MonoBehaviour
{
    public Text question;
    public Button quitButton;
    public GameObject modalPanelObject;

    public static ModalPanel modalPanel;

    public static ModalPanel Instance()
    {
        if(!modalPanel)
        {
            modalPanel = FindObjectOfType(typeof(ModalPanel)) as ModalPanel;

            if (!modalPanel)
            {
                Debug.LogError("There needs to be one active ModalPanel script on a GameObject in your scene");
            }
        }

        return modalPanel;
    }

    public void Choise (string question, UnityAction quitEvent)
    {
        modalPanelObject.SetActive(true);

        quitButton.onClick.RemoveAllListeners();
        quitButton.onClick.AddListener(quitEvent);
        quitButton.onClick.AddListener(ClosePanel);

        this.question.text = question;
    }

    void ClosePanel()
    {
        modalPanelObject.SetActive(false);
    }
}


