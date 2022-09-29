using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPrincipalScript : MonoBehaviour, ISettingsCallBack, ILeaderboardChange
{
    public Camera MainCamera;
    public TransitionScript transitionScript;
    public EventSystem eventSystem;
    public GameObject menuSettings;
    public GameObject buttonsContainer;
    public GameObject leaderboardMenu;
    public GameObject buttonBackLeaderboard;
    public GameObject selectGameMode;
    public GameObject buttonBackSelectGameMode;
    public GameObject scoreAnchor;
    public GameObject loadGamePopup;
    public Button buttonUM;
    private ControlsGame.UIActions _actions;

    private bool _leaderboardShow = false;
    private bool _selectGameModeShow = false;
    // Start is called before the first frame update
    void Start()
    {
        Leaderboard.GetInstance().SetListener(this);

        menuSettings.transform.Find("SettingsScript").GetComponent<SettingsMenuScript>().SetHideSettingsCallback(this);
        if (CrossSceneData.TransitionMainMenu)
        {
            transitionScript.ReverseTransition();
        }
        ControlsGame cg = new ControlsGame();
        _actions = cg.UI;
        _actions.Enable();
    }

    public void Update()
    {
        if (_actions.Cancel.IsPressed())
        {
            if (_leaderboardShow)
            {
                this.HideLeaderboard();
            }
            if (_selectGameModeShow)
            {
                this.HideSelectGameMode();
            }
        }
    }

    public void Solo()
    {
        if(SaveManager.instance.HasSave()){
            selectGameMode.SetActive(false);
            loadGamePopup.SetActive(true);
            eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(loadGamePopup.transform.Find("Button_LoadGame").gameObject);
        }
        else{
            NewGame();
        }
    }

    public void NewGame()
    {
        CrossSceneData.Multijoueur = false;
        CrossSceneData.Mission = false;
        CrossSceneData.TransitionMainMenu = true;
        CrossSceneData.LoadGame = false;
        MultiplayerSystem.RemoveInstance();

        transitionScript.LoadSceneWithTransition("Game");
        eventSystem.gameObject.SetActive(false);
    }
    public void LoadGame()
    {
        CrossSceneData.Multijoueur = false;
        CrossSceneData.Mission = false;
        CrossSceneData.TransitionMainMenu = true;
        CrossSceneData.LoadGame = true;
        MultiplayerSystem.RemoveInstance();

        transitionScript.LoadSceneWithTransition("Game");
        eventSystem.gameObject.SetActive(false);
    }

    public void Multi(bool coop)
    {

        CrossSceneData.Multijoueur = true;
        CrossSceneData.Mission = false;
        CrossSceneData.TransitionMainMenu = true;

        MultiplayerSystem.CreateInstance(coop);
        transitionScript.LoadSceneWithTransition("Game");
        eventSystem.gameObject.SetActive(false);
    }

    public void Mission()
    {
        /*CrossSceneData.Multijoueur = false;
        CrossSceneData.Mission = true;
        SceneManager.LoadScene("Game");*/
        // CrossSceneData.TransitionMainMenu = true;
        Debug.Log("Mission Clicked");
        // MissionSelector.getInstance().Test();
    }

    public void Instruction()
    {
        CrossSceneData.TransitionMainMenu = true;
        transitionScript.LoadSceneWithTransition("Extras");
    }

    public void Options()
    {
        _actions.Disable();
        eventSystem.gameObject.SetActive(false);

        menuSettings.transform.Find("SettingsScript").GetComponent<SettingsMenuScript>().InitializeElements();

        menuSettings.transform.Find("Canvas").gameObject.SetActive(true);
        menuSettings.transform.Find("EventSystem").gameObject.SetActive(true);
        eventSystem.SetSelectedGameObject(null);
    }

    public void HideSettings()
    {
        _actions.Enable();
        eventSystem.gameObject.SetActive(true);
        menuSettings.transform.Find("Canvas").gameObject.SetActive(false);
        menuSettings.transform.Find("EventSystem").gameObject.SetActive(false);
        eventSystem.SetSelectedGameObject(buttonsContainer.transform.Find("Button_Option").gameObject);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void ShowLeaderboard()
    {
        Leaderboard.GetInstance().SetListener(this);
        Leaderboard.GetInstance().FetchTop10();
        eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(buttonBackLeaderboard.gameObject);
        _leaderboardShow = true;
        leaderboardMenu.SetActive(true);
    }
    public void HideLeaderboard()
    {
        eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(buttonsContainer.transform.Find("Button_Leaderboard").gameObject);
        _leaderboardShow = false;
        leaderboardMenu.SetActive(false);
    }

    public void ShowSelectGameMode()
    {
        buttonsContainer.SetActive(false);
        eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(selectGameMode.transform.Find("Button_Solo").gameObject);
        _selectGameModeShow = true;
        selectGameMode.SetActive(true);
        setUMNavigation(1);
    }


    public void HideSelectGameMode()
    {
        eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(buttonsContainer.transform.Find("Button_Play").gameObject);
        _selectGameModeShow = false;
        selectGameMode.SetActive(false);
        buttonsContainer.SetActive(true);
        setUMNavigation(0);
    }

    private void setUMNavigation(int menu)
    {
        Navigation nNav = new Navigation();
        nNav.mode = Navigation.Mode.Explicit;
        nNav.selectOnRight = buttonUM.navigation.selectOnRight;
        switch (menu)
        {
            case 0:
                nNav.selectOnLeft = buttonsContainer.transform.Find("Button_Exit").gameObject.GetComponent<Button>();
                break;
            case 1:
                nNav.selectOnLeft = selectGameMode.transform.Find("Button_Back").gameObject.GetComponent<Button>();
                break;
            default:
                nNav.selectOnLeft = buttonsContainer.transform.Find("Button_Exit").gameObject.GetComponent<Button>();
                break;
        }
        buttonUM.navigation = nNav;
    }

    public void OnLeaderboardReceive(Leaderboard.ScorePlayer[] list)
    {
        throw new System.NotImplementedException();
    }

    public void OnTop10Receive(Leaderboard.ScorePlayer[] list)
    {
        Leaderboard.GenerateGameobjectTab(this.scoreAnchor, list);
    }

    public void OnAroundReceive(Leaderboard.ScorePlayer[] list)
    {
        throw new System.NotImplementedException();
    }

    public void ChangeLanguage()
    {
        if (LocalizationSettings.SelectedLocale.Identifier.Code.Trim().ToLower().Equals("fr"))
        {
            LocalizationSettings.SelectedLocale = Locale.CreateLocale(new LocaleIdentifier(SystemLanguage.English));
            SettingsData.SavedSettings.langue = "en";
        } 
        else
        {
            LocalizationSettings.SelectedLocale = Locale.CreateLocale(new LocaleIdentifier(SystemLanguage.French));
            SettingsData.SavedSettings.langue = "fr";
        }


        menuSettings.transform.Find("SettingsScript").GetComponent<ApplySettingsScript>().SaveToFile();
    }


    public void LaunchFDSWeb()
    {
        Application.OpenURL("https://sciences.edu.umontpellier.fr/");
    }

    public void LaunchUMWeb()
    {
        Application.OpenURL("https://www.umontpellier.fr/");
    }


    public void Credits()
    {
        CrossSceneData.TransitionMainMenu = true;
        transitionScript.LoadSceneWithTransition("Credits");
    }



}
