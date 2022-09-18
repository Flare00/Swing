using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingBallContainer : BallContainer
{

    private SwingAnimator.StateAnim _state; // Up, Side or Down
    private Vector2 _position;
    private float _nbUp; // Number of deplacement vertically
    private float _nbSide; // Number of deplacement horizontally
    private float _nbSideTotal; // Number of deplacement horizontally
    private SwingAnimator.Direction _direction; // right:true or left:false
    private bool _paused;
    private BallAnimation _animation;

    public FloatingBallContainer(Ball ball, SwingAnimator.StateAnim state, Vector2 position,
        float nbSide, float nbUp, SwingAnimator.Direction directionSide, SwingAnimator.Direction directionUp) : base(ball)
    {
        _paused = false;
        _state = state;
        _position = position;
        _nbSide = nbSide;
        _nbUp = nbUp;
        _nbSideTotal = nbSide;
        _direction = directionSide;
        _animation =
            new BallAnimation(
                SwingAnimator.Orientation.Vertical,
                directionUp,
                nbUp);
    }

    public bool Paused
    {
        get => _paused;
        set => _paused = value;
    }

    public SwingAnimator.StateAnim State
    {
        get => _state;
        set => _state = value;
    }

    public Vector2 Position
    {
        get => _position;
        set => _position = value;
    }

    public float NbSide
    {
        get => _nbSide;
        set => _nbSide = value;
    }
    
    public float NbSideTotal
    {
        get => _nbSideTotal;
        set => _nbSideTotal = value;
    }

    public float NbUp
    {
        get => _nbUp;
        set => _nbUp = value;
    }

    public SwingAnimator.Direction Direction
    {
        get => _direction;
        set => _direction = value;
    }

    public BallAnimation Animation
    {
        get => _animation;
        set => _animation = value;
    }

    public void TransformBall()
    {
        Ball b = BallFactory.getInstance().GetTransformedFloatingBall(Ball);
        Vector3 positionBall = Ball.BallObject.transform.position; //Keep the ball position
        RemoveBall();
        Ball = b;
        Ball.BallObject.transform.position = positionBall;
    }

    public void TransformBallVersus()
    {
        if(this.Ball.GetType() != typeof(BrickBall))
        {
            Ball b = BallFactory.getInstance().GetTransformedFloatingBallVersus(Ball);
            Vector3 positionBall = Ball.BallObject.transform.position; //Keep the ball position
            RemoveBall();
            Ball = b;
            Ball.BallObject.transform.position = positionBall;
        }
    }
}