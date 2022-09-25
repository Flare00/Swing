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
}