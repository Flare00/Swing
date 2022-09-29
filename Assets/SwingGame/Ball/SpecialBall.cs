using UnityEngine;

public abstract class SpecialBall : Ball
{

    public SpecialBall() : base()
    {
        this.IsNormalBall = false;
    }

    protected void setTooltip(string header, string content)
    {
        TooltipTrigger tt = this.BallObject.AddComponent<TooltipTrigger>();
        this.BallObject.AddComponent<SphereCollider>();

        tt.header = header;
        tt.content = content;
    }
    public override void SetHideBall(bool hide)
    {
    }
    public override void ActionOnSwing(GameZone zone, int x, int y)
    {
    }
}