using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwingInput : ControlsGame.IPlayer1Actions, ControlsGame.IPlayer2Actions
{
    private class GlobalInputState
    {
        public bool pause = false;
        public List<Game.Action> GetActions()
        {
            List<Game.Action> actions = new List<Game.Action>();
            if (pause)
            {
                actions.Add(Game.Action.Pause);
                pause = false;
            }
            return actions;
        }
    }
    private class PlayerInputState
    {
        private const float TEMPO_VALUE = 0.2f;
        public bool left = false;
        public bool right = false;
        public bool drop = false;
        private float tLeft = 0.0f;
        private float tRight = 0.0f;
        private float tDrop = 0.0f;
        public List<Game.Action> GetActions()
        {
            if (tLeft > 0.0f)
                tLeft -= Time.deltaTime;
            if (tRight > 0.0f)
                tRight -= Time.deltaTime;
            if (tDrop > 0.0f)
                tDrop -= Time.deltaTime;

            List<Game.Action> actions = new List<Game.Action>();
            if (left && tLeft <= 0.0f)
            {
                tLeft = TEMPO_VALUE;
                actions.Add(Game.Action.Left);
            }
            else if (!left)
                tLeft = 0.0f;
            if (right && tRight <= 0.0f)
            {
                tRight = TEMPO_VALUE;
                actions.Add(Game.Action.Right);
            }
            else if (!right)
                tRight = 0.0f;
            if (drop && tDrop <= 0.0f)
            {
                tDrop = TEMPO_VALUE;
                actions.Add(Game.Action.Drop);
            }
            else if (!drop)
                tDrop = 0.0f;
            return actions;
        }
    }

    private InputDevice _p1Controller = null;
    private InputDevice _p2Controller = null;

    private GlobalInputState _global;
    private PlayerInputState _stateP1;
    private PlayerInputState _stateP2;


    public SwingInput()
    {
        _p1Controller = null;
        _p2Controller = null;

        ControlsGame cg = new ControlsGame();
        cg.Player1.SetCallbacks(this);
        cg.Player1.Enable();
        cg.Player2.SetCallbacks(this);
        cg.Player2.Enable();

        _stateP1 = new PlayerInputState();
        _stateP2 = new PlayerInputState();
        _global = new GlobalInputState();
    }

    public List<Game.Action> GetPlayerActions(int player)
    {
        if (player == 0)
        {
            return _stateP1.GetActions();
        }
        else
        {
            return _stateP2.GetActions();
        }
    }

    public List<Game.Action> GetGlobalActions()
    {
        return _global.GetActions();
    }





    // --- Drop ---
    void ControlsGame.IPlayer1Actions.OnDrop(InputAction.CallbackContext context)
    {
        bool same = true;
        if (context.control.device.deviceId > 10)
        {

            if (_p1Controller == null && context.control.device != _p2Controller)
            {
                _p1Controller = context.control.device;
            }
            else if (_p1Controller != context.control.device)
            {
                same = false;
            }
        }

        if (context.started && same)
        {
            _stateP1.drop = true;
        }
        else if (context.canceled && same)
        {
            _stateP1.drop = false;
        }
    }

    void ControlsGame.IPlayer2Actions.OnDrop(InputAction.CallbackContext context)
    {
        bool same = true;
        if (context.control.device.deviceId > 10)
        {

            if (_p2Controller == null && context.control.device != _p1Controller)
            {
                _p2Controller = context.control.device;
            }
            else if (_p2Controller != context.control.device)
            {
                same = false;
            }
        }


        if (context.started && same)
        {
            _stateP2.drop = true;
        }
        else if (context.canceled && same )
        {
            _stateP2.drop = false;
        }
    }

    // --- Left ---
    void ControlsGame.IPlayer1Actions.OnLeft(InputAction.CallbackContext context)
    {
        bool same = true;
        if (context.control.device.deviceId > 10)
        {

            if (_p1Controller == null && context.control.device != _p2Controller)
            {
                _p1Controller = context.control.device;
            }
            else if (_p1Controller != context.control.device)
            {
                same = false;
            }
        }

        if (context.started && same)
        {
            _stateP1.left = true;
        }
        else if (context.canceled && same)
        {
            _stateP1.left = false;
        }
    }

    void ControlsGame.IPlayer2Actions.OnLeft(InputAction.CallbackContext context)
    {
        bool same = true;
        if (context.control.device.deviceId > 10)
        {

            if (_p2Controller == null && context.control.device != _p1Controller)
            {
                _p2Controller = context.control.device;
            }
            else if (_p2Controller != context.control.device)
            {
                same = false;
            }
        }


        if (context.started && same)
        {
            _stateP2.left = true;
        }
        else if (context.canceled && same)
        {
            _stateP2.left = false;
        }
    }

    // --- Right ---
    void ControlsGame.IPlayer1Actions.OnRight(InputAction.CallbackContext context)
    {
        bool same = true;
        if (context.control.device.deviceId > 10)
        {

            if (_p1Controller == null && context.control.device != _p2Controller)
            {
                _p1Controller = context.control.device;
            }
            else if (_p1Controller != context.control.device)
            {
                same = false;
            }
        }

        if (context.started && same)
        {
            _stateP1.right = true;
        }
        else if (context.canceled && same)
        {
            _stateP1.right = false;
        }
    }
    void ControlsGame.IPlayer2Actions.OnRight(InputAction.CallbackContext context)
    {
        bool same = true;
        if (context.control.device.deviceId > 10)
        {

            if (_p2Controller == null && context.control.device != _p1Controller)
            {
                _p2Controller = context.control.device;
            }
            else if (_p2Controller != context.control.device)
            {
                same = false;
            }
        }

        if (context.started && same)
        {
            _stateP2.right = true;
        }
        else if (context.canceled && same)
        {
            _stateP2.right = false;
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _global.pause = true;
        }
    }

    public void OnUp(InputAction.CallbackContext context)
    {
    }

    public void OnDown(InputAction.CallbackContext context)
    {
    }
}
