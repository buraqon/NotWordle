using UnityEngine;
using UnityEngine.UI;
using System;
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
                image.color = HexToColor("D93A3AFF");
                break;
            case State.FalsePlaced:
                image.color = HexToColor("DDBB12FF");
                break;
            case State.Correct:
                image.color = HexToColor("15B410FF");
                break;
        }
    }

    public Color HexToColor(string hexNumber)
    {
        Color color;
        if ( ColorUtility.TryParseHtmlString("#" + hexNumber, out color))
        { 
            return color;
        }
        return new Color();
    }
}

public enum State
{
    Regular,
    False,
    FalsePlaced,
    Correct
}
