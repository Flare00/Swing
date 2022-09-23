using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.UI;
using UnityEngine.Video;

public class ExtrasScript : MonoBehaviour, ControlsGame.IUIActions
{

    public GameObject panelPU;
    public VideoPlayer videoPlayer;
    public GameObject textHeader;
    public GameObject textContent;

    public TransitionScript transitionScript;

    private GameObject ring = null;
    private int posX = 0;
    private int posY = 0;

    private int sizeX = 5;
    private int sizeY = 5;

    public float offsetX;
    public float offsetY;

    void Start()
    {
        transitionScript.ReverseTransition();

        ControlsGame cg = new ControlsGame();
        cg.UI.SetCallbacks(this);
        cg.UI.Enable();

        Ball ball;


        for (int i = 0; i < sizeY; i++)
        {
            for (int j = 0; j < sizeX; j++)
            {
                switch ((j + i * sizeY) + 1)
                {
                    case 1:
                        ball = new JokerBall(false);
                        break;
                    case 2:
                        ball = new BombBall(false);
                        break;
                    case 3:
                        ball = new CutterBall(false);
                        break;
                    case 4:
                        ball = new ZapHorizontalBall(false);
                        break;
                    case 5:
                        ball = new ZapDiagonalBall(false);
                        break;
                    case 6:
                        ball = new CopyLineBall(false);
                        break;
                    case 7:
                        ball = new CopySquareBall(false);
                        break;
                    case 8:
                        ball = new CopyPredictionBall(false);
                        break;
                    case 9:
                        ball = new StarBall(false);
                        break;
                    case 10:
                        ball = new GoldenStarBall(false);
                        break;
                    case 11:
                        ball = new PlasmaNoTriangleBall(false);
                        break;
                    case 12:
                        ball = new PlasmaEmptyTriangleBall(false);
                        break;
                    case 13:
                        ball = new PlasmaFullTriangleBall(false);
                        break;
                    case 14:
                        ball = new BrickBall(false);
                        break;
                    case 15:
                        ball = new BrickSquare(false);
                        break;
                    case 16:
                        ball = new TransformJokerBall(false);
                        break;
                    case 17:
                        ball = new TransformBombBall(false);
                        break;
                    case 18:
                        ball = new TransformDestroyBall(false);
                        break;
                    case 19:
                        ball = new TransformBrickBall(false);
                        break;
                    case 20:
                        ball = new BrickTower(false);
                        break;
                    case 21:
                        ball = new StingBall(false);
                        break;
                    case 22:
                        ball = new TornadoBall(false);
                        break;
                    case 23:
                        ball = new JokerBall(false); // TODO
                        break;
                    case 24:
                        ball = new JokerBall(false); // TODO
                        break;
                    case 25:
                        ball = new RandomBall(false);
                        break;
                    default:
                        ball = new BombBall(false);
                        break;
                }

                ball.BallObject.transform.parent = panelPU.transform;
                ball.BallObject.transform.position = new Vector3(j * 1.5f + offsetX, (sizeY - i) * 1.5f + offsetY, 0);
                ball.BallObject.AddComponent<SphereCollider>();
                ExtrasTriggerScript script = ball.BallObject.AddComponent<ExtrasTriggerScript>();
                script.x = j;
                script.y = i;
                script.mainScript = this;
            }
        }
        ring = GameObject.Instantiate(Resources.Load("Prefabs/Extras/Circle", typeof(GameObject))) as GameObject;
        ring.transform.parent = panelPU.transform;

        moveCursor(posX, posY); // Set first hover on Joker
    }

    public void moveCursor(int x, int y)
    {
        if(ring == null)
        {
            return;
        }
        if (this.posX != x) this.posX = x;
        if (this.posY != y) this.posY = y;
        ring.transform.position = new Vector3(x * 1.5f + offsetX, (sizeY - y) * 1.5f + offsetY, 0);
        LocalizedString header;
        LocalizedString content;

        switch ((x + y * sizeX) + 1)
        {
            case 1:
                // ball = new JokerBall();
                videoPlayer.clip = Resources.Load("Videos/VideoJoker", typeof(VideoClip)) as VideoClip;
                header = new LocalizedString("PowerUp", "joker_h");
                content = new LocalizedString("PowerUp", "joker_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 2:
                // ball = new BombBall();
                videoPlayer.clip = Resources.Load("Videos/VideoBomb", typeof(VideoClip)) as VideoClip;
                header = new LocalizedString("PowerUp", "bomb_h");
                content = new LocalizedString("PowerUp", "bomb_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 3:
                // ball = new CutterBall();
                videoPlayer.clip = Resources.Load("Videos/VideoCutter", typeof(VideoClip)) as VideoClip;
                header = new LocalizedString("PowerUp", "cutter_h");
                content = new LocalizedString("PowerUp", "cutter_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 4:
                // ball = new ZapHorizontalBall();
                videoPlayer.clip = Resources.Load("Videos/VideoZapH", typeof(VideoClip)) as VideoClip;
                header = new LocalizedString("PowerUp", "zapH_h");
                content = new LocalizedString("PowerUp", "zapH_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 5:
                // ball = new ZapDiagonalBall();
                videoPlayer.clip = Resources.Load("Videos/VideoZapD", typeof(VideoClip)) as VideoClip;
                header = new LocalizedString("PowerUp", "zapD_h");
                content = new LocalizedString("PowerUp", "zapD_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 6:
                // ball = new CopyLineBall();
                videoPlayer.clip = Resources.Load("Videos/VideoCopyLine", typeof(VideoClip)) as VideoClip;
                header = new LocalizedString("PowerUp", "copyLine_h");
                content = new LocalizedString("PowerUp", "copyLine_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 7:
                // ball = new CopySquareBall();
                videoPlayer.clip = Resources.Load("Videos/VideoCopySquare", typeof(VideoClip)) as VideoClip;
                header = new LocalizedString("PowerUp", "copySquare_h");
                content = new LocalizedString("PowerUp", "copySquare_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 8:
                // ball = new CopyPredictionBall();
                videoPlayer.clip = Resources.Load("Videos/VideoCopyPrediction", typeof(VideoClip)) as VideoClip;
                header = new LocalizedString("PowerUp", "copyPrediction_h");
                content = new LocalizedString("PowerUp", "copyPrediction_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 9:
                // ball = new StarBall();
                videoPlayer.clip = Resources.Load("Videos/VideoStarWhite", typeof(VideoClip)) as VideoClip;
                header = new LocalizedString("PowerUp", "star_h");
                content = new LocalizedString("PowerUp", "star_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 10:
                // ball = new GoldenStarBall();
                videoPlayer.clip = Resources.Load("Videos/VideoStarGolden", typeof(VideoClip)) as VideoClip;
                header = new LocalizedString("PowerUp", "starGold_h");
                content = new LocalizedString("PowerUp", "starGold_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 11:
                // ball = new PlasmaNoTriangleBall();
                videoPlayer.clip = Resources.Load("Videos/VideoFlash", typeof(VideoClip)) as VideoClip;
                header = new LocalizedString("PowerUp", "flash_h");
                content = new LocalizedString("PowerUp", "flash_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 12:
                // ball = new PlasmaEmptyTriangleBall();
                videoPlayer.clip = Resources.Load("Videos/VideoFlashD", typeof(VideoClip)) as VideoClip;
                header = new LocalizedString("PowerUp", "flashD_h");
                content = new LocalizedString("PowerUp", "flashD_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 13:
                // ball = new PlasmaFullTriangleBall();
                videoPlayer.clip = Resources.Load("Videos/VideoFlashT", typeof(VideoClip)) as VideoClip;
                header = new LocalizedString("PowerUp", "flashT_h");
                content = new LocalizedString("PowerUp", "flashT_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 14:
                // ball = new BrickBall();
                videoPlayer.clip = Resources.Load("Videos/VideoBrick", typeof(VideoClip)) as VideoClip;
                header = new LocalizedString("PowerUp", "brick_h");
                content = new LocalizedString("PowerUp", "brick_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 15:
                // ball = new BrickSquare();
                videoPlayer.clip = Resources.Load("Videos/VideoBrickSquare", typeof(VideoClip)) as VideoClip;
                header = new LocalizedString("PowerUp", "brickSquare_h");
                content = new LocalizedString("PowerUp", "brickSquare_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 16:
                // ball = new TransformJokerBall();
                videoPlayer.clip = Resources.Load("Videos/VideoJokerTransform", typeof(VideoClip)) as VideoClip;
                header = new LocalizedString("PowerUp", "jokerTransform_h");
                content = new LocalizedString("PowerUp", "jokerTransform_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 17:
                // ball = new TransformBombBall();
                videoPlayer.clip = Resources.Load("Videos/VideoBombTransform", typeof(VideoClip)) as VideoClip;
                header = new LocalizedString("PowerUp", "bombTransform_h");
                content = new LocalizedString("PowerUp", "bombTransform_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 18:
                // ball = new TransformDestroyBall();
                videoPlayer.clip = Resources.Load("Videos/VideoDestroyTransform", typeof(VideoClip)) as VideoClip;
                header = new LocalizedString("PowerUp", "destroyTransform_h");
                content = new LocalizedString("PowerUp", "destroyTransform_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 19:
                // ball = new TransformBrickBall();
                videoPlayer.clip = Resources.Load("Videos/VideoBrickTransform", typeof(VideoClip)) as VideoClip;
                header = new LocalizedString("PowerUp", "brickTransform_h");
                content = new LocalizedString("PowerUp", "brickTransform_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 20:
                // ball = new BrickTower();
                videoPlayer.clip = Resources.Load("Videos/VideoTower", typeof(VideoClip)) as VideoClip;
                header = new LocalizedString("PowerUp", "tower_h");
                content = new LocalizedString("PowerUp", "tower_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 21:
                // ball = new StingBall();
                videoPlayer.clip = Resources.Load("Videos/VideoSting", typeof(VideoClip)) as VideoClip;
                header = new LocalizedString("PowerUp", "sting_h");
                content = new LocalizedString("PowerUp", "sting_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 22:
                // Tornado
                videoPlayer.clip = Resources.Load("Videos/VideoJoker", typeof(VideoClip)) as VideoClip;
                header = new LocalizedString("PowerUp", "tornado_h");
                content = new LocalizedString("PowerUp", "tornado_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 23:
                // ?
                videoPlayer.clip = Resources.Load("Videos/VideoJoker", typeof(VideoClip)) as VideoClip;
                header = new LocalizedString("PowerUp", "joker_h");
                content = new LocalizedString("PowerUp", "joker_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 24:
                // ?
                videoPlayer.clip = Resources.Load("Videos/VideoJoker", typeof(VideoClip)) as VideoClip;
                header = new LocalizedString("PowerUp", "joker_h");
                content = new LocalizedString("PowerUp", "joker_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 25:
                // Random
                videoPlayer.clip = Resources.Load("Videos/VideoJoker", typeof(VideoClip)) as VideoClip;
                header = new LocalizedString("PowerUp", "random_h");
                content = new LocalizedString("PowerUp", "random_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            default:
                // ball = new BombBall();
                videoPlayer.clip = Resources.Load("Videos/VideoJoker", typeof(VideoClip)) as VideoClip;
                header = new LocalizedString("PowerUp", "bomb_h");
                content = new LocalizedString("PowerUp", "bomb_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
        }
    }

    void ControlsGame.IUIActions.OnMove(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Vector2 coords = context.ReadValue<Vector2>();
            if (coords.x > 0.01f)
            {
                posX++;
                if (posX >= sizeX)
                {
                    posX = 0;
                }
            }
            else if (coords.x < -0.01f)
            {
                posX--;
                if (posX < 0)
                {
                    posX = sizeX - 1;
                }
            }
            if (coords.y < -0.01f)
            {
                posY++;
                if (posY >= sizeY)
                {
                    posY = 0;
                }
            }
            else if (coords.y > 0.01f)
            {
                posY--;
                if (posY < 0)
                {
                    posY = sizeY - 1;
                }
            }
            moveCursor(posX, posY);
        }
    }

    void ControlsGame.IUIActions.OnSubmit(InputAction.CallbackContext context)
    {
    }

    void ControlsGame.IUIActions.OnLeftClick(InputAction.CallbackContext context)
    {
    }

    void ControlsGame.IUIActions.OnRightClick(InputAction.CallbackContext context)
    {
    }

    void ControlsGame.IUIActions.OnMiddleClick(InputAction.CallbackContext context)
    {
    }

    void ControlsGame.IUIActions.OnScroll(InputAction.CallbackContext context)
    {
    }

    void ControlsGame.IUIActions.OnCancel(InputAction.CallbackContext context)
    {
    }

    void ControlsGame.IUIActions.OnPoint(InputAction.CallbackContext context)
    {
    }

    public void BackAction()
    {
        transitionScript.LoadSceneWithTransition("Menu");
    }
}


