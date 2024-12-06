using System.Collections;
using TMPro;
using UnityEngine;

public class InkDisplayText : MonoBehaviour
{
    public TMP_Text displayText; // TextMeshPro component
    public float typingSpeed = 0.03f; // Speed of the typing animation
    private Coroutine currentCoroutine; // Tracks the current running animation

    /// <summary>
    /// Animates the text letter by letter. Skips animation if already running.
    /// </summary>
    public void AnimateText(string text)
    {
        // Stop the current animation if one is running
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        // Replace <br><br> with newline character
        text = text.Replace("<br>", "\n");

        // Start a new animation
        currentCoroutine = StartCoroutine(TypeText(text));
    }

    /// <summary>
    /// Stops the animation and immediately displays the full text.
    /// </summary>
    public void StopAllAnimations()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
        // Display the full text immediately
        displayText.text = displayText.text.Replace("<br>", "\n");//reverifica asta!
    }

    /// <summary>
    /// Coroutine to type out text letter by letter.
    /// </summary>
    private IEnumerator TypeText(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            Debug.LogWarning("Attempting to animate an empty or null string.");
            yield break;
        }

        displayText.text = ""; // Clear the text box

        foreach (char letter in text.ToCharArray())
        {
            displayText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        // Animation finished
        currentCoroutine = null;
    }
}
