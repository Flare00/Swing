using UnityEngine;

public class CopySquareBall : SpecialBall
{
    public CopySquareBall() : base()
    {
        this.BallObject = GameObject.Instantiate(Resources.Load("Prefabs/PU_CopySquare", typeof(GameObject))) as GameObject;
    }

    public override void Action(GameZone zone, int x, int y)
    {
        // Collision detection max
        if (zone.IsPositionFree(new Vector2Int(x, y-1), false))
        {
            // If ball below
            if (zone.Playground[y - 1][x].HasBall())
            {
                // Get Ball and copy it in 3x3
                Ball b = zone.Playground[y - 1][x].Ball;

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
                                        zone.Playground[y + j][x + i].Ball = (Ball)b.Clone();
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
        return new CopyPredictionBall();
    }
}
