using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class FaintBall : SpecialBall
{
    private class BallSave
    {
        public Ball ball;
        public bool stay;
        public BallSave(Ball ball, bool stay = true)
        {
            this.ball = ball;
            this.stay = stay;
        }
    }
    private List<BallSave> _savedBalls = new List<BallSave>();
    //private Ball[] _saveArray = new Ball[8];

    public FaintBall(bool tooltip = true) : base()
    {
        this.BallObject = GameObject.Instantiate(Resources.Load("Prefabs/PU_Faint", typeof(GameObject))) as GameObject;
        this.type = PuType.FaintType;

        LocalizedString header = new LocalizedString("PowerUp", "faint_h");
        LocalizedString content = new LocalizedString("PowerUp", "faint_c");

        if (tooltip)
        {
            this.setTooltip(header.GetLocalizedString(), content.GetLocalizedString());
        }

    }
    //Old Action
    /*public override void Action(GameZone zone, int x, int y)
    {
        int count = 0;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (!(i == 0 && j == 0))
                {
                    if (x + i >= 0 && x + i < GameZone.LengthPlayGround)
                    {
                        if (y + j >= 0 && y + j < GameZone.HeightPlayGround)
                        {
                            if (zone.Playground[y + j][x + i].HasBall())
                            {
                                Ball b = zone.Playground[y + j][x + i].Ball;
                                if (b.BallObject != null)
                                {
                                    if (_saveArray[count] == null)
                                    {
                                        _saveArray[count] = b;
                                    }
                                    else if (_saveArray[count] != b)
                                    {
                                        if (_saveArray[count].BallObject != null)
                                        {
                                            _saveArray[count].SetHideBall(false);
                                        }
                                        _saveArray[count] = b;
                                    }
                                    _saveArray[count].SetHideBall(true);
                                }
                            }
                            else
                            {
                                if (_saveArray[count] != null)
                                {
                                    if (_saveArray[count].BallObject != null)
                                    {
                                        _saveArray[count].SetHideBall(false);
                                    }
                                    _saveArray[count] = null;
                                }
                            }
                        }
                    }
                    count++;
                }
            }
        }
    }*/

    public override void Action(GameZone zone, int x, int y)
    {
        for (int i = 0; i < _savedBalls.Count; i++)
        {
            _savedBalls[i].stay = false;
        }
        for (int i = -1; i <= 1; i++)
        {
            if (x + i >= 0 && x + i < GameZone.LengthPlayGround)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if ((!(i == 0 && j == 0)) && (y + j >= 0 && y + j < GameZone.HeightPlayGround))
                    {
                        if (zone.Playground[y + j][x + i].HasBall())
                        {
                            Ball b = zone.Playground[y + j][x + i].Ball;
                            bool found = false;
                            for (int k = 0, max = _savedBalls.Count; k < max && !found; k++)
                            {
                                if (b == _savedBalls[k].ball)
                                {
                                    _savedBalls[k].stay = true;
                                    found = true;
                                }
                            }
                            if (!found)
                            {
                                if (b.BallObject != null)
                                {
                                    b.SetHideBall(true);
                                    _savedBalls.Add(new BallSave(b));
                                }
                            }
                        }
                    }
                }
            }
        }

        int it = 0;
        while (it < _savedBalls.Count)
        {
            if (_savedBalls[it].stay)
            {
                it++;
            }
            else
            {
                if (_savedBalls[it].ball != null)
                    if (_savedBalls[it].ball.BallObject != null)
                        _savedBalls[it].ball.SetHideBall(false);
                _savedBalls.RemoveAt(it);
            }
        }
    }

    public override void ActionOnSwing(GameZone zone, int x, int y)
    {
        foreach (BallSave b in _savedBalls)
        {
            if (b.ball != null)
            {
                if (b.ball.BallObject != null)
                {
                    b.ball.SetHideBall(false);
                }
            }
        }
        _savedBalls.Clear();

    }

    public override object Clone()
    {
        return new FaintBall();
    }
}
