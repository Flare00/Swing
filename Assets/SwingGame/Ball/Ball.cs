using System;
using SwingGame.Media;
using UnityEngine;

public abstract class Ball : ICloneable
{
    protected int _weight;
    protected int _idMaterial;
    private bool _isNormalBall;
    protected PuType type;

    private GameObject _ballObject;

    public Ball()
    {
        this._weight = 0;
        this._idMaterial = -1;
    }

    public Ball(int weight, int idMaterial)
    {
        this._weight = weight;
        this._idMaterial = idMaterial;
    }

    ~Ball()
    {
        DestroyWithBallObject();
    }


    public void Explode(GameZone gz,Effect.EffectType effectType)
    {
        AudioManager audioManager = AudioManager.GetInstance();
        audioManager.PlayDestructionBallSound(_ballObject);
        gz.AddEffect(new Effect(effectType, this.BallObject.transform.position));
        DestroyWithBallObject();
    }

    public void DestroyWithBallObject()
    {
        if (this._ballObject != null)
        {
            ActionOnDestroy();
            GameObject.Destroy(this._ballObject);
            this._ballObject = null;
        }
    }

    public int Weight
    {
        get => _weight;
        set
        {
            _weight = value;
            GameObject text = this._ballObject.transform.GetChild(0).gameObject;
            if (_weight > 0)
                text.GetComponent<TMPro.TextMeshPro>().text = "" + _weight;
        }
    }
    public GameObject BallObject { get => _ballObject; set => _ballObject = value; }
    public int IdMaterial { get => _idMaterial; }
    public bool IsNormalBall { get => _isNormalBall; set => _isNormalBall = value; }
    public PuType Type{
        get=> type;
    }

    abstract public void SetHideBall(bool hide);
    abstract public void Action(GameZone zone, int x, int y);
    abstract public void ActionOnSwing(GameZone zone, int x, int y);
    abstract public void ActionOnDestroy();
    public abstract object Clone();
}
