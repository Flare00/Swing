using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    private static ulong SCORE_BALL = 50 ;
    public static int MULTIPLICATOR_MAX = 4;
    public static int MULTIPLICATOR_MIN = 1;
    private static float TIME_MAX = 5.0f;
    public static int BALL_BY_LEVEL = 30;
    private static int INIT_LEVEL = 3;
    private static int INIT_MULTIPLICATOR = 1;
    private static float TIMING_BETWEEN_GAMEOVER_ROWS = 0.1f;
    private float _gameDuration;
    private int _level;
    private ulong _score;
    private int _multiplicator;
    private int _nbBallDrop;
    private int _nbBallBeforeLevelUp;
    private int _countPowerUp;
    private bool _gameOver;
    private bool _gameOverComputing;
    private int _gameOverRow;
    private float _timingGameOver;
    private float _time;
    private string _name;
    private SpecialBall _nextPu;
    private NormalBall _levelBall;

    public GameState()
    {
        _level = INIT_LEVEL;
        _score = 0;
        _multiplicator = INIT_MULTIPLICATOR;
        _nbBallDrop = 0;
        _nbBallBeforeLevelUp = BALL_BY_LEVEL;
        _gameOver = false;
        _gameOverComputing = false;
        _gameOverRow = 0;
        BallFactory.getInstance().RefreshPu(this);
        BallFactory.getInstance().RefreshLevelBall(this);
    }

    public int Level { get => _level; }
    public ulong Score { get => _score; }
    public int Multiplicator { get => _multiplicator; }
    public float GameDuration { get => _gameDuration; set => _gameDuration = value; }

    public void StartGameOver()
    {
        _gameOverComputing = true;
        _gameOverRow = GameZone.HeightPlayGround;
        _timingGameOver = TIMING_BETWEEN_GAMEOVER_ROWS;
    }

    public bool UpdateGameOver(float deltaT)
    {
        _timingGameOver -= deltaT;
        if (_timingGameOver <= 0)
        {
            if(_gameOverRow <= (GameZone.HeightPlayGround + 1)/2.0f) _gameOver = true;
            if (_gameOverRow == 0)
            {
                _gameOverComputing = false;
            }
            else
            {
                _gameOverRow--;
                _timingGameOver = TIMING_BETWEEN_GAMEOVER_ROWS;
            }

            return true;
        }

        return false;
    }
    public int NbBallDrop
    {
        get => _nbBallDrop;
        set
        {
            _nbBallBeforeLevelUp -= value - _nbBallDrop;
            _nbBallDrop = value;
            if (_nbBallBeforeLevelUp <= 0)
            {
                _nbBallBeforeLevelUp = BALL_BY_LEVEL;
                _level++;
                BallFactory.getInstance().RefreshLevelBall(this);
            }
        }
    }

    public SpecialBall NextPu
    {
        get => _nextPu;
        set => _nextPu = value;
    }

    public NormalBall LevelBall
    {
        get => _levelBall;
        set => _levelBall = value;
    }

    public int CountPowerUp
    {
        get => _countPowerUp;
        set => _countPowerUp = value;
    }
    public bool GameOver { get => _gameOver; set => _gameOver = value; }

    public bool GameOverComputing
    {
        get => _gameOverComputing;
    }

    public int GameOverRow
    {
        get => _gameOverRow;
    }

    public string Name
    {
        get => _name;
        set => _name = value;
    }

    public void AddScoreForExplodingBalls(int totalWeight)
    {
        if (!_gameOverComputing)
        {
            _score = _score + (SCORE_BALL * (ulong)(totalWeight * _multiplicator));
        }
    }


    public void ResetMultiplicator()
    {
        _multiplicator = MULTIPLICATOR_MAX;
        _time = TIME_MAX;
    }

    public bool UpdateMultiplicator(float deltaTime)
    {
        bool res = false;
        _time -= deltaTime;
        if (_time <= 0)
        {

            _time = TIME_MAX;

            if (_multiplicator > MULTIPLICATOR_MIN)
            {
                _multiplicator--;
                res = true;
            }

        }
        return res;
    }
}