using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    protected State state;
    protected string cellValue;
    protected Image image;
    protected Text cellText;

    public string CellValue
    {
        get{ return cellValue;}
        set
        {
            cellValue = value;
            cellText.text = cellValue;
        }
    }

    void Start()
    {
        image = GetComponent<Image>();
        cellText = GetComponentInChildren<Text>();
        cellValue = cellText.text;
        SwtichState(State.Regular);
    }

    public virtual void SwtichState(State _state)
    {
        state = _state;
        ChangeColor();
    }

    protected void ChangeColor()
    {
        switch (state)
        {
            case State.Regular:
                image.color = Color.gray;
                break;
            case State.False:
                image.color = Color.red;
                break;
            case State.FalsePlaced:
                image.color = Color.yellow;
                break;
            case State.Correct:
                image.color = Color.green;
                break;
        }
    }
}

public enum State
{
    Regular,
    False,
    FalsePlaced,
    Correct
}
