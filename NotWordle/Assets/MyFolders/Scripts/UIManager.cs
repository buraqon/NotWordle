using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;   
    [SerializeField] GameObject WinPanel;
    [SerializeField] GameObject LosePanel;
    [SerializeField] GameObject InvalidPanel;
    [SerializeField] GameObject RestartButton;
    [SerializeField] GameObject TimerPanel;
    [SerializeField] GameObject WordPanel;

    [SerializeField] GameObject LoginButton;
    [SerializeField] GameObject LogoutButton;

    Text _timerText;

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
    }

    public void Win(string word)
    {
        WinPanel.SetActive(true);
        RestartButton.SetActive(true);
        WordPanel.SetActive(true);

        string text = "The word is\n" + word;
        WordPanel.GetComponentInChildren<Text>().text = text;
    }

    public void Lose(string word)
    {
        LosePanel.SetActive(true);
        RestartButton.SetActive(true);
        WordPanel.SetActive(true);

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

    public void RefreshLogInState(bool state)
    {
        LoginButton.SetActive(!state);

        LogoutButton.SetActive(state);
    }
}
