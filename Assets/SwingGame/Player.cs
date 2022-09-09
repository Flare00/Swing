using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BallContainer
{
    private GameState _gameState;
    private int _position;
    private Animator _animator;

    public Player(GameState gs)
    {
        _gameState = gs;
        //ContainerObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject.Destroy(ContainerObject);
        ContainerObject = GameObject.Instantiate(Resources.Load("Prefabs/PlayerPrefab", typeof(GameObject))) as GameObject;
        ContainerObject.name = "PlayerContainer";
        _animator = ContainerObject.GetComponentInChildren<Animator>();

        /* Material m = base.ContainerObject.GetComponent<Renderer>().material;
         m.color = new Color(1,1,1,0.5f);

         m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
         m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
         m.SetInt("_ZWrite", 0);
         m.DisableKeyword("_ALPHATEST_ON");
         m.DisableKeyword("_ALPHABLEND_ON");
         m.EnableKeyword("_ALPHAPREMULTIPLY_ON");
         m.renderQueue = 3000;*/

    }
    public Player(GameState gs, Ball b) : base(b)
    {
        _gameState = gs;
    }

    public void MoveRight()
    {
        if(_position < 7)
        {
            _position++;
        } 
        else
        {
            _position = 7;
        }
        ContainerObject.transform.localPosition = new Vector3(_position * GameZone.SpacingBall, 0 , 0); 
    }

    public void MoveLeft()
    {
        if (_position > 0)
        {
            _position--;
        }
        else
        {
            _position = 0;
        }
        ContainerObject.transform.localPosition = new Vector3(_position * GameZone.SpacingBall, 0, 0);
    }

    public void DropAnim()
    {
        _animator.Play("DropBall_1", 0, 0f);
        _animator.Play("DropBall_2", 1, 0f);
    }

    public GameState GameState { get => _gameState; set => _gameState = value; }
    public int Position { get => _position; set => _position = value; }
}
