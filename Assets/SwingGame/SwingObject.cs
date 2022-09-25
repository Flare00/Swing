using System;
using System.Collections;
using System.Collections.Generic;
using SwingGame.Media;
using UnityEngine;

public class SwingObject
{
    public const int SWING_STATE_EQUAL_HEIGHT = 0;
    public const int SWING_STATE_RIGHT_LOWER = 1;
    public const int SWING_STATE_LEFT_LOWER = 2;
    private Transform _parentTransform;
    private Vector2Int _positionLeft, _positionRight;
    private GameObject _swingObjectLeft, _swingObjectRight, _gearObject, _weightObjectLeft, _weightObjectRight;
    private int _weightColLeft, _weightColRight;
    private int _swingState;
    private SwingAnimator _animator;
    private bool _deactivated;


    public SwingObject(Vector2Int positionLeft, Transform parentTransform)
    {
        Vector2Int positionRight = positionLeft + Vector2Int.right;
        _parentTransform = parentTransform;
        _swingState = SWING_STATE_EQUAL_HEIGHT;

        _positionLeft = positionLeft;
        _positionRight = positionRight;

        _swingObjectLeft = GameObject.Instantiate(Resources.Load("Prefabs/BalanceL", typeof(GameObject))) as GameObject;
        _gearObject = GameObject.Instantiate(Resources.Load("Prefabs/Gear", typeof(GameObject))) as GameObject;
        _swingObjectRight = GameObject.Instantiate(Resources.Load("Prefabs/BalanceR", typeof(GameObject))) as GameObject;
        _weightObjectLeft = GameObject.Instantiate(Resources.Load("Prefabs/BalanceText", typeof(GameObject))) as GameObject;
        _weightObjectRight = GameObject.Instantiate(Resources.Load("Prefabs/BalanceText", typeof(GameObject))) as GameObject;
        _weightObjectLeft.transform.Find("Plane").GetComponent<Renderer>().material.color = new Color(0, 0, 0, 0.75f);
        _weightObjectRight.transform.Find("Plane").GetComponent<Renderer>().material.color = new Color(0, 0, 0, 0.75f);

        _swingObjectLeft.transform.parent = _parentTransform;
        _swingObjectRight.transform.parent = _parentTransform;

        _weightObjectLeft.transform.parent = _parentTransform;
        _weightObjectRight.transform.parent = _parentTransform;

        _swingObjectLeft.transform.Translate(new Vector3(_positionLeft.x * GameZone.SpacingBall, _positionLeft.y * GameZone.SizeBall, 0));
        _swingObjectRight.transform.Translate(new Vector3(_positionRight.x * GameZone.SpacingBall, _positionRight.y * GameZone.SizeBall, 0));


        _gearObject.transform.parent = _parentTransform;
        _gearObject.transform.Translate(new Vector3(((_positionLeft.x + _positionRight.x) / 2.0f) * GameZone.SpacingBall, -0.45f, 0));
        _weightColLeft = 0;
        _weightColRight = 0;
        _deactivated = false;

        _weightObjectLeft.transform.Translate(new Vector3(_positionLeft.x * GameZone.SpacingBall, (_positionLeft.y - 1.1f) * GameZone.SizeBall, -0.1f));
        _weightObjectRight.transform.Translate(new Vector3(_positionRight.x * GameZone.SpacingBall, (_positionRight.y - 1.1f) * GameZone.SizeBall, -0.1f));

    }

    public SwingObject(Vector2Int positionLeft, Transform parentTransform, int state)
    {

        Vector2Int positionRight = positionLeft + Vector2Int.right;
        _parentTransform = parentTransform;

        _positionLeft = positionLeft;
        _positionRight = positionRight;

        this._swingState = state;
        if(state ==SWING_STATE_RIGHT_LOWER){
            _positionLeft.y++;
            _positionRight.y--;
        } 
        else if(state == SWING_STATE_LEFT_LOWER){
            _positionLeft.y--;
            _positionRight.y++;
        } 

        _swingObjectLeft = GameObject.Instantiate(Resources.Load("Prefabs/BalanceL", typeof(GameObject))) as GameObject;
        _gearObject = GameObject.Instantiate(Resources.Load("Prefabs/Gear", typeof(GameObject))) as GameObject;
        _swingObjectRight = GameObject.Instantiate(Resources.Load("Prefabs/BalanceR", typeof(GameObject))) as GameObject;
        _weightObjectLeft = GameObject.Instantiate(Resources.Load("Prefabs/BalanceText", typeof(GameObject))) as GameObject;
        _weightObjectRight = GameObject.Instantiate(Resources.Load("Prefabs/BalanceText", typeof(GameObject))) as GameObject;
        _weightObjectLeft.transform.Find("Plane").GetComponent<Renderer>().material.color = new Color(0, 0, 0, 0.75f);
        _weightObjectRight.transform.Find("Plane").GetComponent<Renderer>().material.color = new Color(0, 0, 0, 0.75f);

        _swingObjectLeft.transform.parent = _parentTransform;
        _swingObjectRight.transform.parent = _parentTransform;

        _weightObjectLeft.transform.parent = _parentTransform;
        _weightObjectRight.transform.parent = _parentTransform;

        _swingObjectLeft.transform.Translate(new Vector3(_positionLeft.x * GameZone.SpacingBall, _positionLeft.y * GameZone.SizeBall, 0));
        _swingObjectRight.transform.Translate(new Vector3(_positionRight.x * GameZone.SpacingBall, _positionRight.y * GameZone.SizeBall, 0));


        _gearObject.transform.parent = _parentTransform;
        _gearObject.transform.Translate(new Vector3(((_positionLeft.x + _positionRight.x) / 2.0f) * GameZone.SpacingBall, -0.45f, 0));
        _weightColLeft = 0;
        _weightColRight = 0;
        _deactivated = false;

        _weightObjectLeft.transform.Translate(new Vector3(_positionLeft.x * GameZone.SpacingBall, (positionLeft.y - 1.1f) * GameZone.SizeBall, -0.1f));
        _weightObjectRight.transform.Translate(new Vector3(_positionRight.x * GameZone.SpacingBall, (positionRight.y - 1.1f) * GameZone.SizeBall, -0.1f));

    
        
    }

    public bool Deactivated
    {
        get => _deactivated;
        set => _deactivated = value;
    }

    public int State
    {
        get => _swingState;
    }

    public SwingAnimator Animator
    {
        get => _animator;
        set => _animator = value;
    }

    public void ResetWeight(bool isLeft)
    {
        if (isLeft)
        {
            _weightColLeft = 0;
            _weightObjectLeft.GetComponentInChildren<TMPro.TextMeshPro>().text = "" + _weightColLeft;
        }
        else
        {
            _weightColRight = 0;
            _weightObjectRight.GetComponentInChildren<TMPro.TextMeshPro>().text = "" + _weightColRight;

        }
    }

    public void AddWeight(bool isLeft, int weight)
    {
        if (isLeft)
        {
            _weightColLeft += weight;
            _weightObjectLeft.GetComponentInChildren<TMPro.TextMeshPro>().text = "" + _weightColLeft;
        }
        else
        {
            _weightColRight += weight;
            _weightObjectRight.GetComponentInChildren<TMPro.TextMeshPro>().text = "" + _weightColRight;
        }
    }

    public bool UpdateSwing(FixedBallContainer[][] playground)
    {
        bool errors = false;
        if (!_deactivated)
        {
            bool swinged = false;
            if (_weightColLeft == _weightColRight && _swingState != SWING_STATE_EQUAL_HEIGHT)
            {
                swinged = true;
                errors = SetSwingState(SWING_STATE_EQUAL_HEIGHT, playground);
            }
            else if (_weightColLeft > _weightColRight && _swingState != SWING_STATE_LEFT_LOWER)
            {
                swinged = true;
                errors = SetSwingState(SWING_STATE_LEFT_LOWER, playground);
            }
            else if (_weightColLeft < _weightColRight && _swingState != SWING_STATE_RIGHT_LOWER)
            {
                swinged = true;
                errors = SetSwingState(SWING_STATE_RIGHT_LOWER, playground);
            }

            if (swinged)
            {
                AudioManager audioManager = AudioManager.GetInstance();
                audioManager.PlaySwingSound(_swingObjectLeft);
            }
        }

        return errors;
    }


    private bool SetSwingState(int swingState, FixedBallContainer[][] playground)
    {
        int leftIncrement = 0;
        bool jumpBall = swingState != SWING_STATE_EQUAL_HEIGHT;
        if (swingState == SWING_STATE_EQUAL_HEIGHT)
        {
            if (_swingState == SWING_STATE_LEFT_LOWER)
            {
                leftIncrement = 1;
            }
            else if (_swingState == SWING_STATE_RIGHT_LOWER)
            {
                leftIncrement = -1;
            }
        }
        else if (swingState == SWING_STATE_LEFT_LOWER)
        {
            if (_swingState == SWING_STATE_EQUAL_HEIGHT)
            {
                leftIncrement = -1;
            }
            else if (_swingState == SWING_STATE_RIGHT_LOWER)
            {
                leftIncrement = -2;
            }
        }
        else if (swingState == SWING_STATE_RIGHT_LOWER)
        {
            if (_swingState == SWING_STATE_EQUAL_HEIGHT)
            {
                leftIncrement = 1;
            }
            else if (_swingState == SWING_STATE_LEFT_LOWER)
            {
                leftIncrement = 2;
            }
        }

        if (leftIncrement == 0) return true;

        bool tiltLeft = leftIncrement < 0;
        int nbIncrement = Math.Abs(leftIncrement);
        Vector2Int positionTilt = tiltLeft ? _positionLeft : _positionRight;
        Vector2Int positionInvTilt = tiltLeft ? _positionRight : _positionLeft;
        int bigWeight = tiltLeft ? _weightColLeft : _weightColRight;
        int smallWeight = tiltLeft ? _weightColRight : _weightColLeft;

        _positionLeft.y += leftIncrement;
        _positionRight.y += -leftIncrement;
        _swingObjectLeft.transform.Translate(new Vector3(0, leftIncrement, 0));
        _swingObjectRight.transform.Translate(new Vector3(0, -leftIncrement, 0));
        _gearObject.transform.Rotate(new Vector3(0, 0, leftIncrement * 90.0f));
        _swingState = swingState;

        int posYFirstBallInvTilt = -1;
        for (int i = GameZone.HeightPlayGround - 1; i >= positionInvTilt.y; i--)
        {
            if (playground[i][positionInvTilt.x].HasBall())
            {
                posYFirstBallInvTilt = i;
                break;
            }

        }
        //Error the column is full
        if (!GameZone.IsInPlaygroundBounds(new Vector2(0, posYFirstBallInvTilt + nbIncrement - (jumpBall && posYFirstBallInvTilt > -1 ? 1 : 0)))) return true;


        List<SwingAnimator.BallAtIndex> ballsAtIndices = _animator.GetBallsUnderSwingPosition(new Vector2Int(positionInvTilt.x, Math.Max(posYFirstBallInvTilt, positionInvTilt.y)),true);

        int infBoundDownDir = nbIncrement;
        int supBoundDownDir = GameZone.HeightPlayGround;
        //DOWN Direction
        for (int i = infBoundDownDir; i < supBoundDownDir; i++)
        {
            if (playground[i][positionTilt.x].HasBall())
            {
                playground[i - nbIncrement][positionTilt.x].Ball = playground[i][positionTilt.x].Ball;
                playground[i][positionTilt.x].Ball = null;
            }
        }

        int supBoundUpDir = posYFirstBallInvTilt;
        int infBoundUpDir = positionInvTilt.y;
        bool ballJumped = false;
        //UP Direction
        for (int i = supBoundUpDir; i >= infBoundUpDir; i--)
        {
            if (playground[i][positionInvTilt.x].HasBall())
            {
                Ball b = playground[i][positionInvTilt.x].PopBall();
                if (jumpBall && !ballJumped)
                {
                    ballJumped = true;
                    _animator.AddFlyingBall(b, GameZone.HeightPlayGround - i, bigWeight - smallWeight,
                        new Vector2Int(positionInvTilt.x, i),
                        tiltLeft ? SwingAnimator.Direction.DirectionLeft : SwingAnimator.Direction.DirectionRight);
                }
                else
                {
                    ballsAtIndices.Add(new SwingAnimator.BallAtIndex(i + nbIncrement, b, false));
                }
            }
        }


        ballsAtIndices.Sort();
        int offsetY = positionInvTilt.y + nbIncrement;
        for (int i = 0; i < ballsAtIndices.Count; i++)
        {
            if (GameZone.IsInPlaygroundBounds(new Vector2(positionInvTilt.x,i + offsetY)))
            {
                playground[i + offsetY][positionInvTilt.x].Ball = ballsAtIndices[i].Ball;
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    public int Height(bool isLeft)
    {
        if (isLeft)
        {
            return _positionLeft.y;
        }
        else
        {
            return _positionRight.y;
        }
    }
}