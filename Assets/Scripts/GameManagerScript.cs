using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Ink.Runtime;
using System.IO;
using System.Linq; // Add this for LINQ methods - related to toggle

public class GameManagerScript : MonoBehaviour
{
    public string inkFileName = "Story"; // Name of the Ink file 
    private Story story; // Ink runtime story
    public Dictionary<string, GameObject> canvasDictionary;

    public TextAsset jsonAsset;

    //to save the player's answers in a list
    private List<string> playerResponses = new List<string>();
    public GameObject reportCanvas; // Reference to the ReportCanvas
    public TMP_Text reportTextBox;  // Reference to the TextMeshPro text box in the ReportCanvas

    public TMP_Text errorMessageText; // Global error message for continue button on ReflectionCanvas 

    private string currentActiveCanvas = string.Empty; // Tracks the currently active canvas


    void Start()
    {
        // Load the Ink story from JSON file
        story = new Story(jsonAsset.text);

        // Add canvases tagged as "Canvas" in the Dictionary
        canvasDictionary = new Dictionary<string, GameObject>();
        foreach (GameObject canvas in GameObject.FindGameObjectsWithTag("Canvas"))
        {
            canvasDictionary[canvas.name] = canvas;
            canvas.SetActive(false); // Deactivate all canvases initially
        }

        // Add canvases tagged as "ReflectionCanvas" in the Dictionary
        foreach (GameObject canvas in GameObject.FindGameObjectsWithTag("ReflectionCanvas"))
        {
            if (!canvasDictionary.ContainsKey(canvas.name)) // Avoid duplicates
            {
                canvasDictionary[canvas.name] = canvas;
                canvas.SetActive(false); // Deactivate all canvases initially
            }
        }

        // Add canvases tagged as "ReportCanvas" in the Dictionary
        foreach (GameObject canvas in GameObject.FindGameObjectsWithTag("ReportCanvas"))
        {
            if (!canvasDictionary.ContainsKey(canvas.name)) // Avoid duplicates
            {
                canvasDictionary[canvas.name] = canvas;
                // No need to disable here since ActivateCanvas handles it
            }
        }


        // Preload the first content to activate the initial canvas - This is a solution because I had to press 2 times on continue button at the beginning, GameManager could not detect the hashtag in ink file 
        if (story.canContinue)
        {
            //string initialText = story.Continue(); // Advance the story to the first piece of content
            //Debug.Log("Initial text: " + initialText);
            story.Continue(); // Advance to initialize story state
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

        //string targetCanvas = null;

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
            //ActivateCanvas("PlayGameCanvas"); // Change "PlayGameCanvas" to your starting canvas name
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
        /*else
        {
            Debug.LogWarning("No choices available!"); //I no longer need this debug
        }*/
    }

    //method driven by Ink file logic
    //This methos is connected to the CONTINUE BUTTON in Inspector Unity
    //It works for normal canvases and also canvases with the tag "ReflectionCanvas" 
    public void OnButtonPressed()//CONTINUE Button
    {
        // check if the current canvas is a ReflectionCanvas
        if (!string.IsNullOrEmpty(currentActiveCanvas) && canvasDictionary.ContainsKey(currentActiveCanvas))
        {
            GameObject canvas = canvasDictionary[currentActiveCanvas];
            if (canvas.CompareTag("ReflectionCanvas"))
            {
                // Validate inputs before proceeding
                if (!AreAllInputsFilled(canvas))
                {
                    DisplayErrorMessage("Rispondete a tutte le domande prima di procedere :)");
                    return;
                }

                // Clear error message upon successful validation
                ClearErrorMessage();
                CollectResponses();
            }
        }

        if (story.canContinue)
        {
            //Advance the story based on Ink file
            string storyText = story.Continue(); //this is for debug
            Debug.Log("Current text: " + storyText); //this is for debug
            ContinueStory();
        }
        else
        {
            // If the story has no more content, save the report
            Debug.LogWarning("No more content to continue in the story!"); //this is for debug
            SaveReport();
        }
    }
    //method driven by Ink file logic
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

    /* 
        1. This method diactivates all canvases at the beginning of the game
        2. Then it activated only 1 canvas at once based on the flow logic from th Ink file
        3. Moreover, it tracks the currect active canvas name. This is useful for saving user's answers in the report file.
    */
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
            currentActiveCanvas = canvasName; // get the current canvas name

            // Check if the current canvas is the ReportCanvas
            if (canvasDictionary[canvasName].CompareTag("ReportCanvas"))
            {
                DisplayReport(); // Display the saved answers
            }
            else
            {
                // Ensure text components are handled correctly
                HandleEmptyTextComponents(canvasDictionary[canvasName]);
            }

        }
        else
        {
            Debug.LogWarning("Canvas not found: " + canvasName);//If this debug appears, it means the names Unity-Ink might not be the same
            currentActiveCanvas = string.Empty; // Reset if canvas not found
        }
    }

    //This method collects the user's answers from the canvases with tag: ReflectionCanvas
    //Then, collected answers are stored in playerResponses list with clear labeling for questions and answers
    private void CollectResponses()
    {
        // Ensure the current active canvas exists in the dictionary
        if (!string.IsNullOrEmpty(currentActiveCanvas) && canvasDictionary.ContainsKey(currentActiveCanvas))
        {
            GameObject canvas = canvasDictionary[currentActiveCanvas];

            // Check if the current canvas is a ReflectionCanvas
            if (canvas.CompareTag("ReflectionCanvas"))
            {
                // Use a queue to traverse all child objects, including nested ones
                Queue<Transform> objectsToCheck = new Queue<Transform>();
                objectsToCheck.Enqueue(canvas.transform);

                while (objectsToCheck.Count > 0)
                {
                    Transform current = objectsToCheck.Dequeue();

                    // Check if the current object is a TMP_InputField (Open-Ended Question)
                    TMP_InputField inputField = current.GetComponent<TMP_InputField>();
                    if (inputField != null && !string.IsNullOrWhiteSpace(inputField.text))
                    {
                        playerResponses.Add($"Question: {inputField.name}\nAnswer: {inputField.text}");
                        continue; // Move to the next object
                    }

                    // Check if the current object is a ToggleGroup (Closed Question)
                    ToggleGroup toggleGroup = current.GetComponent<ToggleGroup>();
                    if (toggleGroup != null)
                    {
                        Toggle selectedToggle = toggleGroup.ActiveToggles().FirstOrDefault();
                        if (selectedToggle != null)
                        {
                            playerResponses.Add($"Question: {toggleGroup.name}\nAnswer: {selectedToggle.name}");
                        }
                    }

                    // Add all children of the current object to the queue
                    foreach (Transform child in current)
                    {
                        objectsToCheck.Enqueue(child);
                    }
                }
            }
            else
            {
                Debug.Log($"CollectResponses called on a non-ReflectionCanvas: {currentActiveCanvas}. No responses collected.");
            }
        }
        else
        {
            Debug.LogWarning($"No active canvas or canvas not found in dictionary: {currentActiveCanvas}");
        }
    }


    //This method saves the user's answers to a report file in Application.persistentDataPath when the game ends, while ensuring compatibility with Android when the game ends
    private void SaveReport()
    {
        Debug.Log("Attempting to save the report...");
        // Save the responses to a file
        if (playerResponses.Count == 0)
        {
            Debug.LogWarning("No responses to save. Report will not be created.");
            return;
        }
        string filePath = Application.persistentDataPath + "/PlayerReport.txt";
        File.WriteAllLines(filePath, playerResponses);
        Debug.Log($"Report saved at: {filePath}");


    }

    //This method displayes the text from the report file in the emty TextMeshPro box on the report canvas in Unity at the end of the game
    private void DisplayReport()
    {
        if (reportCanvas != null && reportTextBox != null)
        {
            Debug.Log($"Displaying report: {string.Join("\n\n", playerResponses)}");
            // Format and display the user responses in the text box
            reportTextBox.text = string.Join("\n\n", playerResponses);
            Debug.Log("Report displayed in the ReportCanvas.");
        }
        else
        {
            Debug.LogWarning("Report canvas or text box is not assigned.");
        }
    }





    private bool AreAllInputsFilled(GameObject canvas)
    {
        // Use a queue to traverse all child objects in the hierarchy 

        //=> to make sure that it also takes in account the nested objects
        Queue<Transform> objectsToCheck = new Queue<Transform>();
        objectsToCheck.Enqueue(canvas.transform);

        while (objectsToCheck.Count > 0)
        {
            Transform current = objectsToCheck.Dequeue();

            // Check if the current object is a TMP_InputField (Open-Ended Question)
            TMP_InputField inputField = current.GetComponent<TMP_InputField>();
            if (inputField != null)
            {
                if (string.IsNullOrWhiteSpace(inputField.text)) // Empty input field
                {
                    Debug.Log($"Input field '{inputField.name}' is empty.");
                    return false;
                }
                continue; // Move to the next child
            }

            // Check if the current object is a ToggleGroup (Closed Question)
            ToggleGroup toggleGroup = current.GetComponent<ToggleGroup>();
            if (toggleGroup != null)
            {
                if (!toggleGroup.AnyTogglesOn()) // No toggle selected
                {
                    Debug.Log($"Toggle group '{toggleGroup.name}' has no selection.");
                    return false;
                }
            }

            // Add all children of the current object to the queue
            foreach (Transform child in current)
            {
                objectsToCheck.Enqueue(child);
            }
        }

        return true; // All inputs are valid
    }


    private void DisplayErrorMessage(string message)
    {
        if (errorMessageText != null)
        {
            errorMessageText.text = message;
            errorMessageText.gameObject.SetActive(true); // Ensure it's visible
            Debug.Log("Error displayed: " + message);
        }
        else
        {
            Debug.LogWarning("Error message text component is not assigned.");
        }
    }

    private void ClearErrorMessage()
    {
        if (errorMessageText != null)
        {
            errorMessageText.text = ""; // Clear the text
            errorMessageText.gameObject.SetActive(false); // Hide the message
        }
    }




    private void HandleEmptyTextComponents(GameObject canvas)
    {
        InkDisplayText[] displayTextComponents = canvas.GetComponentsInChildren<InkDisplayText>();

        foreach (InkDisplayText displayText in displayTextComponents)
        {
            if (displayText.displayText != null && string.IsNullOrWhiteSpace(displayText.displayText.text))
            {
                string canvasText = FetchTextForCanvas(currentActiveCanvas);
                if (!string.IsNullOrEmpty(canvasText))
                {
                    displayText.StopAllAnimations();
                    displayText.AnimateText(canvasText);
                }
            }
        }
    }

    private string FetchTextForCanvas(string canvasName)
    {
        List<string> currentTags = story.currentTags;

        foreach (string tag in currentTags)
        {
            if (tag.StartsWith("canvas:") && tag.Split(':')[1].Trim() == canvasName)
            {
                // Fetch the current text without advancing the story
                return story.currentText;
            }
        }

        Debug.LogWarning($"No matching tag found for canvas: {canvasName}");
        return string.Empty;
    }



}
