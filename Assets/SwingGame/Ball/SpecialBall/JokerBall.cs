using UnityEngine;
using UnityEngine.Localization;

public class JokerBall : SpecialBall
{
    public JokerBall() : base()
    {
        this.BallObject = GameObject.Instantiate(Resources.Load("Prefabs/PU_Joker", typeof(GameObject))) as GameObject;

        LocalizedString header = new LocalizedString("PowerUp", "joker_h");
        LocalizedString content = new LocalizedString("PowerUp", "joker_c");
        this.setTooltip(header.GetLocalizedString(), content.GetLocalizedString());
    }

    public override void Action(GameZone zone, int x, int y)
    {
        // Nothing to do
    }

    public override object Clone()
    {
        return new JokerBall();
    }
}
