using UnityEngine;
using UnityEngine.Localization;

public class CopyLineBall : SpecialBall
{
    public CopyLineBall() : base()
    {
        this.BallObject = GameObject.Instantiate(Resources.Load("Prefabs/PU_CopyLine", typeof(GameObject))) as GameObject;

        LocalizedString header = new LocalizedString("PowerUp", "copyLine_h");
        LocalizedString content = new LocalizedString("PowerUp", "copyLine_c");
        this.setTooltip(header.GetLocalizedString(), content.GetLocalizedString());
    }

    public override void Action(GameZone zone, int x, int y)
    {
        // Collision detection max
        if (zone.IsPositionFree(new Vector2Int(x, y-1), false))
        {
            // If ball below
            if (zone.Playground[y - 1][x].HasBall())
            {
                // Get Ball and copy it until y=0
                Ball b = zone.Playground[y - 1][x].Ball;
                for (int i = y - 1; i >= 0 && zone.Playground[i][x].HasBall(); i--)
                {
                    zone.Playground[i][x].ExplodeBall(zone,Effect.EffectType.BallTransform);
                    zone.Playground[i][x].Ball = (Ball)b.Clone();
                }
            }
        }
        zone.Playground[y][x].ExplodeBall(zone,Effect.EffectType.BallSmoke);
    }

    public override object Clone()
    {
        return new CopyLineBall();
    }
}
