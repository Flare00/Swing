using UnityEngine;
using UnityEngine.Localization;

public class GoldenStarBall : SpecialBall
{
    public static int GoldenStarBonusMultScore = 4;
    public GoldenStarBall(bool tooltip = true) : base()
    {
        this.BallObject = GameObject.Instantiate(Resources.Load("Prefabs/PU_Star", typeof(GameObject))) as GameObject;
        this.BallObject.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.yellow * 7.0f);
        this.BallObject.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_BaseColor", Color.yellow);

        LocalizedString header = new LocalizedString("PowerUp", "starGold_h");
        LocalizedString content = new LocalizedString("PowerUp", "starGold_c");
        
        if (tooltip)
        {
            this.setTooltip(header.GetLocalizedString(), content.GetLocalizedString());
        }
        this.type = PuType.GoldenStarType;
    }

    public override void Action(GameZone zone, int x, int y)
    {
        // If align 3 stars
        bool align = true;
        for (int i = 1; i < 3 && align; i++)
        {
            if ((x + i) >= GameZone.LengthPlayGround)
            {
                align = false;
            }
            else
            {
                if (!zone.Playground[y][x + i].HasBall())
                {
                    align = false;
                }
                else
                {
                    if (!(zone.Playground[y][x + i].Ball.GetType() == typeof(GoldenStarBall)))
                    {
                        align = false;
                    }
                }
            }
        }
        if (align)
        {
            int totalWeight = 0;
            for (int i = 0; i < GameZone.LengthPlayGround; i++)
            {
                for (int j = 0; j < GameZone.HeightPlayGround; j++)
                {
                    if (zone.Playground[j][i].HasBall())
                    {
                        totalWeight += zone.Playground[j][i].Ball.Weight;
                        zone.Playground[j][i].ExplodeBall(zone,Effect.EffectType.BallAlign);
                    }
                }
            }
            zone.GameState.AddScoreForExplodingBalls(totalWeight * GoldenStarBonusMultScore);
        }
    }

    public override object Clone()
    {
        return new GoldenStarBall();
    }
}
