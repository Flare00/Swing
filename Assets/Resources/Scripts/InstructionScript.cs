using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class InstructionScript : MonoBehaviour
{
    private float _verticalPos = 1.0f;
    private float _scrollSpeed = 0.0015f;
    public TransitionScript transitionScript;
    public ScrollRect scrollRect;
    private ControlsGame game;

    public void Start()
    {
        game = new ControlsGame();
        transitionScript.ReverseTransition();
        game.UI.Move.Enable();
    }

    public void Update()
    {
        float move = game.UI.Move.ReadValue<Vector2>().y;
        if(move > 0.05f || move < -0.05f)
        {
            Scroll(move);
        }
    }

    public void BackAction()
    {
        transitionScript.LoadSceneWithTransition("Menu");
    }

    public void Scroll(float speed)
    {
        _verticalPos += _scrollSpeed * speed;
        if (_verticalPos < 0) _verticalPos = 0;
        if (_verticalPos > 1) _verticalPos = 1;

        scrollRect.verticalNormalizedPosition = _verticalPos;
    }

}
