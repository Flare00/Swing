using UnityEngine;
using UnityEngine.Localization;

public class CutterBall : SpecialBall
{
    public CutterBall(bool tooltip = true) : base()
    {
        this.BallObject = GameObject.Instantiate(Resources.Load("Prefabs/PU_Cutter", typeof(GameObject))) as GameObject;

        LocalizedString header = new LocalizedString("PowerUp", "cutter_h");
        LocalizedString content = new LocalizedString("PowerUp", "cutter_c");
        
        if (tooltip)
        {
            this.setTooltip(header.GetLocalizedString(), content.GetLocalizedString());
        }
    }

    public override void Action(GameZone zone, int x, int y)
    {
        zone.SetDeactivateSwingAtCol(x, true);

        // Collision detection max
        if (zone.IsPositionFree(new Vector2Int(x, y-1), false))
        {
            // If ball below
            if (zone.Playground[y - 1][x].HasBall())
            {
                zone.Playground[y - 1][x].ExplodeBall(zone,Effect.EffectType.BallExplosion);
                zone.Animator.AddDropingBall(zone.Playground[y][x].PopBall(), new Vector2Int(x, y));
            }
        }
        else
        {
            zone.SetDeactivateSwingAtCol(x, false);
        }
        zone.Playground[y][x].ExplodeBall(zone,Effect.EffectType.BallSmoke);
        
    }

    public override object Clone()
    {
        return new CutterBall();
    }
}
