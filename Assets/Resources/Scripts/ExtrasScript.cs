using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class ExtrasScript : MonoBehaviour, ControlsGame.IUIActions
{

    public GameObject panelPU;
    public GameObject videoPlayer;
    public GameObject textHeader;
    public GameObject textContent;
    public TransitionScript transitionScript;

    private GameObject ring;
    private int posX = 0;
    private int posY = 0;

    private int sizeX = 5;
    private int sizeY = 5;

    public float offsetX;
    public float offsetY;

    void Start()
    {
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
                        ball = new JokerBall();
                        break;
                    case 2:
                        ball = new BombBall();
                        break;
                    case 3:
                        ball = new CutterBall();
                        break;
                    case 4:
                        ball = new ZapHorizontalBall();
                        break;
                    case 5:
                        ball = new ZapDiagonalBall();
                        break;
                    case 6:
                        ball = new CopyLineBall();
                        break;
                    case 7:
                        ball = new CopySquareBall();
                        break;
                    case 8:
                        ball = new CopyPredictionBall();
                        break;
                    case 9:
                        ball = new StarBall();
                        break;
                    case 10:
                        ball = new GoldenStarBall();
                        break;
                    case 11:
                        ball = new PlasmaNoTriangleBall();
                        break;
                    case 12:
                        ball = new PlasmaEmptyTriangleBall();
                        break;
                    case 13:
                        ball = new PlasmaFullTriangleBall();
                        break;
                    case 14:
                        ball = new BrickBall();
                        break;
                    case 15:
                        ball = new BrickSquare();
                        break;
                    case 16:
                        ball = new TransformJokerBall();
                        break;
                    case 17:
                        ball = new TransformBombBall();
                        break;
                    case 18:
                        ball = new TransformDestroyBall();
                        break;
                    case 19:
                        ball = new TransformBrickBall();
                        break;
                    case 20:
                        ball = new BrickTower();
                        break;
                    case 21:
                        ball = new StingBall();
                        break;
                    case 22:
                        ball = new JokerBall(); // Tornado
                        break;
                    case 23:
                        ball = new JokerBall(); // Blackout
                        break;
                    case 24:
                        ball = new JokerBall(); // Random
                        break;
                    case 25:
                        ball = new JokerBall(); // ?
                        break;
                    default:
                        ball = new BombBall();
                        break;
                }
                ball.BallObject.transform.parent = panelPU.transform;
                ball.BallObject.transform.position = new Vector3(j * 1.5f + offsetX, (sizeY - i) * 1.5f + offsetY, 0);
            }
        }
        ring = GameObject.Instantiate(Resources.Load("Prefabs/Extras/Circle", typeof(GameObject))) as GameObject;
        ring.transform.parent = panelPU.transform;

        moveCursor(posX, posY); // Set first hover on Joker
    }

    void moveCursor(int x, int y)
    {
        ring.transform.position = new Vector3(x * 1.5f + offsetX, (sizeY - y) * 1.5f + offsetY, 0);
        LocalizedString header;
        LocalizedString content;

        switch ((x + y * sizeX) + 1)
        {
            case 1:
                // ball = new JokerBall();
                videoPlayer.gameObject.GetComponent<RawImage>().texture = Resources.Load("Videos/TextureStack", typeof(Texture)) as Texture;
                header = new LocalizedString("PowerUp", "joker_h");
                content = new LocalizedString("PowerUp", "joker_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 2:
                // ball = new BombBall();
                videoPlayer.gameObject.GetComponent<RawImage>().texture = Resources.Load("Videos/TextureStack", typeof(Texture)) as Texture;
                header = new LocalizedString("PowerUp", "bomb_h");
                content = new LocalizedString("PowerUp", "bomb_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 3:
                // ball = new CutterBall();
                videoPlayer.gameObject.GetComponent<RawImage>().texture = Resources.Load("Videos/TextureStack", typeof(Texture)) as Texture;
                header = new LocalizedString("PowerUp", "cutter_h");
                content = new LocalizedString("PowerUp", "cutter_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 4:
                // ball = new ZapHorizontalBall();
                videoPlayer.gameObject.GetComponent<RawImage>().texture = Resources.Load("Videos/TextureStack", typeof(Texture)) as Texture;
                header = new LocalizedString("PowerUp", "zapH_h");
                content = new LocalizedString("PowerUp", "zapH_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 5:
                // ball = new ZapDiagonalBall();
                videoPlayer.gameObject.GetComponent<RawImage>().texture = Resources.Load("Videos/TextureStack", typeof(Texture)) as Texture;
                header = new LocalizedString("PowerUp", "zapD_h");
                content = new LocalizedString("PowerUp", "zapD_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 6:
                // ball = new CopyLineBall();
                videoPlayer.gameObject.GetComponent<RawImage>().texture = Resources.Load("Videos/TextureStack", typeof(Texture)) as Texture;
                header = new LocalizedString("PowerUp", "copyLine_h");
                content = new LocalizedString("PowerUp", "copyLine_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 7:
                // ball = new CopySquareBall();
                videoPlayer.gameObject.GetComponent<RawImage>().texture = Resources.Load("Videos/TextureStack", typeof(Texture)) as Texture;
                header = new LocalizedString("PowerUp", "copySquare_h");
                content = new LocalizedString("PowerUp", "copySquare_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 8:
                // ball = new CopyPredictionBall();
                videoPlayer.gameObject.GetComponent<RawImage>().texture = Resources.Load("Videos/TextureStack", typeof(Texture)) as Texture;
                header = new LocalizedString("PowerUp", "copyPrediction_h");
                content = new LocalizedString("PowerUp", "copyPrediction_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 9:
                // ball = new StarBall();
                videoPlayer.gameObject.GetComponent<RawImage>().texture = Resources.Load("Videos/TextureStack", typeof(Texture)) as Texture;
                header = new LocalizedString("PowerUp", "star_h");
                content = new LocalizedString("PowerUp", "star_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 10:
                // ball = new GoldenStarBall();
                videoPlayer.gameObject.GetComponent<RawImage>().texture = Resources.Load("Videos/TextureStack", typeof(Texture)) as Texture;
                header = new LocalizedString("PowerUp", "starGold_h");
                content = new LocalizedString("PowerUp", "starGold_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 11:
                // ball = new PlasmaNoTriangleBall();
                videoPlayer.gameObject.GetComponent<RawImage>().texture = Resources.Load("Videos/TextureStack", typeof(Texture)) as Texture;
                header = new LocalizedString("PowerUp", "flash_h");
                content = new LocalizedString("PowerUp", "flash_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 12:
                // ball = new PlasmaEmptyTriangleBall();
                videoPlayer.gameObject.GetComponent<RawImage>().texture = Resources.Load("Videos/TextureStack", typeof(Texture)) as Texture;
                header = new LocalizedString("PowerUp", "flashD_h");
                content = new LocalizedString("PowerUp", "flashD_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 13:
                // ball = new PlasmaFullTriangleBall();
                videoPlayer.gameObject.GetComponent<RawImage>().texture = Resources.Load("Videos/TextureStack", typeof(Texture)) as Texture;
                header = new LocalizedString("PowerUp", "flashT_h");
                content = new LocalizedString("PowerUp", "flashT_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 14:
                // ball = new BrickBall();
                videoPlayer.gameObject.GetComponent<RawImage>().texture = Resources.Load("Videos/TextureStack", typeof(Texture)) as Texture;
                header = new LocalizedString("PowerUp", "brick_h");
                content = new LocalizedString("PowerUp", "brick_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 15:
                // ball = new BrickSquare();
                videoPlayer.gameObject.GetComponent<RawImage>().texture = Resources.Load("Videos/TextureStack", typeof(Texture)) as Texture;
                header = new LocalizedString("PowerUp", "brickSquare_h");
                content = new LocalizedString("PowerUp", "brickSquare_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 16:
                // ball = new TransformJokerBall();
                videoPlayer.gameObject.GetComponent<RawImage>().texture = Resources.Load("Videos/TextureStack", typeof(Texture)) as Texture;
                header = new LocalizedString("PowerUp", "jokerTransform_h");
                content = new LocalizedString("PowerUp", "jokerTransform_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 17:
                // ball = new TransformBombBall();
                videoPlayer.gameObject.GetComponent<RawImage>().texture = Resources.Load("Videos/TextureStack", typeof(Texture)) as Texture;
                header = new LocalizedString("PowerUp", "bombTransform_h");
                content = new LocalizedString("PowerUp", "bombTransform_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 18:
                // ball = new TransformDestroyBall();
                videoPlayer.gameObject.GetComponent<RawImage>().texture = Resources.Load("Videos/TextureStack", typeof(Texture)) as Texture;
                header = new LocalizedString("PowerUp", "destroyTransform_h");
                content = new LocalizedString("PowerUp", "destroyTransform_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 19:
                // ball = new TransformBrickBall();
                videoPlayer.gameObject.GetComponent<RawImage>().texture = Resources.Load("Videos/TextureStack", typeof(Texture)) as Texture;
                header = new LocalizedString("PowerUp", "brickTransform_h");
                content = new LocalizedString("PowerUp", "brickTransform_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 20:
                // ball = new BrickTower();
                videoPlayer.gameObject.GetComponent<RawImage>().texture = Resources.Load("Videos/TextureStack", typeof(Texture)) as Texture;
                header = new LocalizedString("PowerUp", "tower_h");
                content = new LocalizedString("PowerUp", "tower_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 21:
                // ball = new StingBall();
                videoPlayer.gameObject.GetComponent<RawImage>().texture = Resources.Load("Videos/TextureStack", typeof(Texture)) as Texture;
                header = new LocalizedString("PowerUp", "sting_h");
                content = new LocalizedString("PowerUp", "sting_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 22:
                // Tornado
                videoPlayer.gameObject.GetComponent<RawImage>().texture = Resources.Load("Videos/TextureStack", typeof(Texture)) as Texture;
                header = new LocalizedString("PowerUp", "joker_h");
                content = new LocalizedString("PowerUp", "joker_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 23:
                // Blackout
                videoPlayer.gameObject.GetComponent<RawImage>().texture = Resources.Load("Videos/TextureStack", typeof(Texture)) as Texture;
                header = new LocalizedString("PowerUp", "joker_h");
                content = new LocalizedString("PowerUp", "joker_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 24:
                // Random
                videoPlayer.gameObject.GetComponent<RawImage>().texture = Resources.Load("Videos/TextureStack", typeof(Texture)) as Texture;
                header = new LocalizedString("PowerUp", "joker_h");
                content = new LocalizedString("PowerUp", "joker_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            case 25:
                // ?
                videoPlayer.gameObject.GetComponent<RawImage>().texture = Resources.Load("Videos/TextureStack", typeof(Texture)) as Texture;
                header = new LocalizedString("PowerUp", "joker_h");
                content = new LocalizedString("PowerUp", "joker_c");
                textHeader.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = header.GetLocalizedString();
                textContent.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = content.GetLocalizedString();
                break;
            default:
                // ball = new BombBall();
                videoPlayer.gameObject.GetComponent<RawImage>().texture = Resources.Load("Videos/TextureStack", typeof(Texture)) as Texture;
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
        Debug.Log(context.ReadValue<Vector2>());
    }

    public void BackAction()
    {
        transitionScript.LoadSceneWithTransition("Menu");
    }
}


