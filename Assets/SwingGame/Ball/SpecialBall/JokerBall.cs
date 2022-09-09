using UnityEngine;

public class JokerBall : SpecialBall
{
    public JokerBall() : base()
    {
        this.BallObject = GameObject.Instantiate(Resources.Load("Prefabs/PU_Joker", typeof(GameObject))) as GameObject;
    }

    public override void Action(GameZone zone, int x, int y)
    {
        // Nothing to do
    }

    public override object Clone()
    {
        return new JokerBall();
    }
}
