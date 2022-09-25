using UnityEngine;
using UnityEngine.Localization;

public class BlackoutBall : SpecialBall
{
    public BlackoutBall(bool tooltip = true) : base()
    {
        this.BallObject = GameObject.Instantiate(Resources.Load("Prefabs/PU_Blackout", typeof(GameObject))) as GameObject;

        LocalizedString header = new LocalizedString("PowerUp", "blackout_h");
        LocalizedString content = new LocalizedString("PowerUp", "blackout_c");
        
        if (tooltip)
        {
            this.setTooltip(header.GetLocalizedString(), content.GetLocalizedString());
        }
    }

    public override void Action(GameZone zone, int x, int y)
    {
    }

    public override object Clone()
    {
        return new BlackoutBall();
    }
}
