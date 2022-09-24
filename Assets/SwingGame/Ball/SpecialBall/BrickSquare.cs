using UnityEngine;

public class BrickSquare : SpecialBall
{
    public BrickSquare() : base()
    {
        this.BallObject = GameObject.Instantiate(Resources.Load("Prefabs/PU_BrickSquare", typeof(GameObject))) as GameObject;
        
        this.type = PuType.BrickSquareType;
    }

    public override void Action(GameZone zone, int x, int y)
    {
        // Collision detection max
        if (zone.IsPositionFree(new Vector2Int(x, y-1), false))
        {
            // If ball below
            if (zone.Playground[y - 1][x].HasBall())
            {
                // Replace every ball in 3x3 with bricks
                for (int i = -1; i <= 1; i++)
                {
                    if (x + i >= 0 && x + i < GameZone.LengthPlayGround)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            if (!(i == 0 && j == 0))
                            {
                                if (y + j >= 0 && y + j < GameZone.HeightPlayGround)
                                {
                                    if (zone.Playground[y + j][x + i].HasBall())
                                    {
                                        zone.Playground[y + j][x + i].ExplodeBall(zone,Effect.EffectType.BallTransform);
                                        zone.Playground[y + j][x + i].Ball = new BrickBall();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        zone.Playground[y][x].ExplodeBall(zone,Effect.EffectType.BallSmoke);
    }

    public override object Clone()
    {
        return new BrickSquare();
    }
}
