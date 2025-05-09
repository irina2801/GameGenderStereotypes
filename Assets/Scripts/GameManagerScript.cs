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
    private Stack<string> canvasHistory = new Stack<string>();//for go back button


    void Start()
    {
        // Load the Ink story from JSON file
        story = new Story(jsonAsset.text);

        // Add canvases tagged as "Canvas" in the Dictionary
        canvasDictionary = new Dictionary<string, GameObject>();//Initialize Dictionary
        // Populate the dictionary with canvases from all relevant tags
        string[] canvasTags = { "Canvas", "ReflectionCanvas", "ReportCanvas" };
        foreach (string tag in canvasTags)
        {
            foreach (GameObject canvas in GameObject.FindGameObjectsWithTag(tag))
            {
                if (!canvasDictionary.ContainsKey(canvas.name))
                {
                    canvasDictionary[canvas.name] = canvas;
                    canvas.SetActive(false); // Deactivate all canvases initially
                    Debug.Log($"Added canvas: {canvas.name} with tag: {tag}");
                }
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
    /*public void OnButtonPressed()//CONTINUE Button
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
    }*/
    public void OnButtonPressed()
    {
        // Validate the current canvas state
        if (!string.IsNullOrEmpty(currentActiveCanvas) && canvasDictionary.ContainsKey(currentActiveCanvas))
        {
            GameObject canvas = canvasDictionary[currentActiveCanvas];
            if (canvas.CompareTag("ReflectionCanvas"))
            {
                if (!AreAllInputsFilled(canvas))
                {
                    DisplayErrorMessage("Rispondete a tutte le domande prima di procedere :)");
                    return;
                }

                ClearErrorMessage();
                CollectResponses();
            }

            // Handle text animation
            InkDisplayText[] displayTextComponents = canvas.GetComponentsInChildren<InkDisplayText>();
            foreach (var displayText in displayTextComponents)
            {
                if (displayText.IsAnimating())
                {
                    // If the text is animating, instantly display the full text
                    string fullText = FetchTextForCanvas(currentActiveCanvas);
                    displayText.SetFullTextImmediately(fullText);
                    return; // Do not advance the story yet
                }
            }
        }

        // Ensure the story state is synchronized
        if (story.canContinue)
        {
            string storyText = story.Continue(); // Advance the story
            Debug.Log($"Current text: {storyText}");
            ContinueStory();
        }
        else if (story.currentChoices.Count > 0)
        {
            Debug.Log("Choices are available but no further content to continue.");
        }
        else
        {
            // Attempt to resynchronize the story state
            if (!string.IsNullOrEmpty(currentActiveCanvas))
            {
                Debug.LogWarning("No more content to continue! Attempting to resynchronize with Ink story.");
                try
                {
                    story.ChoosePathString(currentActiveCanvas);
                    if (story.canContinue)
                    {
                        string resyncedText = story.Continue();
                        Debug.Log($"Resynchronized text: {resyncedText}");
                        ContinueStory();
                    }
                    else
                    {
                        Debug.LogError("Resynchronization failed. Ending game.");
                        SaveReport();
                    }
                }
                catch
                {
                    Debug.LogError("Failed to resynchronize story state.");
                    SaveReport();
                }
            }
            else
            {
                Debug.LogWarning("No active canvas or Ink path to resynchronize.");
                SaveReport();
            }
        }
    }





    //method driven by Ink file logic
    public void OnChoiceSelected(int choiceIndex) // Connected to Choice Buttons from Unity
    {
        GameObject canvas = canvasDictionary[currentActiveCanvas];
        if (choiceIndex >= 0 && choiceIndex < story.currentChoices.Count)
        {
            // Get the selected choice text
            string choiceText = story.currentChoices[choiceIndex].text;//to save choices 
            // Save the choice to the playerResponses list
            playerResponses.Add($"Choice: {choiceText} from canvas: {currentActiveCanvas}");//to save choices 


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
        // Push the current active canvas to the history before transitioning//go back button
        if (!string.IsNullOrEmpty(currentActiveCanvas))
        {
            canvasHistory.Push(currentActiveCanvas);
        }

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
    //only for questions
    private void CollectResponses()//using Queue so that answers are saved according to the hierarchy they have in canvas (top to bottom) => that's why not get component
    {
        // Ensure the current active canvas exists in the dictionary
        if (!string.IsNullOrEmpty(currentActiveCanvas) && canvasDictionary.ContainsKey(currentActiveCanvas))
        {
            GameObject canvas = canvasDictionary[currentActiveCanvas];

            // Check if the current canvas is a ReflectionCanvas
            if (canvas.CompareTag("ReflectionCanvas"))
            {

                // Save the name of the ReflectionCanvas in the report
                playerResponses.Add($"ReflectionCanvas_Name: {currentActiveCanvas}");

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
                        playerResponses.Add($"Open_Question: {inputField.name}\nAnswer: {inputField.text}");
                        continue; // Move to the next child
                    }

                    // Check if the current object is a ToggleGroup (Closed Question)
                    ToggleGroup toggleGroup = current.GetComponent<ToggleGroup>();
                    if (toggleGroup != null)
                    {
                        Toggle selectedToggle = toggleGroup.ActiveToggles().FirstOrDefault();
                        if (selectedToggle != null)
                        {
                            playerResponses.Add($"Closed_Question: {toggleGroup.name}\nAnswer: {selectedToggle.name}");
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
            // Locate the Scroll View's TextMeshPro text box
            TMP_Text scrollViewTextBox = reportCanvas.GetComponentInChildren<TMP_Text>();

            if (scrollViewTextBox != null)
            {
                // Set the text dynamically
                scrollViewTextBox.text = string.Join("\n\n", playerResponses);
                Debug.Log($"Report displayed in the Scroll View: {string.Join("\n\n", playerResponses)}");
            }
            else
            {
                Debug.LogWarning("Scroll View TextMeshPro text component not found.");
            }
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


    public void OnGoBackButtonPressed()
    {
        if (!string.IsNullOrEmpty(currentActiveCanvas) && canvasDictionary.ContainsKey(currentActiveCanvas))
        {
            GameObject currentCanvas = canvasDictionary[currentActiveCanvas];
            InkDisplayText[] displayTextComponents = currentCanvas.GetComponentsInChildren<InkDisplayText>();

            foreach (var displayText in displayTextComponents)
            {
                if (displayText.IsAnimating())
                {
                    // Instantly display full text if animating
                    string fullText = FetchTextForCanvas(currentActiveCanvas);
                    displayText.SetFullTextImmediately(fullText);
                    return; // Exit without proceeding to canvas transition
                }
            }
        }

        // Proceed with Go Back logic if no text animation is active
        if (canvasHistory.Count > 0)
        {
            string previousCanvas = canvasHistory.Pop();

            // Update Ink story to match previous canvas state
            try
            {
                story.ChoosePathString(previousCanvas); // Rewind Ink state
                Debug.Log($"Jumped back to Ink path: {previousCanvas}");
            }
            catch
            {
                Debug.LogError($"Failed to jump to Ink path for canvas: {previousCanvas}");
                return;
            }

            if (story.canContinue)
            {
                story.Continue(); // Reset canContinue state
            }

            // Activate the previous canvas
            ActivateCanvas(previousCanvas);
        }
        else
        {
            Debug.LogWarning("No previous canvas in history to go back to.");
        }
    }





}
