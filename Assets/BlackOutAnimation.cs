using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackOutAnimation : MonoBehaviour
{
    private static float DURATION = 5.0f;
    private static string TIME_COMPTEUR_NAME = "Vector1_42aeeb7121a24ca39551fdd03d4ff25c";
    private float time = 0.0f;
    private bool started = false;
    public Material materialCompteur = null;
    public Animator animatorAiguille = null;
    
    // Start is called before the first frame update
    void Start()
    {
        materialCompteur.SetFloat(TIME_COMPTEUR_NAME, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (started)
        {
            time += Time.deltaTime * (1.0f / DURATION);
            materialCompteur.SetFloat(TIME_COMPTEUR_NAME, time);
        }

    }

    void StartAnimation()
    {
        this.animatorAiguille.Play("Anim_Aiguille");
        this.started = true;
    }
}
