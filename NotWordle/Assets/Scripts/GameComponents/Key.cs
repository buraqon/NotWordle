using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Key : Cell
{
    Button button;
    void  Start()
    {
        state = State.Regular;
        image = GetComponent<Image>();
        cellText = GetComponentInChildren<Text>();
        cellValue = cellText.text;
        
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonPressed);
    }

    void ButtonPressed()
    {
        GameManager.Instance.GetKey(this);
    }

    public override void SwtichState(State _state)
    {
        if (state < _state)
            state = _state;

        ChangeColor();
    }

    public void ResetKey()
    {
        state = State.Regular;
        image.color = Color.white;
    }
}