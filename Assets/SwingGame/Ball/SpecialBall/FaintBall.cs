using UnityEngine;
using UnityEngine.Localization;

public class FaintBall : SpecialBall
{
    public FaintBall(bool tooltip = true) : base()
    {
        this.BallObject = GameObject.Instantiate(Resources.Load("Prefabs/PU_Faint", typeof(GameObject))) as GameObject;
        this.type = PuType.FaintType;

        LocalizedString header = new LocalizedString("PowerUp", "faint_h");
        LocalizedString content = new LocalizedString("PowerUp", "faint_c");
        
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
        return new FaintBall();
    }
}
