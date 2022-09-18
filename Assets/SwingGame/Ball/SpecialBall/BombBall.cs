using UnityEngine;
using UnityEngine.Localization;

public class BombBall : SpecialBall
{
    private bool _exploded;
    public BombBall() : base()
    {
        this.BallObject = GameObject.Instantiate(Resources.Load("Prefabs/PU_Bomb", typeof(GameObject))) as GameObject;

        LocalizedString header = new LocalizedString("PowerUp", "bomb_h");
        LocalizedString content = new LocalizedString("PowerUp", "bomb_c");
        this.setTooltip(header.GetLocalizedString(), content.GetLocalizedString());

        _exploded = false;
    }


    public override void Action(GameZone zone, int x, int y)
    {
        if (!_exploded)
        {
            // Check collision under
            if (zone.IsPositionFree(new Vector2Int(x, y-1), false))
            {
                // test si y - 1 hasBall()
                if (zone.Playground[y - 1][x].HasBall())
                {
                    _exploded = true;
                }
            }
            // Check collision upper
            else if (!_exploded && y < GameZone.HeightPlayGround)
            {
                if (zone.Playground[y + 1][x].HasBall())
                {
                    _exploded = true;
                }
            }
            // Explosion in 3x3 from position (x,y)
            if (_exploded)
            {
                BombExplode(zone, x, y);
            }
        }
    }

    public void BombExplode(GameZone zone, int x, int y)
    {
        int totalWeight = 0;
        
        // Add effect (ParticleSystem)
        zone.AddEffect(new Effect(Effect.EffectType.BombExplosion, this.BallObject.transform.position));

        zone.Playground[y][x].Ball = null;

        // Explode every ball except center (bomb)
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
                            // If it's a bomb
                            if (zone.Playground[y + j][x + i].HasBall())
                            {
                                if (zone.Playground[y + j][x + i].Ball.GetType() == typeof(BombBall))
                                {
                                    ((BombBall)zone.Playground[y + j][x + i].Ball).BombExplode(zone, x + i, y + j);
                                }
                                else
                                {
                                    totalWeight += zone.Playground[y + j][x + i].Ball.Weight;
                                    zone.Playground[y + j][x + i].ExplodeBall(zone,Effect.EffectType.BallExplosion);
                                }
                            }
                        }
                    }
                }
            }
        }
        zone.GameState.AddScoreForExplodingBalls(totalWeight);
        
        // Call animator for each balls higher than 3x3
        for (int i = -1; i <= 1; i++)
        {
            if (x + i >= 0 && x + i < GameZone.LengthPlayGround)
            {
                for (int j = y + 2; j < GameZone.HeightPlayGround; j++)
                {
                    if (zone.Playground[j][x + i].HasBall())
                    {
                        zone.Animator.AddDropingBall(zone.Playground[j][x + i].PopBall(), new Vector2Int(x + i, j));
                    }
                }
            }
        }

        // Destroy bomb
        this.DestroyWithBallObject();
    }
    public override object Clone()
    {
        return new BombBall();
    }
}
