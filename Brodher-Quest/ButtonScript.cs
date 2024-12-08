using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//Voeg een event trigger toe
[RequireComponent(typeof(EventTrigger))]
public class ButtonScript : MonoBehaviour
{
    [SerializeField]
    private Transform buttonText;
    private Button button;

    //Pak de button component en de TMPro
    void Awake()
    {
        button = GetComponent<Button>();
        buttonText = transform.GetChild(0);
    }
    
    //Gebruik deze code in een event trigger als de button interactable is
    public void TextOffset(float offset)
    {
        if (!button.interactable) return;

        buttonText.transform.position = new Vector3(buttonText.position.x, buttonText.position.y + offset, buttonText.position.z);
    }

}
