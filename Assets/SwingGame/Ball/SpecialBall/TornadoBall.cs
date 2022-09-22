using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Localization;

public class TornadoBall : SpecialBall
{
    public TornadoBall() : base()
    {
        this.BallObject = GameObject.Instantiate(Resources.Load("Prefabs/PU_Tornado", typeof(GameObject))) as GameObject;

        LocalizedString header = new LocalizedString("PowerUp", "tornado_h");
        LocalizedString content = new LocalizedString("PowerUp", "tornado_c");
        this.setTooltip(header.GetLocalizedString(), content.GetLocalizedString());
    }

    public override void Action(GameZone zone, int x, int y)
    {
        // Add effect (ParticleSystem)
        // zone.AddEffect(new Effect(Effect.EffectType.ZapHorizontal, this.BallObject.transform.position)); 
        // Changer la position pour faire en sorte qu'elle soit a 0

        zone.SetDeactivateSwingAtCol(x, true);

        // Make everyball of the column fly
        bool hasBall = true;
        for (int i = y - 1; i > 0 && hasBall; i--)
        {
            if (zone.Playground[i][x].HasBall())
            {
                Ball b = zone.Playground[i][x].PopBall();
                zone.Animator.AddFlyingBall(b, GameZone.HeightPlayGround - i, Random.Range(8, 24),
                        new Vector2Int(x, i),
                        Random.value > 0.5 ? SwingAnimator.Direction.DirectionLeft : SwingAnimator.Direction.DirectionRight);
            }
            else
            {
                hasBall = false;
            }
        }

        zone.SetDeactivateSwingAtCol(x, false);

        // Remove PU
        zone.Playground[y][x].ExplodeBall(zone, Effect.EffectType.BallSmoke);
    }

    public override object Clone()
    {
        return new TornadoBall();
    }
}
