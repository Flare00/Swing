using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Game
{

    public enum Action
    {
        Drop,
        Right,
        Left,
        Pause,
        GameOver
    }


    private struct HUD
    {
        public GameObject gameHUD;
        public GameObject levelContainer;
        public GameObject scoreValue;
        public GameObject nbBallDropValue;
        public GameObject countPowerUpValue;
        public GameObject timer_value;
        public GameObject nextPuContainer;
    }

    private GameObject _playerCamera;
    private Camera _mainCamera;
    private Camera _overlayCamera;
    private HUD _hud;

    private GameState _state;
    private GameZone _zone;
    private bool _pause;
    private bool _multijoueur;
    private bool _initInUpdate = true;
    private double _playTime = 0.0;

    public Game(bool multijoueur, int aspectMode, int idJoueur = 0)
    {
        _multijoueur = multijoueur;
        _pause = false;
        _state = new GameState();
        _zone = new GameZone(_state);

        _state.Name = "Player " + idJoueur + 1;
        this._playerCamera = GameObject.Instantiate(Resources.Load("Prefabs/CameraPlayer", typeof(GameObject))) as GameObject;
        _mainCamera = _playerCamera.transform.Find("MainCamera").gameObject.GetComponent<Camera>();
        _overlayCamera = _playerCamera.transform.Find("OverlayCamera").gameObject.GetComponent<Camera>();

        switch (aspectMode)
        {
            case 0:
                _hud.gameHUD = _overlayCamera.transform.Find("GameHUD_16-9").gameObject;
                _hud.gameHUD.gameObject.SetActive(true);
                break;
            case 1:
                _mainCamera.transform.position = new Vector3(0, 0, -57.0f);
                _hud.gameHUD = _overlayCamera.transform.Find("GameHUD_4-3").gameObject;
                _hud.gameHUD.gameObject.SetActive(true);
                break;
            case 2:
                _mainCamera.transform.position = new Vector3(0, 1.7f, -45.0f);
                _hud.gameHUD = _overlayCamera.transform.Find("GameHUD_16-9Multi").gameObject;
                _hud.gameHUD.gameObject.SetActive(true);
                break;
            case 3:
                _mainCamera.transform.position = new Vector3(0, 4.0f, -42.0f);
                _hud.gameHUD = _overlayCamera.transform.Find("GameHUD_4-3Multi").gameObject;
                _hud.gameHUD.gameObject.SetActive(true);
                break;
        }

        _hud.gameHUD.GetComponentInChildren<Canvas>().worldCamera = this._overlayCamera;
        _hud.gameHUD.GetComponentInChildren<Canvas>().planeDistance = 1;
        _playerCamera.transform.parent = _zone.Zone.transform;
        _hud.gameHUD.transform.parent = _zone.Zone.transform;
        SetupHUD();
        //creer l'écran de gameover
        /*_gameOverScreen = GameObject.Instantiate(Resources.Load("Prefabs/GameOverScreen", typeof(GameObject))) as GameObject;
        _gameOverScreen.GetComponentInChildren<Canvas>().worldCamera = this._overlayCamera;
        _gameOverScreen.GetComponentInChildren<Canvas>().planeDistance = 1;
        _gameOverScreen.transform.parent = _hud.gameHUD.transform;
        //met le gameover en transparent
        _gameOverScreen.GetComponentInChildren<CanvasGroup>().alpha = 0.0f;*/
        //Change les information selon l'aspectRatio

        //Met en place les zone de joueur selon si c'est en multijoueur ou en solo
        if (multijoueur)
        {
            if (idJoueur == 0)
            {
                //Divise la taille de la camera par deux avec un offset de 0 pour pouvoir mettre 2 camera, celle ci 1er position
                _mainCamera.rect = new Rect(0.0f, 0.0f, 0.5f, 1.0f);
                _overlayCamera.rect = _mainCamera.rect;

                //Déplace loin la zone du joueur pour que les camera ne filme pas des environement non voulu.
                _zone.Zone.transform.position = new Vector3(-10.0f, 0.0f, 0.0f);
                //renome la zone en "Player1" pour qu'elle soit visible sur Unity
                _zone.Zone.name = "Player1";
                //déplace le HUD pour qu'il soit visible

            }
            else if (idJoueur == 1)
            {
                //Divise la taille de la camera par deux avec un offset de 0.5f pour pouvoir mettre 2 camera,celle ci en 2e position
                _mainCamera.rect = new Rect(0.5f, 0.0f, 0.5f, 1.0f);
                _overlayCamera.rect = _mainCamera.rect;
                //Déplace loin la zone du joueur pour que les cameras ne filme pas des environement non voulu.
                _zone.Zone.transform.position = new Vector3(10.0f, 0.0f, 0.0f);
                //renome la zone en "Player2" pour qu'elle soit visible sur Unity
                _zone.Zone.name = "Player2";

                //déplace le HUD pour qu'il soit visible
            }

        }
    }

    public GameState State { get => _state; set => _state = value; }
    public GameZone Zone { get => _zone; set => _zone = value; }

    public void Update(float deltaT, List<Action> actions)
    {

        if (_initInUpdate)
        {
            _initInUpdate = false;
            _zone.UpdateMultiplicatorLight();
        }
        List<Action> used = new List<Action>();
        for (int i = 0, max = actions.Count; i < max; i++)
        {
            bool isUsed = false;
            for (int j = 0, maxj = used.Count; j < maxj && !isUsed; j++)
            {
                if (actions[i] == used[j])
                    isUsed = true;
            }
            if (!isUsed)
            {
                DoAction(actions[i]);
                used.Add(actions[i]);
            }
        }



        if (!_pause)
        {
            if(!_state.GameOver) _playTime += deltaT;
            RefreshHUD();
            _zone.ComputeGameZone(deltaT);
        }
    }

    private void SetupHUD()
    {
        _hud.levelContainer = _hud.gameHUD.transform.Find("Canvas/LevelSphereContainer/LevelSphere").gameObject;
        _hud.scoreValue = _hud.gameHUD.transform.Find("Canvas/NameAndScore/Value_Score").gameObject;
        _hud.nbBallDropValue = _hud.gameHUD.transform.Find("Canvas/NbBallDrop/Value_BallDrop").gameObject;
        _hud.countPowerUpValue = _hud.gameHUD.transform.Find("Canvas/NextPU/Value_NextPU").gameObject;
        _hud.timer_value = _hud.gameHUD.transform.Find("Canvas/Time/Value_Time").gameObject;
        _hud.nextPuContainer = _hud.gameHUD.transform.Find("Canvas/NextPU/NextPUSphere").gameObject;
    }

    private void RefreshHUD()
    {
        _hud.scoreValue.GetComponent<TMPro.TextMeshProUGUI>().text =  _state.Score.ToString("n0");
        _hud.nbBallDropValue.GetComponent<TMPro.TextMeshProUGUI>().text = "" + _state.NbBallDrop;
        _hud.countPowerUpValue.GetComponent<TMPro.TextMeshProUGUI>().text = "" + _state.CountPowerUp;
        
        //Calcul du temps
        System.TimeSpan time = System.TimeSpan.FromMilliseconds(this._playTime*1000);
        if (time.Hours > 0)
        {
            _hud.timer_value.GetComponent<TMPro.TextMeshProUGUI>().text = time.Hours + (time.Minutes < 10 ? ":0" : ":") + time.Minutes + (time.Seconds < 10 ? ":0" : ":") + time.Seconds;
        }
        else
        {
            _hud.timer_value.GetComponent<TMPro.TextMeshProUGUI>().text = time.Minutes + (time.Seconds < 10 ? ":0" : ":") + time.Seconds;
        }

        if(_hud.nextPuContainer.transform.childCount > 0)
        {
            if(_hud.nextPuContainer.transform.GetChild(0).gameObject != _state.NextPu.BallObject.gameObject)
            {
                GameObject.Destroy(_hud.nextPuContainer.transform.GetChild(0).gameObject);
                _state.NextPu.BallObject.transform.parent = _hud.nextPuContainer.transform;
                _state.NextPu.BallObject.transform.localPosition = new Vector3(-0.5f, 0.5f, 0.0f);
            }
        } 
        else if (_state.NextPu.BallObject != null)
        {
            _state.NextPu.BallObject.transform.parent = _hud.nextPuContainer.transform;
            _state.NextPu.BallObject.transform.localPosition = new Vector3(-0.5f,0.5f,0);
        }
        
        if(_hud.levelContainer.transform.childCount > 0)
        {
            if(_hud.levelContainer.transform.GetChild(0).gameObject != _state.LevelBall.BallObject.gameObject)
            {
                GameObject.Destroy(_hud.levelContainer.transform.GetChild(0).gameObject);
                _state.LevelBall.BallObject.transform.parent = _hud.levelContainer.transform;
                _state.LevelBall.BallObject.transform.localPosition = new Vector3(-0.5f, 0.5f, 0);
            }
        } 
        else if (_state.LevelBall.BallObject != null)
        {
            _state.LevelBall.BallObject.transform.parent = _hud.levelContainer.transform;
            _state.LevelBall.BallObject.transform.localPosition = new Vector3(-0.5f,0.5f,0);
        }
    }

    private void DoAction(Action action)
    {
        if (_pause && action == Action.Pause)
        {
            _pause = false;
        }
        else if (!_pause)
        {
            switch (action)
            {
                case Action.Pause:
                    _pause = true;
                    break;
                case Action.Drop: _zone.DropBall(); break;
                case Action.Right: _zone.Player.MoveRight(); break;
                case Action.Left: _zone.Player.MoveLeft(); break;
            }
        }
    }

    public void SetPause(bool pause)
    {
        this._pause = pause;
    }

    public bool IsGameOver()
    {
        return this._state.GameOver;
    }
    
    public bool IsGameOverDone()
    {
        return _state.GameOver && !_state.GameOverComputing;
    }

    public ulong GetScore()
    {
        return _state.Score;
    }


}
