using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogWindow : MonoBehaviour
{
    [SerializeField] private Text logText;
    [SerializeField] private ScrollRect scroll;

    bool scrollingDown;

    public void DisplayIntoLogWindow(string message)
    {
        logText.text += message + '\n';
        scroll.normalizedPosition = new Vector2 (0,0);
        scrollingDown = true;
    }

    private void Update()
    {
        if (scrollingDown)
        {
            scroll.normalizedPosition = Vector2.Lerp(scroll.normalizedPosition, new Vector2(0,0), 1f);
        }

        if (scroll.normalizedPosition.y == 0)
        {
            scrollingDown = false;
        }
    }
}
