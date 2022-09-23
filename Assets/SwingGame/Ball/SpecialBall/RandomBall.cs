using UnityEngine;
using UnityEngine.Localization;

public class RandomBall : SpecialBall
{
    public RandomBall(bool tooltip = true) : base()
    {
        this.BallObject = GameObject.Instantiate(Resources.Load("Prefabs/PU_Random", typeof(GameObject))) as GameObject;

        LocalizedString header = new LocalizedString("PowerUp", "random_h");
        LocalizedString content = new LocalizedString("PowerUp", "random_c");

        if (tooltip)
        {
            this.setTooltip(header.GetLocalizedString(), content.GetLocalizedString());
        }
    }

    public override void Action(GameZone zone, int x, int y)
    {
        // Nothing to do
    }

    public override object Clone()
    {
        return new RandomBall();
    }
}
