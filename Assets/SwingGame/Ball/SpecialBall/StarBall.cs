using UnityEngine;

public class StarBall : SpecialBall
{
    public StarBall() : base()
    {
        this.BallObject = GameObject.Instantiate(Resources.Load("Prefabs/PU_Star", typeof(GameObject))) as GameObject;
        this.type = PuType.StarType;
    }

    public override void Action(GameZone zone, int x, int y)
    {
        // If ball below
        if (GameZone.IsInPlaygroundBounds(new Vector2(x,y-1)) && zone.Playground[y - 1][x].HasBall())
        {
            if (zone.Playground[y - 1][x].Ball.GetType() == typeof(StarBall))
            {
                zone.Playground[y - 1][x].ExplodeBall(zone,Effect.EffectType.StarTransform);
                zone.Playground[y - 1][x].Ball = new GoldenStarBall();
                zone.Playground[y][x].ExplodeBall(zone,Effect.EffectType.BallSmoke);
            }
        }
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
                    if (!(zone.Playground[y][x + i].Ball.GetType() == typeof(StarBall)))
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
            
            zone.GameState.AddScoreForExplodingBalls(totalWeight);
        }
    }

    public override object Clone()
    {
        return new StarBall();
    }
}
