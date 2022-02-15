using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;   
    WordGameDict wordDictionary;

    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private Vector2 _gridPos;
    [SerializeField] private float _cellSize;
    [SerializeField] private GameObject _cellInstance;
    [SerializeField] private GameObject _cellHolder;
    [SerializeField] private GameObject _keyboard;
    
    private GridSystem _grid;
    private string _testWord;
    private string _word = "";
    private Key[] _keysUsed;
    private int _currentIndex = 0;
    private float _timer = 0;
    private bool _isStarted = true;

    const int LENGTH = 5;

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
        _grid = new GridSystem(_gridSize, _gridPos, _cellSize);
        _testWord = wordDictionary.GetRandomWord(LENGTH);
        _keysUsed = new Key[_testWord.Length];
        InitiateCells();
    }

    void Update()
    {
        if(_isStarted)
        {
            _timer += Time.deltaTime;
            UIManager.Instance.DisplayTimer(_timer);
        }  
    }

    public void GetKey(Key key) 
    {
        KeyCode vKey = (KeyCode) System.Enum.Parse(typeof(KeyCode), key.CellValue);
        if(CheckLetter(vKey))
        {
            AddLetter(key);
        }
    }

    bool CheckLetter(KeyCode key)
    {
        if(key >= KeyCode.A && key <= KeyCode.Z)
            return true;

        return false;   
    }
    void AddLetter(Key key)
    {
        if(_word.Length < _testWord.Length)
        {
            _word += key.CellValue;
            _keysUsed[_word.Length-1] = key;
            _grid.SetValue(_word.Length-1, _currentIndex, key.CellValue.ToString());
        }
    }
    public void RemoveLetter()
    {
        if(_word.Length > 0)
        {
            _grid.SetValue(_word.Length-1, _currentIndex, "");
            _keysUsed[_word.Length-1] = null;
            _word = _word.Remove(_word.Length-1);
        }
    }
    public void SubmitGuess()
    {
        if (!wordDictionary.CheckWord(_word, _testWord.Length))
        {   
            UIManager.Instance.Invalid();
            return;
        }
            
        string tempWord = CheckRight();

        if(tempWord == "-----")
        {
            WinGame();
            return;
        }
            
        CheckWrong(tempWord);

        if(_currentIndex >= _gridSize.y -1)
        {
            LoseGame();
            return;
        }
            
        _word = "";
        _currentIndex++;
    }

    private void SubmittedState(int i , State state)
    {
        _grid.SetState(i, _currentIndex, state);
        _keysUsed[i].SwtichState(state);
    }

    private void InitiateCells()
    {
        for(int i=0; i<_gridSize.x; i++)
        {
            for (int j=0; j<_gridSize.y; j++)
            {
                GameObject clone = Instantiate(_cellInstance, _cellHolder.transform);
                clone.transform.position = _grid.GetWorldPosition(i, j);
                _grid.SetCell(i, j, clone.GetComponent<Cell>());
            }
        }
    }

    private string CheckRight()
    {
        string tempWord = "";
        for (int i = 0; i < _testWord.Length; i++)
        {
            if (_word[i] == _testWord[i])
            {
                SubmittedState(i, State.Correct);
                tempWord += "-";
            }
            else
            {
                tempWord += _testWord[i];
            }
        }
        return tempWord;
    }

    private void CheckWrong(string tempWord)
    {
        for (int i = 0; i < tempWord.Length; i++)
        {
            if (tempWord[i] != '-')
            {
                if (tempWord.Contains(_word[i].ToString()))
                    SubmittedState(i, State.FalsePlaced);
                else
                    SubmittedState(i, State.False);
            }
        }
    }

    public void RestartGame()
    {
        for(int i=0; i<_gridSize.x; i++)
        {
            for (int j=0; j<_gridSize.y; j++)
            {
                _grid.SetState(i,j, State.Regular);
                _grid.SetValue(i, j, "");
            }
        }
        
        foreach(var key in _keyboard.GetComponentsInChildren<Key>())
        {
            key.ResetKey();
        }
        _testWord = wordDictionary.GetRandomWord(LENGTH);
        _timer = 0;
        _isStarted = true;  
        _currentIndex = 0;
        _word = "";
    }

    public void WinGame()
    {
        _isStarted = false;
        UIManager.Instance.Win(_testWord);
    }

    public void LoseGame()
    {
        _isStarted = false;
        UIManager.Instance.Lose(_testWord);
    }
}
