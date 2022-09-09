using UnityEngine;

public class BrickBall : SpecialBall
{
    public BrickBall() : base()
    {
        this.BallObject = GameObject.Instantiate(Resources.Load("Prefabs/PU_BrickBall", typeof(GameObject))) as GameObject;
    }

    public override void Action(GameZone zone, int x, int y) { /* No Action By Default */ }

    public override object Clone()
    {
        return new BrickBall();
    }
}
