using UnityEngine;
using UnityEngine.Localization;

public class CopyPredictionBall : SpecialBall
{
    public CopyPredictionBall() : base()
    {
        this.BallObject = GameObject.Instantiate(Resources.Load("Prefabs/PU_CopyPrediction", typeof(GameObject))) as GameObject;

        LocalizedString header = new LocalizedString("PowerUp", "copyPrediction_h");
        LocalizedString content = new LocalizedString("PowerUp", "copyPrediction_c");
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
                // Get Ball and copy it on first row prediction
                Ball b = zone.Playground[y - 1][x].Ball;
                for (int i = 0; i < GameZone.LengthPlayGround; i++)
                {
                    zone.Prediction[0][i].ExplodeBall(zone,Effect.EffectType.BallTransform);
                    zone.Prediction[0][i].Ball = (Ball)b.Clone();
                }
            }
        }
        zone.Playground[y][x].ExplodeBall(zone,Effect.EffectType.BallSmoke);
    }
    public override object Clone()
    {
        return new CopyPredictionBall();
    }
}
