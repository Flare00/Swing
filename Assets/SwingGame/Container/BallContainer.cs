using System.Collections;
using System.Collections.Generic;
using SwingGame.Media;
using UnityEngine;

public class BallContainer
{
    private Ball _ball;
    private GameObject _containerObject;

    public BallContainer(Ball b)
    {
        _containerObject = new GameObject("BallContainer");
        _ball = b;
        if(_ball != null) 
            _ball.BallObject.transform.parent = _containerObject.transform;
    }

    public BallContainer()
    {
        _containerObject = new GameObject("BallContainer");
        _ball = null;
    }

    public Ball Ball { 
        get => _ball; 
        set { 
            _ball = value;
            if (_ball != null)
            {
                _ball.BallObject.transform.parent = _containerObject.transform;
                _ball.BallObject.transform.localPosition = Vector3.zero;
            }
        } 
    }
    public GameObject ContainerObject { get => _containerObject; set => _containerObject = value; }
    public bool HasBall()
    {
        return _ball != null;
    }
    
    
    public void ExplodeBall(GameZone gz,Effect.EffectType effectType = Effect.EffectType.NoEffect)
    {
        if (_ball != null)
        {
            _ball.Explode(gz,effectType);
            _ball = null;
        }
    }
    
    public void RemoveBall()
    {
        if (_ball != null)
        {
            _ball.DestroyWithBallObject();
            _ball = null;
        }
    }

    public Ball PopBall()
    {
        Ball b = _ball;
        _ball = null;
        return b;
    }
}
