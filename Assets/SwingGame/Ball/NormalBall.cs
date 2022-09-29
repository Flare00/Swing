using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBall : Ball
{
    private bool _isHide;
    public NormalBall(int weight, int idMaterial = 0) : base(weight, idMaterial)
    {
        this.type = PuType.NormalType;
        this.IsNormalBall = true;
        this.BallObject = GameObject.Instantiate(Resources.Load("Prefabs/Sphere", typeof(GameObject))) as GameObject;

        AudioSource audioSource = this.BallObject.AddComponent<AudioSource>();
        audioSource.clip = Resources.Load("../") as AudioClip;
        audioSource.Play();

        SelectWeight(weight);
        SelectMaterial(idMaterial);
        _isHide = false;
    }

    public override void Action(GameZone zone, int x, int y)
    {
    }
    public override void ActionOnSwing(GameZone zone, int x, int y)
    {
    }
    public override void ActionOnDestroy()
    {
    }



    public override object Clone()
    {
        return new NormalBall(this.Weight, this.IdMaterial);
    }

    private void SelectWeight(int weight)
    {
        GameObject text = this.BallObject.transform.GetChild(0).gameObject;
        if (weight > 0)
        {
            text.GetComponent<TMPro.TextMeshPro>().text = "" + weight;
        }
        else
        {
            text.GetComponent<TMPro.TextMeshPro>().text = "";
        }
    }

    private void SelectMaterial(int idMaterial)
    {
        Renderer r = this.BallObject.GetComponent<Renderer>();

        int select = idMaterial;

        if (idMaterial < 10)
        {
            r.material = Resources.Load("Material/MaterialBaseColor/MaterialBall", typeof(Material)) as Material;
        }
        else if (idMaterial < 20)
        {
            r.material = Resources.Load("Material/MaterialBaseColor/MaterialStrip", typeof(Material)) as Material;
            select -= 10;
        }
        else if (idMaterial < 30)
        {
            select -= 20;
            r.material = Resources.Load("Material/MaterialBaseColor/MaterialLosange", typeof(Material)) as Material;
        }
        else if (idMaterial < 40)
        {
            select -= 30;
            r.material = Resources.Load("Material/MaterialBaseColor/MaterialTriangle", typeof(Material)) as Material;
        }
        else if (idMaterial < 50)
        {
            select -= 40;
            r.material = Resources.Load("Material/MaterialBaseColor/MaterialGrid", typeof(Material)) as Material;
        }
        else if (idMaterial < 60)
        {
            select -= 50;
            r.material = Resources.Load("Material/MaterialBaseColor/MaterialNoise", typeof(Material)) as Material;
        }
        else
        {
            r.material = Resources.Load("Material/MaterialBaseColor/MaterialBall", typeof(Material)) as Material;
            select = -1;
        }

        switch (select)
        {
            case 0: r.material.color = Color.blue; break;
            case 1: r.material.color = Color.red; break;
            case 2: r.material.color = new Color(0.0f, 1f, 0.0f); break;
            case 3: r.material.color = new Color(1.0f, 1.0f, 0.0f); break;
            case 4: r.material.color = Color.cyan; break;
            case 5: r.material.color = new Color(1.0f, 0.0f, 1.0f); break;
            case 6: r.material.color = new Color(1.0f, 0.50f, 0.0f); break;
            case 7: r.material.color = new Color(0.0f, 0.5f, 0.0f); break;
            case 8: r.material.color = new Color(1.0f, 1.0f, 1.0f); break;
            case 9: r.material.color = new Color(0.45f, 0.45f, 0.45f); break;
            default:
                r.material.color = new Color(0.0f, 0.0f, 0.0f);
                break;
        }
    }

    public override void SetHideBall(bool hide)
    {
        if (_isHide != hide)
        {
            Renderer r = this.BallObject.GetComponent<Renderer>();
            if (hide)
            {
                SelectMaterial(-1);
                SelectWeight(0);
                _isHide = true;
            }
            else
            {
                SelectMaterial(_idMaterial);
                SelectWeight(_weight);
                _isHide = false;
            }
        }
    }
}
