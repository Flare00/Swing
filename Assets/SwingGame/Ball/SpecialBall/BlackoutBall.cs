using UnityEngine;
using UnityEngine.Localization;

public class BlackoutBall : SpecialBall
{
    private Animator _animator;
    private bool _animPlayed;
    private bool _firstPass;
    private GameZone _zone;
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

    public override void ActionOnDestroy()
    {
        if (_zone != null)
        {
            for (int i = 0; i < GameZone.LengthPlayGround; i++)
            {
                for (int j = 0; j < GameZone.HeightPlayGround; j++)
                {
                    if (_zone.Playground[j][i].HasBall())
                    {
                        if (_zone.Playground[j][i].Ball.BallObject != null)
                        {
                            _zone.Playground[j][i].Ball.SetHideBall(false);
                        }
                    }
                }
            }
        }
    }

    public override void Action(GameZone zone, int x, int y)
    {
        if(_zone == null)
        {
            _zone = zone;
        }
        // Play Animation
        if (!_animPlayed)
        {
            playAnimation();
        }
        
        // Blackout
        for (int i = 0; i < GameZone.LengthPlayGround; i++)
        {
            for (int j = 0; j < GameZone.HeightPlayGround; j++)
            {
                if (zone.Playground[j][i].HasBall())
                {
                    zone.Playground[j][i].Ball.SetHideBall(true);
                }
            }
        }

        // Check if animation played
        if (this._animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !_firstPass)
        {
            // Unhide balls
            for (int i = 0; i < GameZone.LengthPlayGround; i++)
            {
                for (int j = 0; j < GameZone.HeightPlayGround; j++)
                {
                    if (zone.Playground[j][i].HasBall())
                    {
                        if (zone.Playground[j][i].Ball.BallObject != null)
                        {
                            zone.Playground[j][i].Ball.SetHideBall(false);
                        }
                    }
                }
            }
            // Remove PU
            zone.Playground[y][x].ExplodeBall(zone, Effect.EffectType.BallSmoke);
        }
        _firstPass = false;
    }

    public void ActionOnExit(GameZone zone)
    {
        for(int i = 0; i < GameZone.LengthPlayGround; i++)
        {
            for(int j = 0; j < GameZone.HeightPlayGround; j++)
            {
                if (zone.Playground[j][i].HasBall())
                {
                    if (zone.Playground[j][i].Ball.BallObject != null)
                    {
                        zone.Playground[j][i].Ball.SetHideBall(false);
                    }
                }
            }
        }
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
