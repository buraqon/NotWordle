using UnityEngine;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;   
    public WordGameDict wordDictionary;

    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private Vector2 _gridPos;
    [SerializeField] private float _cellSize;
    [SerializeField] private GameObject _cellInstance;
    [SerializeField] private GameObject _cellHolder;
    [SerializeField] private GameObject _keyboard;
    [SerializeField] float rushTimeLeft;

    // user details
    public string _name;
    private string _id;

    const int LENGTH = 5;

    private LevelManager _levelManager;
    private int _mode = 0;
    private float _timer = 0;
    private float _startedTime;
    private float _nowTime;
    private int _round = 0;
    private bool vibrateOn = true;
    private bool musicOn = true;

    public int Mode 
    {
        get {return _mode; }
        private set
        { 
            _mode = value ;
        }
    }

    public bool VibrateOn
    {
        get{return vibrateOn;}
    }
    public bool MusicOn
    {
        get{return musicOn;}
    }

    public float Timer 
    {
        get {return _timer; }
        private set
        { 
            _timer = value ;
            UIManager.Instance.DisplayTimer(_timer);
        }
    }

    public int Round 
    {
        get {return _round; }
        private set
        { 
            _round = value ;
            UIManager.Instance.DisplayRounds(_round);
        }
    }

    public GameObject Keyboard
    {
        get {return _keyboard;}
    }

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
    }

    void Start()
    {
        wordDictionary = new WordGameDict();
        GridSystem _grid = new GridSystem(_gridSize, _gridPos, _cellSize);

        _levelManager = new LevelManager(LENGTH, _grid);
        _levelManager.StartLevel();

        PlayfabManager.Instance.Login(_id);
        _startedTime = DateTime.Now.Second + DateTime.Now.Minute * 60 + DateTime.Now.Hour * 360 + DateTime.Now.Millisecond * 0.001f;

        InitiateCells();
        LoadPrefs();
    }

    void Update()
    {
        if(_levelManager.IsStarted)
        {
            UpdateTimer();
        }  

        if(Mode == 2)
        {
            if (Timer >= rushTimeLeft)
            {
                FinishTimeGame();
            }
        }
    }

    public void GetKey(Key key) 
    {
        KeyCode vKey = (KeyCode) System.Enum.Parse(typeof(KeyCode), key.CellValue);
        if(CheckLetter(vKey))
        {
            _levelManager.AddLetter(key);
        }
    }

    bool CheckLetter(KeyCode key)
    {
        if(key >= KeyCode.A && key <= KeyCode.Z)
            return true;

        return false;   
    }
    
    private void InitiateCells()
    {
        for(int i=0; i<_gridSize.x; i++)
        {
            for (int j=0; j<_gridSize.y; j++)
            {
                GameObject clone = Instantiate(_cellInstance, _cellHolder.transform);
                clone.transform.position = _levelManager._grid.GetWorldPosition(i, j);
                _levelManager._grid.SetCell(i, j, clone.GetComponent<Cell>());
            }
        }
    } 

    public void SubmitGuess()
    {
        _levelManager.SubmittingGuess();
    }

    public void RemoveLetter()
    {
        _levelManager.RemoveLetter();
    }

    public void RestartRound()
    {
        _levelManager.RestartLevel();
    }

    public void RestartGame()
    {
        Timer = 0;
        _startedTime = _nowTime;
        Round = 0;
        RestartRound();
    }

    public void SetUserInfo(string fieldName, string field)
    {
        switch (fieldName)
        {
            case  "Name":
                _name = field;
                break;

            case "ID":
                _id = field;
                break;
        } 
    }

    public void ResetUserInfo()
    {
        _name = "";
        _id = "";
    }

    public string GetUserInfo(string fieldName)
    {
        switch (fieldName)
        {
            case  "Name":
                return _name;

            case "ID":
                return _id;
        } 
        Debug.Log("ERROR WRONG FIELD INSERTED FOR USER INFO IN THE GAME MANAGER!!");
        return null;
    }

    public void UpdateTimer()
    {
        _nowTime = DateTime.Now.Second + DateTime.Now.Minute * 60 + DateTime.Now.Hour * 360 + DateTime.Now.Millisecond * 0.001f; 
        Timer = _nowTime - _startedTime;
    }

    public void WinRound()
    {
        WinModeCheck();
    }
    
    public void SwitchMode(int mode)
    {
        if(mode >= 0)
        {
            _mode = mode;
            Timer = 0;
            Round = 0;
            RestartGame();
            _levelManager.PauseLevel();
            UIManager.Instance.ShowSwitchModePanel();
            UIManager.Instance.ToggleRound(mode != 0);
        }
        else
            return;
    }

    void WinModeCheck()
    {
        switch(_mode)
        {
            case 0:
                WinGame();
                break;
            
            case 1:
                Round ++;
                if(Round >= 5)
                {
                    if(_name.Length > 0)
                        PlayfabManager.Instance.SendLeaderboardFive((int)Timer);

                    WinGame();
                }
                else
                {
                    RestartRound();
                }
                break;
            
            case 2:
                Round ++;
                RestartRound();
                break;
        }   
    }

    public void WinGame()
    {
        _levelManager.PauseLevel();
        UIManager.Instance.Win();
        
    }
    public void LoseGame(string word)
    {
        _levelManager.PauseLevel();
        UIManager.Instance.Lose(word);
    }

    public void FinishTimeGame()
    {
        _levelManager.PauseLevel();
        UIManager.Instance.TimeRoundOver();
        PlayfabManager.Instance.SendLeaderboardRush(Round);
    }

    public void ResumeGame()
    {   
        _levelManager.UnPauseLevel();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToggleVibrate(bool state)
    {
        vibrateOn = state;
        SavePrefs();
    }
    public void ToggleMusic(bool state)
    {
        musicOn = state;
        SavePrefs();
    }

    public void SavePrefs()
    {
        PlayerPrefs.SetInt("Vibration", Convert.ToInt32(VibrateOn));
        PlayerPrefs.SetInt("Music", Convert.ToInt32(MusicOn));
        PlayerPrefs.Save();
    }

    public void LoadPrefs()
    {
        vibrateOn = System.Convert.ToBoolean(PlayerPrefs.GetInt("Vibration", 0));
        musicOn = System.Convert.ToBoolean(PlayerPrefs.GetInt("Music", 0));
        UIManager.Instance.LoadSettings(musicOn, vibrateOn);
    }
}



public enum levelModes
{
    Classic,
    Five_Round,
    Rush
}
