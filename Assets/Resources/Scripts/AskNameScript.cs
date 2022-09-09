using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class AskNameScript : MonoBehaviour
{
    public TMPro.TextMeshProUGUI[] letters;
    public EventTrigger[] triggersLetter;
    private IAskNameListener callback;
    private ControlsGame _game;

    // Start is called before the first frame update
    void Start()
    {
        _game = new ControlsGame();
        for (int i = 0; i < triggersLetter.Length; i++)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Move;
            int num = i;
            entry.callback.AddListener((data) => LetterMove(num, (AxisEventData)data));
            triggersLetter[i].triggers.Add(entry);
        }
    }

    private void Update()
    {
    }

    public void LetterMove(int num, AxisEventData data)
    {

        if (data.moveDir == MoveDirection.Up)
        {
            LetterUp(num);
        }

        if (data.moveDir == MoveDirection.Down)
        {
            LetterDown(num);
        }
    }

    public void LetterUp(int num)
    {
        char actual = letters[num].text[0];
        char res = actual;
        if (actual == 'A')
        {
            res = '9';
        } else if(actual == '0')
        {
            res = 'Z';
        }
        else
        {
            res--;
        }
        letters[num].text = "" + res;
    }

    public void LetterDown(int num)
    {
        char actual = letters[num].text[0];
        char res = actual;
        if (actual == 'Z')
        {
            res = '0';
        }
        else if (actual == '9')
        {
            res = 'A';
        }
        else
        {
            res++;
        }
        letters[num].text = "" + res;
    }

    public void Valider()
    {
        string name = letters[0].text[0] + "" + letters[1].text[0] + "" + letters[2].text[0];
        callback.askNameValidate(this,name);
    }

    public void Cancel()
    {
        callback.askNameCancel(this);
    }

    public void SetListener(IAskNameListener listener)
    {
        this.callback = listener;
    }


}
