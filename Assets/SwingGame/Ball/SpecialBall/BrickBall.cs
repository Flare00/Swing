using UnityEngine;
using UnityEngine.Localization;

public class BrickBall : SpecialBall
{
    public BrickBall(bool tooltip = true) : base()
    {
        this.BallObject = GameObject.Instantiate(Resources.Load("Prefabs/PU_BrickBall", typeof(GameObject))) as GameObject;

        LocalizedString header = new LocalizedString("PowerUp", "brick_h");
        LocalizedString content = new LocalizedString("PowerUp", "brick_c"); 
        if (tooltip)
        {
            this.setTooltip(header.GetLocalizedString(), content.GetLocalizedString());
        }
    }

    public override void Action(GameZone zone, int x, int y) { /* No Action By Default */ }

    public override object Clone()
    {
        return new BrickBall();
    }
}
