using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.PointerEventData;
using UnityEngine.InputSystem;

public class PauseMenuScript : MonoBehaviour, ISettingsCallBack
{

    public SwingLoop swingloop;
    public Canvas canvas;
    public EventSystem eventSystem;
    public TransitionScript transition;
    public GameObject menuSettings;
    public GameObject buttonsContainer;
    private bool _isPause = false;
    private float _pauseFade = 0.0f;
    private float _speedFade = 3.0f;

    private bool _isOptionShow = false;
    private bool _waitForRelease = false;
    private ControlsGame.UIActions uiActions;

    public void Start()
    {
        menuSettings.transform.Find("SettingsScript").GetComponent<SettingsMenuScript>().SetHideSettingsCallback(this);
        menuSettings.gameObject.SetActive(false);
        eventSystem.gameObject.SetActive(false);
        canvas.gameObject.SetActive(false);
        canvas.GetComponent<CanvasGroup>().alpha = 0.0f;

        ControlsGame cg = new ControlsGame();
        uiActions = cg.UI;
    }

    public void Update()
    {
        if (uiActions.Cancel.IsPressed())
        {
            if (!_isOptionShow && !_waitForRelease)
            {
                ResumeAction();
            }
        } else
        {
            if(_waitForRelease) _waitForRelease = false;
        }

        if (_isPause && _pauseFade < 1.0f)
        {
            _pauseFade += Time.deltaTime * _speedFade;
            if (_pauseFade < 1.0f)
            {
                canvas.GetComponent<CanvasGroup>().alpha = _pauseFade;
            }
            else
            {
                canvas.GetComponent<CanvasGroup>().alpha = 1.0f;
                eventSystem.gameObject.SetActive(true);
            }
        }
        else if (!_isPause && _pauseFade > 0.0f)
        {
            _pauseFade -= Time.deltaTime * _speedFade;
            if (_pauseFade > 0.0f)
            {
                canvas.GetComponent<CanvasGroup>().alpha = _pauseFade;
            }
            else
            {
                canvas.GetComponent<CanvasGroup>().alpha = 0.0f;
                canvas.gameObject.SetActive(false);
            }
        }
    }

    public void Show()
    {
        uiActions.Enable();
        _isPause = true;
        canvas.gameObject.SetActive(true);
    }

    public void Hide()
    {
        HideSettings();
        uiActions.Disable();
        _isPause = false;
        eventSystem.gameObject.SetActive(false);


    }

    public void ResumeAction()
    {
        swingloop.ResumeAction();
    }
    public void RestartAction()
    {
        transition.LoadSceneWithTransition("Game");
    }

    public void MainMenuAction()
    {
        transition.LoadSceneWithTransition("Menu");
    }

    public void QuitGameAction()
    {
        Application.Quit();
    }

    public void Options()
    {
        menuSettings.gameObject.SetActive(true);
        _isOptionShow = true;
        eventSystem.gameObject.SetActive(false);


        menuSettings.transform.Find("SettingsScript").GetComponent<SettingsMenuScript>().InitializeElements();
        menuSettings.transform.Find("SettingsScript").GetComponent<SettingsMenuScript>().EnableInput();

        menuSettings.transform.Find("Canvas").gameObject.SetActive(true);
        menuSettings.transform.Find("EventSystem").gameObject.SetActive(true);
        eventSystem.SetSelectedGameObject(null);
    }

    public void HideSettings()
    {
        _isOptionShow = false;
        _waitForRelease = true;
        eventSystem.gameObject.SetActive(true);
        menuSettings.transform.Find("SettingsScript").GetComponent<SettingsMenuScript>().DisableInput();
        menuSettings.transform.Find("Canvas").gameObject.SetActive(false);
        menuSettings.transform.Find("EventSystem").gameObject.SetActive(false);
        menuSettings.gameObject.SetActive(false);

        eventSystem.SetSelectedGameObject(buttonsContainer.transform.Find("ButOption").gameObject);
    }
}
