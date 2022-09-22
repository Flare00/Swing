using UnityEngine;
using UnityEngine.Localization;

public class PlasmaNoTriangleBall : SpecialBall
{
    public PlasmaNoTriangleBall(bool tooltip = true) : base()
    {
        this.BallObject = GameObject.Instantiate(Resources.Load("Prefabs/PU_PlasmaNoTriangle", typeof(GameObject))) as GameObject;

        LocalizedString header = new LocalizedString("PowerUp", "flashD_h");
        LocalizedString content = new LocalizedString("PowerUp", "flashD_c");
        
        if (tooltip)
        {
            this.setTooltip(header.GetLocalizedString(), content.GetLocalizedString());
        }
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
                    for (int i = 0; i < 2; i++)
                    {
                        int r = Random.Range(x - depth, x + depth + 1);

                        if (r < GameZone.LengthPlayGround && r >= 0)
                        {
                            if (zone.Playground[j][r].HasBall())
                            {
                                zone.Playground[j][r].ExplodeBall(zone,Effect.EffectType.BallTransform);
                                zone.Playground[j][r].Ball = (Ball)b.Clone();
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
        return new PlasmaNoTriangleBall();
    }
}
