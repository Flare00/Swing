using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBall : Ball
{
    public NormalBall(int weight, int idMaterial = 0) : base(weight, idMaterial)
    {
        this.IsNormalBall = true;
        this.BallObject = GameObject.Instantiate(Resources.Load("Prefabs/Sphere", typeof(GameObject))) as GameObject;

        AudioSource audioSource = this.BallObject.AddComponent<AudioSource>();
        audioSource.clip = Resources.Load("../") as AudioClip;
        audioSource.Play();

        //this._ballObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        GameObject text = this.BallObject.transform.GetChild(0).gameObject;
        if (weight > 0)
        {
            text.GetComponent<TMPro.TextMeshPro>().text = "" + weight;
        }
        else
        {
            text.GetComponent<TMPro.TextMeshPro>().text = "";
        }

        Renderer r = this.BallObject.GetComponent<Renderer>();

        int select = idMaterial;

        if (idMaterial < 12)
        {
            r.material = Resources.Load("Material/MaterialBaseColor/MaterialBall", typeof(Material)) as Material;
        }
        else if (idMaterial < 24)
        {
            select -= 12;
            r.material = Resources.Load("Material/MaterialBaseColor/MaterialVoronoi", typeof(Material)) as Material;
        }
        else if (idMaterial < 36)
        {
            select -= 24;
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
            case 2: r.material.color = new Color(0.0f, 0.35f, 0.0f); break;
            case 3: r.material.color = Color.cyan; break;
            case 4: r.material.color = new Color(0.28f, 0.18f, 0.05f); break;
            case 5: r.material.color = new Color(0.40f, 0.0f, 0.40f); break;
            case 6: r.material.color = new Color(0.70f, 0.45f, 0.12f); break;
            case 7: r.material.color = new Color(0.25f, 0.25f, 0.25f); break;
            case 8: r.material.color = new Color(0.4f, 1.0f, 0.4f); break;
            case 9: r.material.color = new Color(1.0f, 0.2f, 1.0f); break;
            case 10: r.material.color = new Color(1.0f, 0.45f, 0.14f); break;
            case 11: r.material.color = new Color(1.0f, 1.0f, 0.0f); break;
            default:
                r.material.color = new Color(0.0f, 0.0f, 0.0f);
                break;
        }
    }

    public override void Action(GameZone zone, int x, int y)
    {
    }

    public override object Clone()
    {
        return new NormalBall(this.Weight, this.IdMaterial);
    }
}
