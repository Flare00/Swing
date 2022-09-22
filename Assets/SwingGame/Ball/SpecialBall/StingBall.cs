using UnityEngine;
using UnityEngine.Localization;

public class StingBall : SpecialBall
{
    private Animator _animator;
    private bool _animatorNoRepeat;

    public StingBall(bool tooltip = true) : base()
    {
        this.BallObject = GameObject.Instantiate(Resources.Load("Prefabs/PU_Sting", typeof(GameObject))) as GameObject;

        _animator = BallObject.GetComponentInChildren<Animator>();
        _animatorNoRepeat = false;

        LocalizedString header = new LocalizedString("PowerUp", "sting_h");
        LocalizedString content = new LocalizedString("PowerUp", "sting_c");
        
        if (tooltip)
        {
            this.setTooltip(header.GetLocalizedString(), content.GetLocalizedString());
        }
    }

    public override void Action(GameZone zone, int x, int y)
    {
        // Animation
        ChangeAnimator();
        if (!(this._animator.GetCurrentAnimatorStateInfo(1).normalizedTime < 2 && this._animator.GetCurrentAnimatorStateInfo(1).IsName("Anim_StingSpike")))
        {
            _animator.Play("Anim_StingSpike", 1, 0f);

            bool exploded = false;

            int boundY = -1;
            if (y + 1 >= 0 && y + 1 < GameZone.HeightPlayGround)
            {
                if (zone.Playground[y + 1][x].HasBall())
                {
                    zone.Playground[y + 1][x].ExplodeBall(zone,Effect.EffectType.BallExplosion);
                    boundY = y + 1;
                    exploded = true;
                }
            }
            if (y - 1 >= 0 && y - 1 < GameZone.HeightPlayGround)
            {
                if (zone.Playground[y - 1][x].HasBall())
                {
                    zone.Playground[y - 1][x].ExplodeBall(zone,Effect.EffectType.BallExplosion);
                    boundY = y;
                    exploded = true;
                }
            }
            if (exploded)
            {
                for (int i = boundY; i < GameZone.HeightPlayGround; i++)
                {
                    if (zone.Playground[i][x].HasBall())
                    {
                        zone.Animator.AddDropingBall(zone.Playground[i][x].PopBall(), new Vector2Int(x, i));
                    }
                }
            }
        }
    }

    private void ChangeAnimator()
    {
        if (!_animatorNoRepeat)
        {
            _animatorNoRepeat = true;
            this._animator.runtimeAnimatorController = Resources.Load("Animations/PU_StingAnimatorNoRepeat", typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        }
    }
    public override object Clone()
    {
        return new StingBall();
    }

}
