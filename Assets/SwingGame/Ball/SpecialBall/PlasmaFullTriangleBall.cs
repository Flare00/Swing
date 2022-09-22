using UnityEngine;
using UnityEngine.Localization;

public class PlasmaFullTriangleBall : SpecialBall
{
    public PlasmaFullTriangleBall(bool tooltip = true) : base()
    {
        this.BallObject = GameObject.Instantiate(Resources.Load("Prefabs/PU_PlasmaFullTriangle", typeof(GameObject))) as GameObject;

        LocalizedString header = new LocalizedString("PowerUp", "flashT_h");
        LocalizedString content = new LocalizedString("PowerUp", "flashT_c");
        
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
                    for(int i = x - depth; i <= x + depth; i++)
                    {
                        if (i < GameZone.LengthPlayGround && i >= 0)
                        {
                            if (zone.Playground[j][i].HasBall())
                            {
                                zone.Playground[j][i].ExplodeBall(zone,Effect.EffectType.BallTransform);
                                zone.Playground[j][i].Ball = (Ball)b.Clone();
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
        return new PlasmaFullTriangleBall();
    }
}
