using UnityEngine;
using System.Collections.Generic;

public class ZapDiagonalBall : SpecialBall
{
    public ZapDiagonalBall() : base()
    {
        this.BallObject = GameObject.Instantiate(Resources.Load("Prefabs/PU_ZapDiag", typeof(GameObject))) as GameObject;
    }

    public override void Action(GameZone zone, int x, int y)
    {

        // Add effect (ParticleSystem)
        zone.AddEffect(new Effect(Effect.EffectType.ZapDiagonal, this.BallObject.transform.position));
        
        List<int> tabIndexHit = new List<int>();

        bool edit = true;

        for (int i = 1; i < GameZone.LengthPlayGround && edit; i++)
        {
            edit = false;

            int xDiag = x + i;
            int yDiag = y + i;

            int leftSave = -1;
            int rightSave = -1;

            if (destroyDiag(zone, xDiag, yDiag))
            {
                rightSave = yDiag + 1;
            }

            yDiag = y - i;

            if (destroyDiag(zone, xDiag, yDiag))
            {
                rightSave = yDiag + 1;
            }


            xDiag = x - i;
            yDiag = y + i;

            if (destroyDiag(zone, xDiag, yDiag))
            {
                leftSave = yDiag + 1;
            }


            yDiag = y - i;
            if (destroyDiag(zone, xDiag, yDiag))
            {
                leftSave = yDiag + 1;
            }

            if (rightSave >= 0 && x + i >= 0 && x + i < GameZone.LengthPlayGround)
            {
                edit = true;
                for (int j = rightSave; j < GameZone.HeightPlayGround; j++)
                {
                    if (x + i >= 0 && x + i < GameZone.LengthPlayGround)
                    {
                        if (zone.Playground[j][x + i].HasBall())
                        {
                            if (zone.IsPositionFree(new Vector2Int(x + i, j - 1)))
                            {
                                zone.Animator.AddDropingBall(zone.Playground[j][x + i].PopBall(), new Vector2Int(x + i, j));
                            }
                        }
                    }
                }
            }
            if (leftSave >= 0 && x - i >= 0 && x - i < GameZone.LengthPlayGround)
            {
                edit = true;
                for (int j = leftSave; j < GameZone.HeightPlayGround; j++)
                {
                    if (zone.Playground[j][x - i].HasBall())
                    {
                        if (zone.IsPositionFree(new Vector2Int(x - i, j - 1)))
                        {
                            zone.Animator.AddDropingBall(zone.Playground[j][x - i].PopBall(), new Vector2Int(x - i, j));
                        }
                    }
                }
            }
        }
        // Remove PU
        zone.Playground[y][x].ExplodeBall(zone,Effect.EffectType.BallSmoke);
    }

    private bool destroyDiag(GameZone zone, int x, int y)
    {
        
        if (x >= 0 && x < GameZone.LengthPlayGround && y >= 0 && y < GameZone.HeightPlayGround)
        {
            if (zone.Playground[y][x].HasBall())
            {
                // If it's a bomb
                if (zone.Playground[y][x].Ball.GetType() == typeof(BombBall))
                {
                    ((BombBall)zone.Playground[y][x].Ball).BombExplode(zone, x, y);
                }
                else
                {
                    zone.GameState.AddScoreForExplodingBalls(zone.Playground[y][x].Ball.Weight);
                    zone.Playground[y][x].ExplodeBall(zone,Effect.EffectType.BallExplosion);
                }
            }
            return true;
        }
        return false;
    }


    public override object Clone()
    {
        return new ZapDiagonalBall();
    }
}
