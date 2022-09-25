using UnityEngine;
using UnityEngine.Localization;

public class JokerBall : SpecialBall
{
    public JokerBall(bool tooltip = true) : base()
    {
        this.BallObject = GameObject.Instantiate(Resources.Load("Prefabs/PU_Joker", typeof(GameObject))) as GameObject;

        LocalizedString header = new LocalizedString("PowerUp", "joker_h");
        LocalizedString content = new LocalizedString("PowerUp", "joker_c");
        
        if (tooltip)
        {
            this.setTooltip(header.GetLocalizedString(), content.GetLocalizedString());
        }
        this.type = PuType.JokerType;
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
