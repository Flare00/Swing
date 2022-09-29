using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditScript : MonoBehaviour
{
    public TransitionScript transitionScript;
    // Start is called before the first frame update
    void Start()
    {
        transitionScript.ReverseTransition();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BackAction()
    {
        Debug.Log("BACK");
        transitionScript.LoadSceneWithTransition("Menu");
    }
}
