using UnityEngine;
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

    // user details
    private string _name;
    private string _id;

    const int LENGTH = 5;

    private LevelManager _levelManager;

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

        InitiateCells();
    }

    void Update()
    {
        if(_levelManager.IsStarted)
        {
            _levelManager.UpdateLevel();
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

    public void RestartGame()
    {
        _levelManager.RestartLevel();
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
                PlayfabManager.Instance.Login(_id);
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
}
