using System.Collections;
using System.Collections.Generic;
using System;
using SwingGame.Media;
using UnityEngine;

public class GameZone
{
    public const int HeightPrediction = 2;

    public const int HeightPlayGround = 9;
    public const int RealHeightPlayGround = 8;
    public const int LengthPlayGround = 8;
    public const int StackMergeSize = 5;
    public const int NbSwings = 4;
    public const float SpacingBall = 1.5f;
    public const float SizeBall = 1.0f;
    public const float SpacingPredictionPlayer = 0.1f;
    public const float SpacingPlayerPlayground = 0.1f;
    public const float SpacingFlyingBallPlayground = SpacingPlayerPlayground - SizeBall / 2;
    public const float DistanceExitFlying = 3f * SpacingBall;

    private struct MultiplicatorLightPlayground
    {
        public Material level4_Right, level4_Left;
        public Material level3_Right, level3_Left;
        public Material level2_Right, level2_Left;
    }

    private List<Effect> _listEffect;
    private GameObject _zoneGlobal;
    private GameObject _zonePrediction;
    private GameObject _zonePlayground;
    private GameObject _zoneSwings;
    private GameObject _zonePlayer;

    private FixedBallContainer[][] _playground;
    private FixedBallContainer[][] _prediction;

    private SwingObject[] _swings;

    private GameState _gameState;
    private SwingAnimator _animator;
    private Player _player;

    private GameObject _anchorFlyLeft, _anchorFlyRight;

    private MultiplicatorLightPlayground _multiplicatorLightPlayground;

    public GameObject ZoneGlobal {get => _zoneGlobal;}

    public GameZone(GameState gs)
    {
        _gameState = gs;
        //Init Unity Zone
        _zoneGlobal = new GameObject("Player1");
        _zonePrediction = new GameObject("PredictionZone");
        _zonePlayground = new GameObject("PlayGroundZone");
        _zonePlayer = new GameObject("PlayerZone");
        _zoneSwings = new GameObject("SwingsZone");

        _zonePrediction.transform.parent = _zoneGlobal.transform;
        _zonePlayground.transform.parent = _zoneGlobal.transform;
        _zonePlayer.transform.parent = _zoneGlobal.transform;
        _zoneSwings.transform.parent = _zoneGlobal.transform;

        GameObject decor = GameObject.Instantiate(Resources.Load("Prefabs/Decor", typeof(GameObject))) as GameObject;
        _multiplicatorLightPlayground.level2_Left =
            decor.transform.Find("Light_Mul2").Find("Left").GetComponent<Renderer>().material;
        _multiplicatorLightPlayground.level2_Right =
            decor.transform.Find("Light_Mul2").Find("Right").GetComponent<Renderer>().material;
        _multiplicatorLightPlayground.level3_Left =
            decor.transform.Find("Light_Mul3").Find("Left").GetComponent<Renderer>().material;
        _multiplicatorLightPlayground.level3_Right =
            decor.transform.Find("Light_Mul3").Find("Right").GetComponent<Renderer>().material;
        _multiplicatorLightPlayground.level4_Left =
            decor.transform.Find("Light_Mul4").Find("Left").GetComponent<Renderer>().material;
        _multiplicatorLightPlayground.level4_Right =
            decor.transform.Find("Light_Mul4").Find("Right").GetComponent<Renderer>().material;
        decor.transform.parent = _zoneGlobal.transform;

        _listEffect = new List<Effect>();

        //Init Player
        _player = new Player(_gameState);
        _player.ContainerObject.transform.parent = _zonePlayer.transform;
        _anchorFlyLeft = new GameObject("anchorSwingLeft");
        _anchorFlyLeft.transform.parent = _zonePlayer.transform;
        _anchorFlyLeft.transform.position = new Vector3(-DistanceExitFlying, 0, 0);
        _anchorFlyRight = new GameObject("anchorSwingRight");
        _anchorFlyRight.transform.parent = _zonePlayer.transform;
        _anchorFlyRight.transform.position =
            new Vector3((((NbSwings * 2) - 1) * SpacingBall) + DistanceExitFlying, 0, 0);


        //Init Prediction Tab
        _prediction = new FixedBallContainer[HeightPrediction][];
        for (int i = 0; i < HeightPrediction; i++)
        {
            _prediction[i] = new FixedBallContainer[LengthPlayGround];
            for (int j = 0; j < LengthPlayGround; j++)
            {
                _prediction[i][j] = new FixedBallContainer(new Vector3(j * SpacingBall, i),
                    BallFactory.getInstance().GenerateBall(_gameState));
                _prediction[i][j].ContainerObject.transform.parent = _zonePrediction.transform;
            }
        }


        //Init Playground Tab
        _playground = new FixedBallContainer[HeightPlayGround][];
        for (int i = 0; i < HeightPlayGround; i++)
        {
            _playground[i] = new FixedBallContainer[LengthPlayGround];
            for (int j = 0; j < LengthPlayGround; j++)
            {
                _playground[i][j] = new FixedBallContainer(new Vector3(j * SpacingBall, i));
                _playground[i][j].ContainerObject.transform.parent = _zonePlayground.transform;
            }
        }


        //Init Swing Objects
        _swings = new SwingObject[NbSwings];
        for (int i = 0; i < NbSwings; i++)
        {
            _swings[i] = new SwingObject(new Vector2Int(2 * i, 1), _zoneSwings.transform);
        }

        //Init Animator
        _animator = new SwingAnimator(this, _swings);
        if (_gameState.Multiplayer && MultiplayerSystem.getInstance() != null)
            MultiplayerSystem.getInstance().SubscribePlayer(_animator);

        _zonePrediction.transform.Translate(0.5f, -HeightPrediction, 0);
        _zonePlayer.transform.Translate(0.5f, -HeightPrediction - SizeBall - SpacingPredictionPlayer, 0);
        _zonePlayground.transform.Translate(0.5f, -HeightPrediction - RealHeightPlayGround - SpacingPredictionPlayer - SpacingPlayerPlayground - (2 * SizeBall), 0);
        _zoneSwings.transform.Translate(0.5f, -HeightPrediction - HeightPlayGround - SpacingPredictionPlayer - SpacingPlayerPlayground - (2 * SizeBall), 0);

        _zoneGlobal.transform.Translate((-LengthPlayGround * SpacingBall / 2.0f) + 0.25f, 7, 0);
    }

    public void UpdateMultiplicatorLight()
    {
        Color grey = new Color(0.1f, 0.1f, 0.1f);
        Color lv4 = Color.HSVToRGB(0, 1.0f, 1.0f);
        Color lv3 = Color.HSVToRGB(0.027f, 1.0f, 1.0f);
        Color lv2 = Color.HSVToRGB(0.055f, 1.0f, 1.0f);

        switch (_gameState.Multiplicator)
        {
            case 4:
                //Enable Color
                //Level 4
                _multiplicatorLightPlayground.level4_Left.SetColor("_BaseColor", lv4);
                _multiplicatorLightPlayground.level4_Right.SetColor("_BaseColor", lv4);
                _multiplicatorLightPlayground.level4_Left.SetColor("_EmissionColor", lv4);
                _multiplicatorLightPlayground.level4_Right.SetColor("_EmissionColor", lv4);
                //Level 3
                _multiplicatorLightPlayground.level3_Left.SetColor("_BaseColor", lv3);
                _multiplicatorLightPlayground.level3_Right.SetColor("_BaseColor", lv3);
                _multiplicatorLightPlayground.level3_Left.SetColor("_EmissionColor", lv3);
                _multiplicatorLightPlayground.level3_Right.SetColor("_EmissionColor", lv3);
                //Level 2
                _multiplicatorLightPlayground.level2_Left.SetColor("_BaseColor", lv2);
                _multiplicatorLightPlayground.level2_Right.SetColor("_BaseColor", lv2);
                _multiplicatorLightPlayground.level2_Left.SetColor("_EmissionColor", lv2);
                _multiplicatorLightPlayground.level2_Right.SetColor("_EmissionColor", lv2);

                break;
            case 3:
                //Disable Color
                //Level 4
                _multiplicatorLightPlayground.level4_Left.SetColor("_BaseColor", grey);
                _multiplicatorLightPlayground.level4_Right.SetColor("_BaseColor", grey);
                _multiplicatorLightPlayground.level4_Left.SetColor("_EmissionColor", Color.black);
                _multiplicatorLightPlayground.level4_Right.SetColor("_EmissionColor", Color.black);

                //Enable Color
                //Level 3
                _multiplicatorLightPlayground.level3_Left.SetColor("_BaseColor", lv3);
                _multiplicatorLightPlayground.level3_Right.SetColor("_BaseColor", lv3);
                _multiplicatorLightPlayground.level3_Left.SetColor("_EmissionColor", lv3);
                _multiplicatorLightPlayground.level3_Right.SetColor("_EmissionColor", lv3);
                //Level 2
                _multiplicatorLightPlayground.level2_Left.SetColor("_BaseColor", lv2);
                _multiplicatorLightPlayground.level2_Right.SetColor("_BaseColor", lv2);
                _multiplicatorLightPlayground.level2_Left.SetColor("_EmissionColor", lv2);
                _multiplicatorLightPlayground.level2_Right.SetColor("_EmissionColor", lv2);
                break;
            case 2:
                //Disable Color
                //Level 4
                _multiplicatorLightPlayground.level4_Left.SetColor("_BaseColor", grey);
                _multiplicatorLightPlayground.level4_Right.SetColor("_BaseColor", grey);
                _multiplicatorLightPlayground.level4_Left.SetColor("_EmissionColor", Color.black);
                _multiplicatorLightPlayground.level4_Right.SetColor("_EmissionColor", Color.black);
                //Level 3
                _multiplicatorLightPlayground.level3_Left.SetColor("_BaseColor", grey);
                _multiplicatorLightPlayground.level3_Right.SetColor("_BaseColor", grey);
                _multiplicatorLightPlayground.level3_Left.SetColor("_EmissionColor", Color.black);
                _multiplicatorLightPlayground.level3_Right.SetColor("_EmissionColor", Color.black);

                //Level 2
                _multiplicatorLightPlayground.level2_Left.SetColor("_BaseColor", lv2);
                _multiplicatorLightPlayground.level2_Right.SetColor("_BaseColor", lv2);
                _multiplicatorLightPlayground.level2_Left.SetColor("_EmissionColor", lv2);
                _multiplicatorLightPlayground.level2_Right.SetColor("_EmissionColor", lv2);
                break;
            case 1:

                //Disable Color
                //Level 4
                _multiplicatorLightPlayground.level4_Left.SetColor("_BaseColor", grey);
                _multiplicatorLightPlayground.level4_Right.SetColor("_BaseColor", grey);
                _multiplicatorLightPlayground.level4_Left.SetColor("_EmissionColor", Color.black);
                _multiplicatorLightPlayground.level4_Right.SetColor("_EmissionColor", Color.black);
                //Level 3
                _multiplicatorLightPlayground.level3_Left.SetColor("_BaseColor", grey);
                _multiplicatorLightPlayground.level3_Right.SetColor("_BaseColor", grey);
                _multiplicatorLightPlayground.level3_Left.SetColor("_EmissionColor", Color.black);
                _multiplicatorLightPlayground.level3_Right.SetColor("_EmissionColor", Color.black);
                //Level 2
                _multiplicatorLightPlayground.level2_Left.SetColor("_BaseColor", grey);
                _multiplicatorLightPlayground.level2_Right.SetColor("_BaseColor", grey);
                _multiplicatorLightPlayground.level2_Left.SetColor("_EmissionColor", Color.black);
                _multiplicatorLightPlayground.level2_Right.SetColor("_EmissionColor", Color.black);
                break;
        }
    }

    public static bool IsInPlaygroundBounds(Vector2 position, bool realBounds = false)
    {
        return position.x >= 0 && position.x < LengthPlayGround && position.y >= 0 &&
               position.y < (realBounds ? RealHeightPlayGround : HeightPlayGround);
    }

    public bool IsPositionFree(Vector2Int position, bool checkBall = true, bool realPlaygroundBounds = false)
    {
        return IsInPlaygroundBounds(position, realPlaygroundBounds) &&
               _swings[position.x / 2].Height(position.x % 2 == 0) - 1 != position.y &&
               (!_playground[position.y][position.x].HasBall() || !checkBall);
    }

    public void SetDeactivateSwingAtCol(int col, bool deactivated)
    {
        _swings[col / 2].Deactivated = deactivated;
    }

    public void DropBall()
    {
        _player.DropAnim();

        AudioManager audioManager = AudioManager.GetInstance();
        audioManager.PlayDropBallSound(_player.ContainerObject);

        if (!_gameState.GameOverComputing)
        {
            bool generateBall = false;
            if (_player.HasBall())
            {
                if (_animator.CanDropBallAt(_player.Position, HeightPlayGround - 1))
                {
                    _player.Ball.BallObject.transform.parent = _zoneGlobal.transform;
                    _animator.AddDropingBall(_player.Ball, new Vector2(_player.Position, HeightPlayGround + SpacingPlayerPlayground),  SpacingPlayerPlayground);
                    _gameState.NbBallDrop++;
                    _gameState.CountPowerUp--;
                    generateBall = true;
                }
            }
            else
            {
                generateBall = true;
            }

            if (generateBall)
            {
                _player.Ball = _prediction[0][_player.Position].Ball;
                _prediction[0][_player.Position].Ball = _prediction[1][_player.Position].Ball;
                _prediction[1][_player.Position].Ball = BallFactory.getInstance().GenerateBall(_gameState);
            }
        }
    }

    public GameObject Zone
    {
        get => _zoneGlobal;
    }

    public FixedBallContainer[][] Playground
    {
        get => _playground;
        set => _playground = value;
    }

    public FixedBallContainer[][] Prediction
    {
        get => _prediction;
        set => _prediction = value;
    }

    public GameState GameState
    {
        get => _gameState;
        set => _gameState = value;
    }

    public SwingAnimator Animator
    {
        get => _animator;
        set => _animator = value;
    }

    public Player Player
    {
        get => _player;
        set => _player = value;
    }

    public GameObject AnchorFlyLeft
    {
        get => _anchorFlyLeft;
    }

    public GameObject AnchorFlyRight
    {
        get => _anchorFlyRight;
    }

    public void AddEffect(Effect e)
    {
        this._listEffect.Add(e);
    }

    public void UpdateEffect()
    {
        List<Effect> ts = new List<Effect>(this._listEffect);
        for (int i = 0; i < ts.Count; i++)
        {
            if (ts[i].IsEnd())
            {
                this._listEffect.Remove(ts[i]);
            }
        }
    }

    public void ComputeGameZone(float deltaT)
    {
        //If Multiplcator Changed update lights multiplicator
        if (_gameState.UpdateMultiplicator(deltaT))
        {
            UpdateMultiplicatorLight();
        }

        //Animate all the floating Ball
        if (Animator.Animate(deltaT) && !_gameState.GameOverComputing) _gameState.StartGameOver();

        //Update the weights of the swings and compute the swings
        UpdateWeightSwings();
        if (ComputeSwing() && !_gameState.GameOverComputing) _gameState.StartGameOver();

        if (!_gameState.GameOverComputing)
        {
            //Compute the effects of Power Ups
            ComputePowerUps();
            //Compute Alignment of Normal balls
            ComputeAlignment();
            //Compute Stack of Normal balls
            ComputeStack();
            //Start a GameOver if the last row is not empty
            if (!IsLastRowEmpty() && !_gameState.GameOverComputing) _gameState.StartGameOver();
        }

        if (_gameState.GameOverComputing)
        {
            //Explode the GameZone
            if (_gameState.UpdateGameOver(deltaT)) ExplodeStepGameZone();
        }

        //Update all the VFXs
        UpdateEffect();
    }

    private bool IsLastRowEmpty()
    {
        for (int i = 0; i < LengthPlayGround; i++)
        {
            if (_playground[HeightPlayGround - 1][i].HasBall()) return false;
        }

        return true;
    }

    private void UpdateWeightSwings()
    {
        for (int i = 0; i < LengthPlayGround; i++)
        {
            int indexSwing = i / 2;
            bool isLeft = i % 2 == 0;
            _swings[indexSwing].ResetWeight(isLeft);

            for (int j = 0; j < HeightPlayGround; j++)
            {
                if (_playground[j][i].HasBall())
                {
                    _swings[indexSwing].AddWeight(isLeft, _playground[j][i].Ball.Weight);
                }
            }
        }
    }

    private bool ComputeSwing()
    {
        for (int i = 0; i < NbSwings; i++)
        {
            if (_swings[i].UpdateSwing(_playground)) return true;
        }

        return false;
    }

    private void ComputePowerUps()
    {
        // TODO : MODIFY THE COMPLEXITY ( O(n²) )

        // Seek every Power ups in the playground
        for (int i = 0; i < LengthPlayGround; i++)
        {
            for (int j = 0; j < HeightPlayGround; j++)
            {
                if (_playground[j][i].HasBall())
                {
                    if (!_playground[j][i].Ball.IsNormalBall)
                    {
                        // Call action on every power up
                        _playground[j][i].Ball.Action(this, i, j);
                    }
                }
            }
        }
    }

    private void ComputeStack()
    {
        for (int col = 0; col < LengthPlayGround; col++)
        {
            //We only update a col if there isn't droping ball in it
            if (_animator.NbDropingBallAtCol(col) == 0)
            {
                int i = _swings[col / 2].Height(col % 2 == 0);
                int precMaterial = -1, count = 0;
                int mergedLine = -1;
                //while we havent reach the top of the stack
                while (i < HeightPlayGround && _playground[i][col].HasBall())
                {
                    //we add count if ball has same material
                    if (_playground[i][col].Ball.IdMaterial == precMaterial && _playground[i][col].Ball.IsNormalBall)
                    {
                        count++;
                    }
                    else count = 0;

                    //top ball falling
                    if (mergedLine != -1)
                    {
                        Ball b = _playground[i][col].Ball;
                        _playground[i][col].Ball = null;
                        _animator.AddDropingBall(b, new Vector2Int(i, mergedLine));
                        mergedLine++;
                    }

                    //if we found 5 stacking ball, merging them
                    if (count == StackMergeSize - 1)
                    {
                        while (i + 1 < HeightPlayGround && _playground[i + 1][col].HasBall() &&
                               _playground[i + 1][col].Ball.IdMaterial == precMaterial)
                        {
                            i++;
                            count++;
                        }

                        //TODO Merging animation
                        int sum = 0;
                        for (int k = 0; k < count; k++)
                        {
                            //removing each ball
                            sum += _playground[i - k][col].Ball.Weight;
                            _playground[i - k][col].ExplodeBall(this, Effect.EffectType.BallTransform);
                        }

                        //making the bottom ball weight the sum of all mergerd balls.
                        _playground[i - count][col].Ball.Weight += sum;
                        count = 0;
                        mergedLine = i;
                    }

                    if (_playground[i][col].HasBall())
                        precMaterial = _playground[i][col].Ball.IdMaterial;


                    i++;
                }
            }
        }
    }

        private void ComputeAlignment()
    {
        List<Vector2Int> destroyBall = new List<Vector2Int>();
        bool[]
            processedCol =
                new bool[LengthPlayGround]; //tableau contenant les colonnes updatées (pour la chute de balles)
        for (int i = 0; i < LengthPlayGround; i++)
        {
            processedCol[i] = false;
        }


        int totalWeight = 0;


        //Parcours du tableau
        for (int col = 0; col < LengthPlayGround; col++)
        for (int i = 0; i < HeightPlayGround; i++)
        {
            if (_playground[i][col].HasBall())
            {
                int material = -1; //material = -1 means no previous ball
                int colmin = col, colmax = col;
                int count = 0;
                //au niveau de la boule, on parcours vers la droite pour trouver des boules à aligner
                if (_playground[i][col].Ball.IsNormalBall)
                    material = _playground[i][col].Ball.IdMaterial;
                while (colmax < LengthPlayGround &&
                       _playground[i][colmax].HasBall() &&
                       (material == -1 && _playground[i][colmax].Ball.IsNormalBall ||
                        _playground[i][colmax].Ball.IdMaterial == material &&
                        _playground[i][colmax].Ball.IsNormalBall ||
                        _playground[i][colmax].Ball is JokerBall))
                {
                    if (material == -1 && _playground[i][colmax].Ball.IsNormalBall)
                    {
                        material = _playground[i][colmax].Ball.IdMaterial;
                    }

                    colmax++;
                    count++;
                }

                //on passe dans ce if si on a trouvé un alignement à 3 boules
                if (count >= 3 || colmin - colmax >= 3)
                {
                    bool[] usedCol = new bool[LengthPlayGround];
                    for (int k = 0; k < LengthPlayGround; k++)
                    {
                        usedCol[k] = false;
                    }

                    List<Vector2Int> alignedBalls = new List<Vector2Int>();
                    //We find all adjascent from the first ball
                    int weight = FindAdjacency(alignedBalls, usedCol, i, colmin, material);

                    bool cancelled = false;

                    for(int k=0;k<LengthPlayGround;k++)
                        if (usedCol[k])
                            foreach(Ball b in _animator.GetBallsBetweenPosition(k,0,HeightPlayGround-1)){
                                if(b.IdMaterial==material || b is JokerBall)
                                    cancelled = true;
                            }
                                
                    if(!cancelled){
                        foreach (var pos in alignedBalls)
                        {
                            destroyBall.Add(pos);
                        }
                        totalWeight += weight;
                    }
                }
            }
        }

        while (destroyBall.Count != 0)
        {
            Vector2Int pos = destroyBall[0];
            destroyBall.Remove(pos);
            _playground[pos.x][pos.y].ExplodeBall(this, Effect.EffectType.BallAlign);
        }

        if (totalWeight > 0)
        {
            _gameState.AddScoreForExplodingBalls(totalWeight);
            _gameState.ResetMultiplicator();
            UpdateMultiplicatorLight();
        }

        //Process about the ball fall
        for (int k = 0; k < LengthPlayGround; k++)
        {
            int firstFree = -1; //set the first empty space to -1
            for (int l = _swings[k / 2].Height(k % 2 == 0); l < HeightPlayGround; l++)
            {
                if (!_playground[l][k].HasBall())
                {
                    if (firstFree == -1)
                    {
                        //if an empty space is found (the first one)
                        firstFree = l;
                    }
                }
                else
                {
                    if (firstFree != -1)
                    {
                        //a ball was found on top of an empty space, making it fall to empty space and incrementing it by 1.
                        Ball b = _playground[l][k].PopBall();
                        _animator.AddDropingBall(b, new Vector2Int(k, l));
                        firstFree++;
                    }
                }
            }
        }
    }


    private int FindAdjacency(List<Vector2Int> alignedBalls, bool[] processedCol, int line, int col, int material)
    {
        Vector2Int pos = new Vector2Int(line, col);

        alignedBalls.Add(pos);
        int weight = 0;

        //line+1
        if (line + 1 < HeightPlayGround && _playground[line + 1][col].HasBall() &&
            (_playground[line + 1][col].Ball.IdMaterial == material || material == -1 &&
                                                                    _playground[line + 1][col].Ball is NormalBall
                                                                    || _playground[line + 1][col].Ball is JokerBall))
        {
            Vector2Int p = new Vector2Int(line + 1, col);
            if (material == -1)
                material = _playground[line + 1][col].Ball.IdMaterial;
            if (!alignedBalls.Contains(p))
                weight += FindAdjacency(alignedBalls, processedCol, line + 1, col, material);
        }

        //line-1
        if (line - 1 >= 0 && _playground[line - 1][col].HasBall() &&
            (_playground[line - 1][col].Ball.IdMaterial == material || material == -1 &&
                                                                    _playground[line - 1][col].Ball is NormalBall
                                                                    || _playground[line - 1][col].Ball is JokerBall))
        {
            Vector2Int p = new Vector2Int(line - 1, col);
            if (material == -1)
                material = _playground[line - 1][col].Ball.IdMaterial;
            if (!alignedBalls.Contains(p))
                weight += FindAdjacency(alignedBalls, processedCol, line - 1, col, material);
        }

        //col+1
        if (col + 1 < LengthPlayGround && _playground[line][col + 1].HasBall() &&
            (_playground[line][col + 1].Ball.IdMaterial == material || material == -1 &&
                                                                    _playground[line][col + 1].Ball is NormalBall
                                                                    || _playground[line][col + 1].Ball is JokerBall))
        {
            Vector2Int p = new Vector2Int(line, col + 1);
            if (material == -1)
                material = _playground[line][col + 1].Ball.IdMaterial;
            if (!alignedBalls.Contains(p))
                weight += FindAdjacency(alignedBalls, processedCol, line, col + 1, material);
        }

        //col-1
        if (col - 1 >= 0 && _playground[line][col - 1].HasBall() &&
            (_playground[line][col - 1].Ball.IdMaterial == material || material == -1 &&
                                                                    _playground[line][col - 1].Ball is NormalBall
                                                                    || _playground[line][col - 1].Ball is JokerBall))
        {
            Vector2Int p = new Vector2Int(line, col - 1);
            if (material == -1)
                material = _playground[line][col - 1].Ball.IdMaterial;
            if (!alignedBalls.Contains(p))
                weight += FindAdjacency(alignedBalls, processedCol, line, col - 1, material);
        }

        // we give score to the player for each deleted ball and we delete the ball from the grid
        weight += _playground[line][col].Ball.Weight;
        processedCol[col] = true;
        return weight;
    }

    private void ExplodeStepGameZone()
    {
        _animator.ExplodeFloatingBalls();

        for (int i = _gameState.GameOverRow; i < HeightPlayGround; i++)
        {
            for (int j = 0; j < LengthPlayGround; j++)
            {
                _playground[i][j].ExplodeBall(this, Effect.EffectType.BallExplosion);
            }
        }
    }
}