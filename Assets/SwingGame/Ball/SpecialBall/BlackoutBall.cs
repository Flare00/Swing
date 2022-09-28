using UnityEngine;
using UnityEngine.Localization;

public class BlackoutBall : SpecialBall
{
    private Animator _animator;
    private bool _animPlayed;
    private bool _firstPass;

    public BlackoutBall(bool tooltip = true) : base()
    {
        this.BallObject = GameObject.Instantiate(Resources.Load("Prefabs/PU_Blackout", typeof(GameObject))) as GameObject;
        this.type = PuType.RandomType;

        _animator = this.BallObject.GetComponentInChildren<Animator>();
        _animPlayed = false;
        _firstPass = true;

        LocalizedString header = new LocalizedString("PowerUp", "blackout_h");
        LocalizedString content = new LocalizedString("PowerUp", "blackout_c");

        if (tooltip)
        {
            this.setTooltip(header.GetLocalizedString(), content.GetLocalizedString());
        }
    }

    public override void Action(GameZone zone, int x, int y)
    {
        if (!_animPlayed)
        {
            playAnimation();
        }
        


        if (this._animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !_firstPass)
        {
            // Remove PU
            zone.Playground[y][x].ExplodeBall(zone, Effect.EffectType.BallSmoke);
        }
        _firstPass = false;
    }

    public void playAnimation()
    {
        this._animator.runtimeAnimatorController = Resources.Load("Animations/PU_BlackoutNoRepeat", typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        _animator.Play("Anim_Blackout", 0, 0f);
        _animPlayed = true;
    }

    public override object Clone()
    {
        return new BlackoutBall();
    }
}
