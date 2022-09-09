using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BallAnimation
{
    private SwingAnimator.Orientation _vertical; // vertical or horizontal
    private SwingAnimator.Direction _direction; // right:true or left:false OR up:true or down:false
    private float _depl; // Number of case to be deplaced
    private float _timing; // Timing of the animation remaining

    public BallAnimation(SwingAnimator.Orientation vertical, SwingAnimator.Direction direction, float depl)
    {
        _vertical = vertical;
        _direction = direction;
        _depl = depl;
        InitTiming();
    }

    public void Change(SwingAnimator.Orientation vertical, SwingAnimator.Direction direction, float depl)
    {
        _vertical = vertical;
        _direction = direction;
        _depl = depl;
        InitTiming();
    }

    public void ResetDepl(float depl)
    {
        _depl = depl;
        InitTiming();
    }

    public void Actualize(float deltaT, GameObject go)
    {
        float timingDepl = Math.Min(_timing, deltaT);
        float multDir = _direction == SwingAnimator.Direction.DirectionRight || _direction == SwingAnimator.Direction.DirectionUp ? 1 : -1; //-1 if Left or Down | 1 if Right or Up
        if (_vertical == SwingAnimator.Orientation.Vertical)
        {
            go.transform.Translate(0, multDir * timingDepl * SwingAnimator.VerticalSpeed * GameZone.SizeBall, 0);
        }
        else
        {
            go.transform.Translate(multDir * timingDepl * SwingAnimator.HorizontalSpeed * GameZone.SpacingBall, 0, 0);
        }

        //Decrement the timing
        _timing -= timingDepl;
    }

    public bool IsFinished()
    {
        return _timing == 0;
    }

    private void InitTiming()
    {
        if (_vertical == SwingAnimator.Orientation.Vertical) //VERTICAL
        {
            _timing = _depl / SwingAnimator.VerticalSpeed;
        }
        else //HORIZONTAL
        {
            _timing = _depl / SwingAnimator.HorizontalSpeed;
        }
    }
}