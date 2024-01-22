using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    public Transform Initial { get; set; }
    public Button Play { get; set; }

    public TextMeshProUGUI Countdown { get; set; }
    public TextMeshProUGUI Round { get; set; }

    public void LoadComponents()
    {
        Initial = transform.Find("Initial");
        Play = UIHelper.GetComponent<Button>(transform, "Initial/Button");
        Countdown = UIHelper.GetComponent<TextMeshProUGUI>(transform, "Countdown/Text");
        Round = UIHelper.GetComponent<TextMeshProUGUI>(transform, "Round/Text");

        EventBindings();
    }

    private void EventBindings()
    {
    }

    public void HideInitial()
    {
        Initial.transform.gameObject.SetActive(false);
    }

    public void SetCountdown(string text)
    {
        ShowOrHideCountdown(true);
        Countdown.text = text;
    }

    public void SetRound(int value)
    {
        Round.text = $"<b>Round<b>:\n{value}";
    }

    public void SetStartCountdown()
    {
        ShowOrHideCountdown(true);
        Countdown.text = "START";
        StartCoroutine(StartCountdownCoroutine());
    }

    public void ShowOrHideCountdown(bool enable)
    {
        Countdown.gameObject.SetActive(enable);
    }

    private IEnumerator StartCountdownCoroutine()
    {
        yield return new WaitForSeconds(0.35f);
        ShowOrHideCountdown(false);
    }
}