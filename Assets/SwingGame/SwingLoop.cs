using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SwingLoop : MonoBehaviour
{
    public EventSystem mainEventSystem;
    public GameObject PreloadObjects;
    public PauseMenuScript pauseScript;
    public GameOverScript gameoverScript;
    public TransitionScript transitionScript;

    private Game[] _games;
    private SwingInput _inputV2;
    private int _nbPlayer = 1;
    private bool _multiplayer = false;

    private bool _isPause;
    private bool _isGameOver = false;
    private bool _isGameOverAsked = false;
    private bool _waitForRelease = false;
    private bool _calledOnce = false;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Destroy(PreloadObjects);

        _isPause = false;
        _inputV2 = new SwingInput();
        _multiplayer = CrossSceneData.Multijoueur;
        // 0 = 16/9 , 1 = 4/3, 2 = 16/9 2 joueur , 3 = 4/3 2 joueur
        int aspectMode = 0;
        float ratio = (float)Screen.width / (float)Screen.height;
        if (ratio > 1.3 && ratio < 1.35)
        {
            aspectMode = 1;
        }

        if (_multiplayer)
        {
            _nbPlayer = 2;
            aspectMode += 2;
        }
        else
        {
            _nbPlayer = 1;
        }

        _games = new Game[_nbPlayer];
        for (int i = 0; i < _nbPlayer; i++)
        {
            _games[i] = new Game(_multiplayer, aspectMode, i,CrossSceneData.LoadGame);
        }
        transitionScript.ReverseTransition();
    }

    public void OnEnable()
    {
        if (_inputV2 == null)
        {
            _inputV2 = new SwingInput();
        }
    }

    // Update is called once per frame
    void Update()
    {
        int gameOverCounter = 0;
        int gameOverAskedCounter = 0;
        for (int i = 0; i < _nbPlayer && !this._isGameOver; i++)
        {
            if (_games[i].IsGameOverDone())
            {
                gameOverCounter++;
            }
            if (_games[i].IsGameOver())
            {
                gameOverAskedCounter++;
            }
        }

        if(gameOverCounter >= this._nbPlayer)
        {
            this._isGameOver = true;
        }

        if(gameOverAskedCounter >= this._nbPlayer)
        {
            this._isGameOverAsked = true;
        }

        if (_isGameOverAsked && !_calledOnce)
        {
            _calledOnce = true;

            this.mainEventSystem.gameObject.SetActive(false);

            pauseScript.Hide();
            if (_nbPlayer == 1)
            {
                gameoverScript.Show();
                gameoverScript.SetScore(_games[0].State.Score);
            }
            else if (_nbPlayer == 2)
            {
                gameoverScript.Show(false);
                gameoverScript.SetScore(_games[0].State.Score, _games[1].State.Score);
            }

        }


        if ( !_isGameOver && !_isPause)
        {
            bool pauseAsked = false;
            List<Game.Action> globalActions = _inputV2.GetGlobalActions();
            for (int i = 0, max = globalActions.Count; i < max; i++)
            {
                if (globalActions[i] == Game.Action.Pause)
                {
                    pauseAsked = true;
                }
            }

            if (_waitForRelease)
            {
                if (pauseAsked)
                {
                    pauseAsked = false;
                }
                else
                {
                    _waitForRelease = false;
                }
            }


            if (pauseAsked)
            {
                _isPause = true;
                this.mainEventSystem.gameObject.SetActive(false);

                pauseScript.Show();
                for (int i = 0; i < _nbPlayer; i++)
                {
                    _games[i].SetPause(true);
                }

            }

            for (int i = 0; i < _nbPlayer && !_isPause; i++)
            {
                if (!_games[i].IsGameOverDone())
                {
                    List<Game.Action> actionList = _inputV2.GetPlayerActions(i);
                    if (_nbPlayer == 1)
                    {
                        actionList.AddRange(_inputV2.GetPlayerActions(1));
                    }
                    _games[i].Update(Time.deltaTime, actionList);
                }

            }
        }
    }

    public void ResumeAction()
    {
        for (int i = 0; i < _nbPlayer; i++)
        {
            _games[i].SetPause(false);
        }
        pauseScript.Hide();
        _isPause = false;
        _waitForRelease = true;
        this.mainEventSystem.gameObject.SetActive(true);
    }

}
