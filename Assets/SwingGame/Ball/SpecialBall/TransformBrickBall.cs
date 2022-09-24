using UnityEngine;

public class TransformBrickBall : SpecialBall
{
    public TransformBrickBall() : base()
    {
        this.BallObject = GameObject.Instantiate(Resources.Load("Prefabs/PU_BrickTransform", typeof(GameObject))) as GameObject;
        this.type = PuType.TransformBrickType;
    }

    public override void Action(GameZone zone, int x, int y)
    {
        // Collision detection max
        if (zone.IsPositionFree(new Vector2Int(x, y-1), false))
        {
            // If ball below
            if (zone.Playground[y - 1][x].HasBall())
            {
                bool isNormalBall = zone.Playground[y- 1][x].Ball.IsNormalBall;
                int idMaterialHit =zone.Playground[y - 1][x].Ball.IdMaterial;
                System.Type typeHit = zone.Playground[y- 1][x].Ball.GetType();

                for (int i = 0; i < GameZone.LengthPlayGround; i++)
                {
                    bool noUpperBall = false;
                    for (int j = 0; j < GameZone.HeightPlayGround && !noUpperBall; j++)
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
                                zone.Playground[j][i].ExplodeBall(zone,Effect.EffectType.BallTransform);
                                zone.Playground[j][i].Ball = new BrickBall();
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
        return new TransformBrickBall();
    }
}
