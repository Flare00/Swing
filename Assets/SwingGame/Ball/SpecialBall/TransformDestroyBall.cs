using System.Collections.Generic;
using UnityEngine;

public class TransformDestroyBall : SpecialBall
{
    public TransformDestroyBall() : base()
    {
        this.BallObject = GameObject.Instantiate(Resources.Load("Prefabs/PU_DestroyTransform", typeof(GameObject))) as GameObject;
    }

    public override void Action(GameZone zone, int x, int y)
    {
        zone.Playground[y][x].ExplodeBall(zone,Effect.EffectType.BallSmoke);
        if (y > 0)
        {
            // If ball below
            if (zone.Playground[y - 1][x].HasBall())
            {
                // Get Ball material and seek every similar
                int[] tabIndexHit = new int[GameZone.LengthPlayGround];
                bool isNormalBall = zone.Playground[y- 1][x].Ball.IsNormalBall;
                int idMaterialHit =zone.Playground[y - 1][x].Ball.IdMaterial;
                System.Type typeHit = zone.Playground[y- 1][x].Ball.GetType();

                int totalWeight = 0;
                for (int i = 0; i < GameZone.LengthPlayGround; i++)
                {
                    tabIndexHit[i] = -1;

                    for (int j = GameZone.HeightPlayGround - 1; j >= 0; j--)
                    {
                        if (zone.Playground[j][i].HasBall())
                        {
                            
                            bool same = isNormalBall == zone.Playground[j][i].Ball.IsNormalBall;
                            if (same)
                            {
                                if (isNormalBall)
                                {
                                    same = zone.Playground[j][i].Ball.IdMaterial == idMaterialHit;
                                }
                                else
                                {
                                    same = typeHit == zone.Playground[j][i].Ball.GetType();
                                }
                            }
                            if (same)
                            {
                                // LA BALL VIENT D'EXPLOSER
                                totalWeight += zone.Playground[j][i].Ball.Weight;
                                zone.Playground[j][i].ExplodeBall(zone,Effect.EffectType.BallExplosion);
                                tabIndexHit[i] = j;
                            }
                        }
                    }
                }

                // SCORE
                zone.GameState.AddScoreForExplodingBalls(totalWeight);

                // For each hit, check animator 
                for (int i = 0; i < GameZone.LengthPlayGround; i++)
                {
                    if (tabIndexHit[i] >= 0)
                    {
                        for (int j = tabIndexHit[i]; j < GameZone.HeightPlayGround; j++)
                        {
                            if (zone.Playground[j][i].HasBall())
                            {
                                zone.Animator.AddDropingBall(zone.Playground[j][i].PopBall(), new Vector2Int(i, j));
                            }
                        }
                    }
                }
            }
        }
    }

    public override object Clone()
    {
        return new TransformDestroyBall();
    }
}
