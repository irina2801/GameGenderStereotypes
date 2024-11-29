using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceCanvas : MonoBehaviour
{
    [Header("Button to Ink Mapping")]//for visualisation in Unity in Inspector
    public string firstChoice; // Ink key for YES button (e.g., "AChoice")
    public string secondChoice;  // Ink key for NO button (e.g., "BChoice")

   /* // Public method to notify GameManager when a choice is made
    public void OnChoiceMade(string choiceOption)
    {
        FindObjectOfType<GameManagerScript>().HandleChoice(choiceOption);
    }*/
    
}