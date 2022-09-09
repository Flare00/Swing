using SwingGame.Media;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ApplySettingsScript : MonoBehaviour
{
    public const string SETTINGS_FILE_NAME = "settings.json";
    public UniversalRenderPipelineAsset _pipelineAssets;
    public SettingsMenuScript _settingsMenuScript = null;
    private bool _initialized = false;
    private GameMusicManager _gameMusicManager;
    private string _settingsPath = "";
    // Start is called before the first frame update
    void Start()
    {
        _settingsPath = Application.persistentDataPath + "/" + SETTINGS_FILE_NAME;
        GameObject go = GameObject.Find("MusicManager");

        if(go != null)
        {
            _gameMusicManager = go.GetComponent<GameMusicManager>();
        }

        GraphicsSettings.renderPipelineAsset = _pipelineAssets;
        QualitySettings.renderPipeline = _pipelineAssets;
        ReadFromFile();

        SettingsData.ApplySavedToUnsaved();
        _settingsMenuScript.InitializeElements();
    }

    public void Update()
    {
        if (!_initialized)
        {
            _initialized = true;
            ApplySettings();
        }
    }

    private void ReadFromFile()
    {
        bool readSuccess = false;
        if (System.IO.File.Exists(_settingsPath))
        {
            string data = System.IO.File.ReadAllText(_settingsPath);

            if (data.Trim().Length > 0)
            {
                SettingsData.SavedSettings = JsonUtility.FromJson<SettingsData.Data>(data);
                readSuccess = true;
            }
        }

        if(!readSuccess)
        {
            SettingsData.SavedSettings.msaa = 2;
            SettingsData.SavedSettings.renderScale = 1.0f;
            SettingsData.SavedSettings.vSync = 1;
            SettingsData.SavedSettings.musicVolume = 1.0f;
            SettingsData.SavedSettings.soundEffectVolume = 1.0f;
            SettingsData.SavedSettings.langue = "fr";
        }
    }

    public void SaveToFile()
    {
        string json = JsonUtility.ToJson(SettingsData.SavedSettings);

        System.IO.FileInfo file = new System.IO.FileInfo(_settingsPath);
        System.IO.File.WriteAllText(file.FullName, json);
    }

    public void ApplySettings()
    {
        SettingsData.ApplyUnsavedToSaved();

        SaveToFile();

        _pipelineAssets.msaaSampleCount = SettingsData.SavedSettings.msaa;
        _pipelineAssets.renderScale = SettingsData.SavedSettings.renderScale;

        QualitySettings.vSyncCount = SettingsData.SavedSettings.vSync;

        AudioManager.SOUND_VOLUME = SettingsData.SavedSettings.soundEffectVolume;
        if(_gameMusicManager != null)
        {
            _gameMusicManager.ChangeVolume(SettingsData.SavedSettings.musicVolume);
        }

        if(SettingsData.SavedSettings.langue == "fr")
        {
            LocalizationSettings.SelectedLocale = Locale.CreateLocale(new LocaleIdentifier(SystemLanguage.French));
        }
        else if (SettingsData.SavedSettings.langue == "en")
        {
            LocalizationSettings.SelectedLocale = Locale.CreateLocale(new LocaleIdentifier(SystemLanguage.English));
        }
    }

}
