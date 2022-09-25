using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Localization;

public class ZapHorizontalBall : SpecialBall
{
    public ZapHorizontalBall(bool tooltip = true) : base()
    {
        this.BallObject = GameObject.Instantiate(Resources.Load("Prefabs/PU_ZapH", typeof(GameObject))) as GameObject;

        LocalizedString header = new LocalizedString("PowerUp", "zapH_h");
        LocalizedString content = new LocalizedString("PowerUp", "zapH_c");
        if (tooltip)
        {
            this.setTooltip(header.GetLocalizedString(), content.GetLocalizedString());
        }
        this.type = PuType.ZapHorizontalType;
    }

    public override void Action(GameZone zone, int x, int y)
    {
        // Add effect (ParticleSystem)
        zone.AddEffect(new Effect(Effect.EffectType.ZapHorizontal, this.BallObject.transform.position));

        // Destroy every ball horizontally and add index hit
        List<int> tabIndexHit = new List<int>();
        int totalWeight = 0;
        for (int i = 0; i < GameZone.LengthPlayGround; i++)
        {
            if (zone.Playground[y][i].HasBall() && i != x)
            {
                // If it's a bomb
                if (zone.Playground[y][i].Ball.GetType() == typeof(BombBall))
                {
                    ((BombBall)zone.Playground[y][i].Ball).BombExplode(zone, i, y);
                }
                else
                {
                    totalWeight += zone.Playground[y][i].Ball.Weight;
                    zone.Playground[y][i].ExplodeBall(zone,Effect.EffectType.BallExplosion);
                    tabIndexHit.Add(i);
                }
            }
        }
        zone.GameState.AddScoreForExplodingBalls(totalWeight);
        
        // For each hit, check animator 
        for (int i = 0; i < tabIndexHit.Count; i++)
        {
            bool noUpperBall = false;
            for (int j = y + 1; j < GameZone.HeightPlayGround && !noUpperBall; j++)
            {
                if (zone.Playground[j][(int)tabIndexHit[i]].HasBall())
                {
                    zone.Animator.AddDropingBall(zone.Playground[j][(int)tabIndexHit[i]].PopBall(), new Vector2Int((int)tabIndexHit[i], j));
                }
                else
                {
                    noUpperBall = true;
                }
            }
        }
        // Remove PU
        zone.Playground[y][x].ExplodeBall(zone,Effect.EffectType.BallSmoke);
    }

    public override object Clone()
    {
        return new ZapHorizontalBall();
    }
}
