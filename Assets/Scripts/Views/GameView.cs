using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the game's user interface, including buttons, text displays, and countdowns.
/// </summary>
public class GameView : MonoBehaviour
{
    // UI components.
    public Transform Initial { get; set; }
    public Button Play { get; set; }

    // Text elements for displaying game information.
    public TextMeshProUGUI Countdown { get; set; }
    public TextMeshProUGUI Round { get; set; }
    public TextMeshProUGUI Monsters { get; set; }
    public TextMeshProUGUI ElapsedTime { get; set; }

    /// <summary>
    /// Loads UI components and finds the necessary elements in the scene.
    /// </summary>
    public void LoadComponents()
    {
        // Find UI elements within the scene and assign them to the respective properties.
        Initial = transform.Find("Initial");
        Play = UIHelper.GetComponent<Button>(transform, "Initial/Button");
        Countdown = UIHelper.GetComponent<TextMeshProUGUI>(transform, "Countdown/Text");
        Round = UIHelper.GetComponent<TextMeshProUGUI>(transform, "Round/Text");
        Monsters = UIHelper.GetComponent<TextMeshProUGUI>(transform, "Monsters/Text");
        ElapsedTime = UIHelper.GetComponent<TextMeshProUGUI>(transform, "ElapsedTime/Text");
    }

    /// <summary>
    /// Hides the initial UI screen.
    /// </summary>
    public void HideInitial()
    {
        Initial.gameObject.SetActive(false);
    }

    /// <summary>
    /// Sets the countdown text.
    /// </summary>
    /// <param name="text">The text to display in the countdown.</param>
    public void SetCountdown(string text)
    {
        ShowOrHideCountdown(true);
        Countdown.text = text;
    }

    /// <summary>
    /// Sets the current round number in the UI.
    /// </summary>
    /// <param name="value">The round number to display.</param>
    public void SetRound(int value)
    {
        Round.text = $"<b>Round<b>:\n{value}";
    }

    /// <summary>
    /// Sets the current number of monsters and the total number.
    /// </summary>
    /// <param name="value">Current number of monsters.</param>
    /// <param name="total">Total number of monsters.</param>
    public void SetMonsters(int value, int total)
    {
        if (value < 0)
            value = 0;

        Monsters.text = $"<b>Monsters<b>:\n{value}/{total}";
    }

    /// <summary>
    /// Sets the elapsed time text in the UI.
    /// </summary>
    /// <param name="elapsed">The elapsed time to display.</param>
    public void SetElapsedTime(string elapsed)
    {
        ElapsedTime.text = $"<b>Elapsed Time<b>:\n{elapsed}";
    }

    /// <summary>
    /// Initiates the start countdown in the UI.
    /// </summary>
    public void SetStartCountdown()
    {
        ShowOrHideCountdown(true);
        Countdown.text = "START";
        StartCoroutine(StartCountdownCoroutine());
    }

    /// <summary>
    /// Shows or hides the countdown text element.
    /// </summary>
    /// <param name="enable">True to show, false to hide the countdown.</param>
    public void ShowOrHideCountdown(bool enable)
    {
        Countdown.gameObject.SetActive(enable);
    }

    /// <summary>
    /// Coroutine to handle the brief display of the "START" message before hiding the countdown.
    /// </summary>
    private IEnumerator StartCountdownCoroutine()
    {
        yield return new WaitForSeconds(0.35f);
        ShowOrHideCountdown(false);
    }
}
