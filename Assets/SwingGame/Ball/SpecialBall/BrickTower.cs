using System.Collections.Generic;
using UnityEngine;

public class BrickTower : SpecialBall
{
    public BrickTower() : base()
    {
        this.BallObject = GameObject.Instantiate(Resources.Load("Prefabs/PU_Tower", typeof(GameObject))) as GameObject;
        this.type = PuType.BrickTowerType;
    }

    public override void Action(GameZone zone, int x, int y)
    {
        zone.Playground[y][x].ExplodeBall(zone,Effect.EffectType.BallSmoke);

        List<Ball> floatingBalls = zone.Animator.GetBallsBetweenPosition(x,y,GameZone.RealHeightPlayGround-1,true);
        
        for (int i = 0; i < floatingBalls.Count; i++)
        {
            floatingBalls[i].Explode(zone,Effect.EffectType.BallExplosion);
        }
        for (int i = y; i < GameZone.RealHeightPlayGround; i++)
        {
            zone.Playground[i][x].Ball = new BrickBall();
        }
    }

    public override object Clone()
    {
        return new BrickTower();
    }
}
