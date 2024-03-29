using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance; 

    [SerializeField] GameObject Name;  
    [SerializeField] GameObject WinPanel;
    [SerializeField] GameObject LosePanel;
    [SerializeField] GameObject TimeOverPanel;
    [SerializeField] GameObject InvalidPanel;
    [SerializeField] GameObject RestartButton;
    [SerializeField] GameObject TimerPanel;
    [SerializeField] GameObject RoundPanel;
    [SerializeField] GameObject WordPanel;
    [SerializeField] GameObject SwitchModePanel;
    [SerializeField] GameObject LoginButton;
    [SerializeField] GameObject LogoutButton;
    [SerializeField] GameObject LeaderboardButton;
    [SerializeField] GameObject MusicOn;
    [SerializeField] GameObject MusicOff;
    [SerializeField] GameObject VibrateOn;
    [SerializeField] GameObject VibrateOff;

    [SerializeField] GameObject Keyboard;

    
    [SerializeField] AudioSource Music;

    Text _timerText;
    Text _roundsText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Multiple instances of a singleton in the scene");
            Destroy(this);
        }

        _timerText = TimerPanel.GetComponentInChildren<Text>();
        _roundsText = RoundPanel.GetComponentInChildren<Text>();
    }

    public void WinRound(string word)
    {
        WordPanel.SetActive(true);
        string text = "The word is\n" + word;
        WordPanel.GetComponentInChildren<Text>().text = text;
    }

    public void Lose(string word)
    {
        LosePanel.SetActive(true);
        RestartButton.SetActive(true);
        WordPanel.SetActive(true);
        Keyboard.SetActive(false);

        string text = "The word is\n" + word;
        WordPanel.GetComponentInChildren<Text>().text = text;
    }

    public void Win()
    {
        WinPanel.SetActive(true);
        RestartButton.SetActive(true);
        Keyboard.SetActive(false);
    }

    public void TimeRoundOver(string word)
    {
        WordPanel.SetActive(true);
        TimeOverPanel.SetActive(true);
        RestartButton.SetActive(true);
        Keyboard.SetActive(false);

        string text = "The word is\n" + word;
        WordPanel.GetComponentInChildren<Text>().text = text;
    }

    public void Invalid()
    {
        StartCoroutine(ShowPanel(InvalidPanel));
    }

    IEnumerator ShowPanel(GameObject panel)
    {
        float showTime = 1f;
        panel.SetActive(true);
        yield return new WaitForSeconds(showTime);
        panel.SetActive(false); 
    }

    public void DisplayTimer(float time)
    {
        _timerText.text = time.ToString("F1");
    }

    public void DisplayRounds(int rounds)
    {
        _roundsText.text = rounds.ToString();
    }

    public void ToggleRound(bool state)
    {
        RoundPanel.SetActive(state);
        TimerPanel.SetActive(state);
    }

    public void RefreshLogInState(bool state)
    {
        // LoginButton.SetActive(!state);

        LeaderboardButton.SetActive(state);

        // LogoutButton.SetActive(state);
    }

    public void SetName(string _name)
    {
        Name.GetComponent<TextMeshProUGUI>().text = _name;
    }

    public void ShowSwitchModePanel()
    {
        SwitchModePanel.SetActive(true);
        Transform mode = SwitchModePanel.transform.Find("Mode");
        mode.GetComponent<Text>().text = Enum.GetName(typeof(levelModes), GameManager.Instance.Mode);
    }

    public void SelectButton(GameObject obj)
    {
        obj.GetComponent<Image>().color = Color.grey;
    }
    public void UnselectButton(GameObject obj)
    {
        obj.GetComponent<Image>().color = Color.white;
    }
    
    public void LoadSettings(bool music, bool vibration)
    {
        MusicOn.SetActive(music);

        MusicOff.SetActive(!music);
        Music.mute = !music;

        VibrateOn.SetActive(vibration);

        VibrateOff.SetActive(!vibration);
    }
}
