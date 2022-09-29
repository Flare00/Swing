using System;
using System.Collections.Generic;
using UnityEngine;

public class Effect
{
    private GameObject _go;
    private List<ParticleSystem> _ps;

    public enum EffectType
    {
        NoEffect,
        BombExplosion,
        BallExplosion,
        BallAlign,
        ZapHorizontal,
        ZapDiagonal,
        BallTransform,
        BallSmoke,
        StarTransform,
        TornadoEffect
    }

    public Effect(EffectType type, Vector3 v)
    {
        _ps = new List<ParticleSystem>();
        switch (type)
        {
            case EffectType.NoEffect:
                break;
            case EffectType.BombExplosion:
                _go = GameObject.Instantiate(Resources.Load("Particles/PS_BombExplosion", typeof(GameObject))) as GameObject;
                _ps.Add(_go.transform.Find("Particle").GetComponent<ParticleSystem>());
                break;
            case EffectType.BallExplosion:
                _go = GameObject.Instantiate(Resources.Load("Particles/PS_BallExplosion", typeof(GameObject))) as GameObject;
                _ps.Add(_go.transform.Find("Particle").GetComponent<ParticleSystem>());
                break;
            case EffectType.BallTransform:
                _go = GameObject.Instantiate(Resources.Load("Particles/PS_BallTransform", typeof(GameObject))) as GameObject;
                _ps.Add(_go.transform.Find("Particle").GetComponent<ParticleSystem>());
                break;
            case EffectType.BallAlign:
                _go = GameObject.Instantiate(Resources.Load("Particles/PS_BallAlign", typeof(GameObject))) as GameObject;
                _ps.Add(_go.transform.Find("Particle").GetComponent<ParticleSystem>());
                break;
            case EffectType.BallSmoke:
                _go = GameObject.Instantiate(Resources.Load("Particles/PS_BallSmoke", typeof(GameObject))) as GameObject;
                _ps.Add(_go.transform.Find("Particle").GetComponent<ParticleSystem>());
                break;
            case EffectType.ZapHorizontal:
                _go = GameObject.Instantiate(Resources.Load("Particles/PS_ZapHEffect", typeof(GameObject))) as GameObject;
                _ps.Add(_go.transform.Find("Particle").GetComponent<ParticleSystem>());
                break;
            case EffectType.StarTransform:
                _go = GameObject.Instantiate(Resources.Load("Particles/PS_Star", typeof(GameObject))) as GameObject;
                _ps.Add(_go.transform.Find("Particle").GetComponent<ParticleSystem>());
                break;
            case EffectType.ZapDiagonal:
                _go = GameObject.Instantiate(Resources.Load("Particles/PS_ZapDiagEffect", typeof(GameObject))) as GameObject;
                _ps.Add(_go.transform.Find("Particle1").GetComponent<ParticleSystem>());
                _ps.Add(_go.transform.Find("Particle2").GetComponent<ParticleSystem>());
                break;
            case EffectType.TornadoEffect:
                _go = GameObject.Instantiate(Resources.Load("Particles/PS_TornadoEffect", typeof(GameObject))) as GameObject;
                _ps.Add(_go.transform.Find("Particle1").GetComponent<ParticleSystem>());
                _ps.Add(_go.transform.Find("Particle2").GetComponent<ParticleSystem>());
                break;

        }
        if (_go != null)
        {
            _go.transform.position = v;
            for (int i = 0; i < _ps.Count; i++)
            {
                _ps[i].Play();
            }
        }
    }

    public bool IsEnd()
    {
        bool end = true;
        
        for (int i = 0; i < _ps.Count && end; i++)
        {
            if (_ps[i] != null && _ps[i].IsAlive())
            {
                end = false;
            }
        }

        if (end)
        {
            _ps.Clear();
            GameObject.Destroy(_go);
        }
        
        return end;
    }

}
