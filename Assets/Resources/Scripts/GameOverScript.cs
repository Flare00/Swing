using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour, IAskNameListener, ILeaderboardChange
{
    public Canvas canvas;
    public EventSystem eventSystem;
    public TransitionScript transitionScript;
    public GameObject scoreAnchor;
    private bool _isGameOver = false;
    private float _gameOverFade = 0.0f;
    private float _speed = 2.0f;
    private float _avancementScore = 0;
    private float _speedScore = 0.5f;
    private ulong _scoreP1 = 0;
    private ulong _scoreP2 = 0;

    private bool _solo = true;

    private GameObject _askNameP1;

    public void Start()
    {
        eventSystem.gameObject.SetActive(false);
        canvas.gameObject.SetActive(false);
        canvas.transform.Find("InfoPartieDuo/Winner_Value").gameObject.SetActive(false);
        canvas.GetComponent<CanvasGroup>().alpha = 0.0f;
    }

    public void Update()
    {
        if (_isGameOver && _gameOverFade < 1.0f)
        {
            _gameOverFade += Time.deltaTime * _speed;
            if (_gameOverFade < 1.0f)
            {
                canvas.GetComponent<CanvasGroup>().alpha = _gameOverFade;
            }
            else
            {
                eventSystem.gameObject.SetActive(true);
                canvas.GetComponent<CanvasGroup>().alpha = 1.0f;
            }
        }
        else if (!_isGameOver && _gameOverFade > 0.0f)
        {
            _gameOverFade -= Time.deltaTime * _speed;
            if (_gameOverFade > 0.0f)
            {
                canvas.GetComponent<CanvasGroup>().alpha = _gameOverFade;
            }
            else
            {
                canvas.GetComponent<CanvasGroup>().alpha = 0.0f;
                canvas.gameObject.SetActive(false);
            }
        }

        if (_scoreP1 + _scoreP2 > 0 && _avancementScore < 1.0f)
        {
            _avancementScore += Time.deltaTime * _speedScore;
            if (_avancementScore >= 1.0f)
            {
                _avancementScore = 1.0f;
                if (_solo)
                {
                    canvas.transform.Find("InfoPartieSolo/Score_Value").GetComponent<TMPro.TextMeshProUGUI>().text = _scoreP1.ToString("n0");
                } 
                else
                {
                    canvas.transform.Find("InfoPartieDuo/ScoreP1_Value").GetComponent<TMPro.TextMeshProUGUI>().text = _scoreP1.ToString("n0");
                    canvas.transform.Find("InfoPartieDuo/ScoreP2_Value").GetComponent<TMPro.TextMeshProUGUI>().text = _scoreP2.ToString("n0");

                    canvas.transform.Find("InfoPartieDuo/Winner_Value").gameObject.SetActive(true);


                    if (_scoreP1 > _scoreP2)
                    {
                        LocalizedString lsPlayer = new LocalizedString("Local", "joueur");
                        LocalizedString lsWin = new LocalizedString("Local", "gagne");
                        canvas.transform.Find("InfoPartieDuo/Winner_Value").GetComponent<TMPro.TextMeshProUGUI>().text = lsPlayer.GetLocalizedString() + " 1 " + lsWin.GetLocalizedString();
                    }
                    else if (_scoreP2 > _scoreP1)
                    {
                        LocalizedString lsPlayer = new LocalizedString("Local", "joueur");
                        LocalizedString lsWin = new LocalizedString("Local", "gagne");
                        canvas.transform.Find("InfoPartieDuo/Winner_Value").GetComponent<TMPro.TextMeshProUGUI>().text = lsPlayer.GetLocalizedString() + " 2 " + lsWin.GetLocalizedString();
                    }
                }
            } 
            else
            {
                if (_solo)
                {

                    canvas.transform.Find("InfoPartieSolo/Score_Value").GetComponent<TMPro.TextMeshProUGUI>().text = ((ulong)(_scoreP1 * _avancementScore)).ToString("n0");
                }
                else
                {

                    //LocalizationSettings.StringDatabase.GetTable("");
                    canvas.transform.Find("InfoPartieDuo/ScoreP1_Value").GetComponent<TMPro.TextMeshProUGUI>().text = ((ulong)(_scoreP1 * _avancementScore)).ToString("n0");
                    canvas.transform.Find("InfoPartieDuo/ScoreP2_Value").GetComponent<TMPro.TextMeshProUGUI>().text = ((ulong)(_scoreP2 * _avancementScore)).ToString("n0");
                }
            }
        }
    }

    public void Show(bool solo = true)
    {
        _solo = solo;
        if (_solo)
        {
            canvas.transform.Find("InfoPartieSolo").gameObject.SetActive(true);
            canvas.transform.Find("InfoPartieDuo").gameObject.SetActive(false);
        }
        else
        {
            canvas.transform.Find("InfoPartieSolo").gameObject.SetActive(false);
            canvas.transform.Find("InfoPartieDuo").gameObject.SetActive(true);
        }
        _isGameOver = true;
        canvas.gameObject.SetActive(true);

        Leaderboard.GetInstance().SetListener(this);
        Leaderboard.GetInstance().FetchTop10();
    }

    public void Hide()
    {
        eventSystem.gameObject.SetActive(false);
        _isGameOver = false;
    }

    public void SetScore(ulong p1Score, ulong p2Score = 0)
    {
        this._scoreP1 = p1Score;
        this._scoreP2 = p2Score;

        if(this._solo && _scoreP1 > 0)
        {
            AskName();
        }
    }

    public void AskName()
    {
        if (this._solo && _scoreP1 > 0)
        {
            this._askNameP1 = GameObject.Instantiate(Resources.Load("Prefabs/Leaderboard/AskName", typeof(GameObject))) as GameObject;
            this._askNameP1.transform.Find("AskNameScript").GetComponent<AskNameScript>().SetListener(this);

            this.eventSystem.SetSelectedGameObject(this._askNameP1.transform.Find("Selector/Letter1").gameObject);
        } 
    }

    public void RestartAction()
    {
        CrossSceneData.LoadGame = false;
        transitionScript.LoadSceneWithTransition("Game");
    }

    public void MainMenuAction()
    {
        transitionScript.LoadSceneWithTransition("Menu");
    }

    public void askNameValidate(AskNameScript askNameScript, string name)
    {

        System.DateTime dt = System.DateTime.Now;
        string horodatage = dt.Year + "" + dt.Month + "" + dt.Day + "_" + dt.Hour + "" + dt.Minute + "" + dt.Second;

        Leaderboard lb = Leaderboard.GetInstance();
        lb.SetListener(this);

        if (this._askNameP1.transform.Find("AskNameScript").GetComponent<AskNameScript>() == askNameScript)
        {
            this._askNameP1.gameObject.SetActive(false);
            lb.SendScore(new Leaderboard.ScorePlayer(name, this._scoreP1, horodatage));
            this.eventSystem.SetSelectedGameObject(this.canvas.transform.Find("Buttons/Button_Restart").gameObject);
        }
    }

    public void askNameCancel(AskNameScript askNameScript)
    {
        if (this._askNameP1.transform.Find("AskNameScript").GetComponent<AskNameScript>() == askNameScript)
        {
            this._askNameP1.gameObject.SetActive(false);
            this.eventSystem.SetSelectedGameObject(this.canvas.transform.Find("Buttons/Button_Restart").gameObject);
        }

        Leaderboard.GetInstance().SetListener(this);
        Leaderboard.GetInstance().FetchTop10();
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
}
