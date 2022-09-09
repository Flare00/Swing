using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionScript : MonoBehaviour
{
    public Animator transitionAnimator;

    private bool _transitionLoadScene = false;
    private int _frameSkip = 2;
    private string _sceneToLoad = "Game";

    private bool _launchStandard = false;
    private bool _launchReverse = false;

    // Update is called once per frame
    void Update()
    {
        if (_launchStandard)
        {
            _launchStandard = false;
            transitionAnimator.Play("Anim_Transition");
        } 
        if (_launchReverse)
        {
            _launchReverse = false;
            transitionAnimator.Play("Anim_ReverseTransition");
        }
        if (_transitionLoadScene && _sceneToLoad.Trim().Length > 0)
        {
            if(_frameSkip < 0)
            {
                if (transitionAnimator.GetCurrentAnimatorStateInfo(0).length < transitionAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime)
                {
                    SceneManager.LoadScene(_sceneToLoad.Trim());
                    _transitionLoadScene = false;
                }
            } else
            {
                _frameSkip--;
            }
        }
    }

    public void LoadSceneWithTransition(string scene)
    {
        if (!_transitionLoadScene)
        {
            _sceneToLoad = scene;
            _transitionLoadScene = true;
            _launchStandard = true;
        }

    }

    public void ReverseTransition()
    {
        _launchReverse = true;
    }
}
