using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelManager : MonoBehaviour
{
    // Maak een lijst met alle knoppen
    public Button[] buttons;

    
    void Update()
    {
        // Loop door alle levels heen.
        for (int i = 0; i < buttons.Length; i++)
        {
            // Kijk wat de players laatste level was en zet het volgende level aal
            if (i < PlayerPrefs.GetInt("lastLevel", 1))
            {
                buttons[i].interactable = true;
            }
            else
            {
                buttons[i].interactable = false;
                

            }
        }
    }
}
