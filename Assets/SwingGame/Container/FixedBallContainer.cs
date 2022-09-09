using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedBallContainer : BallContainer
{
    private Vector3 _position;

    public FixedBallContainer(Vector3 pos) : base(null)
    {
        _position = pos;
        ContainerObject.transform.position = _position;
    }
    public FixedBallContainer(Vector3 pos, Ball b) : base(b)
    {
        _position = pos;
        ContainerObject.transform.position = _position;

    }

    public Vector3 Position { get => _position; set { _position = value; base.ContainerObject.transform.localPosition = _position; } }
}
