using UnityEngine;

public class PlasmaEmptyTriangleBall : SpecialBall
{
    public PlasmaEmptyTriangleBall() : base()
    {
        this.BallObject = GameObject.Instantiate(Resources.Load("Prefabs/PU_PlasmaDemiTriangle", typeof(GameObject))) as GameObject;
        this.type = PuType.PlasmaEmptyTriangleType;
    }

    public override void Action(GameZone zone, int x, int y)
    {
        // Collision detection max
        if (zone.IsPositionFree(new Vector2Int(x, y-1), false))
        {
            // If ball below
            if (zone.Playground[y - 1][x].HasBall())
            {
                // Get Ball and copy it
                Ball b = zone.Playground[y - 1][x].Ball;

                for (int j = y - 2, depth = 1; j >= 0; j--, depth++)
                {
                    // Right Side
                    if (x + depth < GameZone.LengthPlayGround)
                    {
                        if (zone.Playground[j][x + depth].HasBall())
                        {
                            zone.Playground[j][x + depth].ExplodeBall(zone,Effect.EffectType.BallTransform);
                            zone.Playground[j][x + depth].Ball = (Ball)b.Clone();
                        }
                    }

                    // Left Side
                    if (x - depth >= 0)
                    {
                        if (zone.Playground[j][x - depth].HasBall())
                        {
                            zone.Playground[j][x - depth].ExplodeBall(zone,Effect.EffectType.BallTransform);
                            zone.Playground[j][x - depth].Ball = (Ball)b.Clone();
                        }
                    }
                }
            }
        }
        zone.Playground[y][x].ExplodeBall(zone,Effect.EffectType.BallSmoke);
    }

    public override object Clone()
    {
        return new PlasmaEmptyTriangleBall();
    }
}
