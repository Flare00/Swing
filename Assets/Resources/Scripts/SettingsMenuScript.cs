using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsMenuScript : MonoBehaviour
{
    public GameObject container;
    public ApplySettingsScript applySettingsScript;
    public EventSystem eventSystem;
    private GameMusicManager _gameMusicManager = null;
    private ISettingsCallBack _hideCallBack;
    private bool firstChange = true;
    private bool _waitForRelease = false;
    private ControlsGame actions;


    // Start is called before the first frame update
    void Start()
    {

        actions = new ControlsGame();
        GameObject go = GameObject.Find("MusicManager");
        if(go != null)
        {
            _gameMusicManager = go.GetComponent<GameMusicManager>();
        }
    }

    public void Update()
    {
        if (actions.UI.Cancel.IsPressed() && !_waitForRelease)
        {
            Cancel();
        }
        else if (_waitForRelease)
        {
            _waitForRelease = false;
        }
    }

    public void EnableInput()
    {
        _waitForRelease = true;
        if(actions != null)
        {
            actions.UI.Enable();
        } else
        {
            actions = new ControlsGame();
            actions.UI.Enable();
        }
    }
    public void DisableInput()
    {
        if(actions != null)
        {
            actions.UI.Disable();
        }
    }

    public void InitializeElements()
    {
        {
            TMPro.TMP_Dropdown msaaDrop = container.transform.Find("Aliasing").gameObject.GetComponentInChildren<TMPro.TMP_Dropdown>();
            switch (SettingsData.SavedSettings.msaa)
            {
                case 1:
                    msaaDrop.value = 0;
                    break;
                case 2:
                    msaaDrop.value = 1;
                    break;
                case 4:
                    msaaDrop.value = 2;
                    break;
                case 8:
                    msaaDrop.value = 3;
                    break;
            }
        }

        {
            container.transform.Find("RenderScale/RenderScale_Slider").gameObject.GetComponent<Slider>().value = SettingsData.SavedSettings.renderScale * 10;
            container.transform.Find("RenderScale/RenderScale_Value").GetComponent<TMPro.TextMeshProUGUI>().text = SettingsData.SavedSettings.renderScale.ToString("0.0");
        }

        {
            int value = (int)(SettingsData.SavedSettings.musicVolume*10.0f);
            container.transform.Find("Music/Music_Value").GetComponent<TMPro.TextMeshProUGUI>().text = (((float)value) / 10.0f).ToString("0.0");
            container.transform.Find("Music/Music_Slider").GetComponent<Slider>().value = value;
            if(_gameMusicManager != null)
            {
                _gameMusicManager.ChangeVolume(SettingsData.SavedSettings.musicVolume);
            }
        }


        {
            int value = (int)(SettingsData.SavedSettings.soundEffectVolume * 10.0f);
            container.transform.Find("SoundEffect/SoundEffect_Value").GetComponent<TMPro.TextMeshProUGUI>().text = (((float)value) / 10.0f).ToString("0.0");
            container.transform.Find("SoundEffect/SoundEffect_Slider").GetComponent<Slider>().value = value;
        }
    }


    public void MSAAChange(TMPro.TMP_Dropdown change)
    {
        switch (change.value)
        {
            case 0:
                SettingsData.UnsavedSettings.msaa = 1;
                break;
            case 1:
                SettingsData.UnsavedSettings.msaa = 2;
                break;
            case 2:
                SettingsData.UnsavedSettings.msaa = 4;
                break;
            case 3:
                SettingsData.UnsavedSettings.msaa = 8;
                break;

        }
    }

    public void MusicVolumeChange(Slider slider)
    {
        float value = slider.value / 10.0f;
        container.transform.Find("Music/Music_Value").GetComponent<TMPro.TextMeshProUGUI>().text = value.ToString("0.0");
        SettingsData.UnsavedSettings.musicVolume = value;
        if(_gameMusicManager != null)
        {
            _gameMusicManager.ChangeVolume(value);
        }
    }

    public void SoundEffectVolumeChange(Slider slider)
    {
        float value = slider.value / 10.0f;
        container.transform.Find("SoundEffect/SoundEffect_Value").GetComponent<TMPro.TextMeshProUGUI>().text = value.ToString("0.0");
        SettingsData.UnsavedSettings.soundEffectVolume = value;
        if (firstChange)
        {
            firstChange = false;
        } else
        {
            AudioSource.PlayClipAtPoint(Resources.Load("Audio/DropBall") as AudioClip, gameObject.transform.position, value);
        }

    }


    public void RenderScaleChange(Slider slider)
    {
        float value = slider.value / 10.0f;
        container.transform.Find("RenderScale/RenderScale_Value").GetComponent<TMPro.TextMeshProUGUI>().text = value.ToString("0.0");
        SettingsData.UnsavedSettings.renderScale = value;
    }


    public void VSyncChange(TMPro.TMP_Dropdown change)
    {
        SettingsData.UnsavedSettings.vSync = change.value;
    }

    public void Confirm()
    {
        eventSystem.SetSelectedGameObject(container.transform.Find("Music/Music_Slider").gameObject);
        applySettingsScript.ApplySettings();
        _hideCallBack.HideSettings();
    }

    public void Cancel()
    {
        eventSystem.SetSelectedGameObject(container.transform.Find("Music/Music_Slider").gameObject);
        if (_gameMusicManager != null)
        {
            _gameMusicManager.ChangeVolume(SettingsData.SavedSettings.musicVolume);
        }
        _hideCallBack.HideSettings();
    }

    public void SetHideSettingsCallback(ISettingsCallBack callBack)
    {
        this._hideCallBack = callBack;
    }
}
