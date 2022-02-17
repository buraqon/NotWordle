using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager 
{
    private int _wordLength;

    private string _testWord;
    private string _word = "";
    private Key[] _keysUsed;
    private int _currentIndex = 0;
    
    private bool _isStarted = true;

    public GridSystem _grid;

    public bool IsStarted
    {
        get {return _isStarted;}
    }
    public LevelManager(int _wordLength, GridSystem _grid)
    {
        this._wordLength = _wordLength;
        this._grid = _grid;
    }

    public void StartLevel()
    {
        _testWord = GameManager.Instance.wordDictionary.GetRandomWord(_wordLength);
        Debug.Log(_testWord);
        _keysUsed = new Key[_testWord.Length];

        _isStarted = true;  
        _currentIndex = 0;
        _word = "";
    }

    public void AddLetter(Key key)
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

    public void SubmittingGuess()
    {
        if (!GameManager.Instance.wordDictionary.CheckWord(_word, _testWord.Length))
        {   
            UIManager.Instance.Invalid();
            return;
        }
            
        string tempWord = CheckRight();

        if(tempWord == "-----")
        {
            WinLevel();
            return;
        }
            
        CheckWrong(tempWord);

        if(_currentIndex >= _grid.GridSize.y -1)
        {
            LoseLevel();
            return;
        }
            
        _word = "";
        _currentIndex++;
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

    public void RestartLevel()
    {
        for(int i=0; i<_grid.GridSize.x; i++)
        {
            for (int j=0; j<_grid.GridSize.y; j++)
            {
                _grid.SetState(i,j, State.Regular);
                _grid.SetValue(i, j, "");
            }
        }
        
        foreach(var key in GameManager.Instance.Keyboard.GetComponentsInChildren<Key>())
        {
            key.ResetKey();
        }

        StartLevel();
    }

    private void SubmittedState(int i , State state)
    {
        _grid.SetState(i, _currentIndex, state);
        _keysUsed[i].SwtichState(state);
    }

    public void WinLevel()
    {
        UIManager.Instance.WinRound(_testWord);
        GameManager.Instance.WinRound();
    }

    public void LoseLevel()
    {
        GameManager.Instance.LoseGame(_testWord);
    }

    public void PauseLevel()
    {
        _isStarted = false;
    }
}
