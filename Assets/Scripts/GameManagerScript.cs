using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Ink.Runtime;
using System.IO;

public class GameManagerScript : MonoBehaviour
{
    public string inkFileName = "Story"; // Name of the Ink file 
    private Story story; // Ink runtime story
    public Dictionary<string, GameObject> canvasDictionary;

    public TextAsset jsonAsset;

    void Start()
    {
        // Load the Ink story from JSON file
        // string inkJson = System.IO.File.ReadAllText("Assets/StreamingAssets/" + inkFileName + ".json");
        story = new Story(jsonAsset.text);

        // Populate the dictionary with canvases tagged as "Canvas"
        canvasDictionary = new Dictionary<string, GameObject>();
        foreach (GameObject canvas in GameObject.FindGameObjectsWithTag("Canvas"))
        {
            canvasDictionary[canvas.name] = canvas;
            canvas.SetActive(false); // Deactivate all canvases initially
        }

        // Preload the first content to activate the initial canvas - This is a solution because I had to press 2 times on continue button at the beginning, GameManager could not detect the hashtag in ink file 
        if (story.canContinue)
        {
            string initialText = story.Continue(); // Advance the story to the first piece of content
            Debug.Log("Initial text: " + initialText);
            ContinueStory(); // Process the first set of tags and activate the corresponding canvas
        }
        else
        {
            Debug.LogWarning("No content to continue in the story at initialization.");
        }
    }

    private void ContinueStory()
    {
        // Get the current tags from the Ink story
        List<string> currentTags = story.currentTags;

        // Log the current tags for debugging
        Debug.Log("Current tags: " + string.Join(", ", currentTags));


        // Track whether a canvas was activated
        bool canvasActivated = false;

        foreach (string tag in currentTags)
        {
            if (tag.StartsWith("canvas:"))
            {
                string canvasName = tag.Split(':')[1].Trim();
                ActivateCanvas(canvasName);
                canvasActivated = true;
                break;
            }
        }

        //If no canvas was activated, activate a default one - kind of like a WARNING
        if (canvasActivated == false)
        {
            Debug.LogWarning("No canvas tags found in the current story. Activating a default canvas.");
            ActivateCanvas("PlayGameCanvas"); // Change "PlayGameCanvas" to your starting canvas name
        }




        // Log current choices
        if (story.currentChoices.Count > 0)
        {
            Debug.Log("Choices available:");
            for (int i = 0; i < story.currentChoices.Count; i++)
            {
                Debug.Log($"Choice {i}: {story.currentChoices[i].text}");
            }
        }
        else
        {
            Debug.LogWarning("No choices available!");
        }


    }

    public void OnButtonPressed()//This is connected to the CONTINUE BUTTON
    {

        if (story.canContinue)
        {
            //story.Continue(); // Advance the Ink story //This was before the debug
            string storyText = story.Continue(); // Advance the Ink story
            Debug.Log("Current text: " + storyText);
            ContinueStory();
        }
        else
        {
            Debug.LogWarning("No more content to continue in the story!");
        }
    }

    public void OnChoiceSelected(int choiceIndex) // Connected to Choice Buttons from Unity
    {
        if (choiceIndex >= 0 && choiceIndex < story.currentChoices.Count)
        {
            Debug.Log($"Choice {choiceIndex + 1} selected: {story.currentChoices[choiceIndex].text}");
            story.ChooseChoiceIndex(choiceIndex); // Select the choice in the Ink story

            // Process the story after making a choice
            if (story.canContinue)//making sure that the story advances after making a choice
            {
                string storyText = story.Continue();
                Debug.Log("Story advanced: " + storyText);
            }
            else
            {
                Debug.LogWarning("Story cannot continue after the choice.");
            }

            // Continue processing the story (activating canvases or next choices)
            ContinueStory();
        }
        else
        {
            Debug.LogError($"Invalid choice index: {choiceIndex}. Choices available: {story.currentChoices.Count}");
        }
    }

    private void ActivateCanvas(string canvasName)
    {
        // Deactivate all canvases
        foreach (var canvas in canvasDictionary.Values)
        {
            canvas.SetActive(false);
        }

        // Activate the specified canvas
        if (canvasDictionary.ContainsKey(canvasName))
        {
            canvasDictionary[canvasName].SetActive(true);
        }
        else
        {
            Debug.LogWarning("Canvas not found: " + canvasName);//If this debug appears, it means the names Unity-Ink might not be the same
        }
    }

}
