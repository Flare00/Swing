using UnityEngine;
using UnityEngine.Localization;

public class FaintBall : SpecialBall
{
    private Ball[] _saveArray = new Ball[8];

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

    public override void Action(GameZone zone, int x, int y)
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
    }

    public override object Clone()
    {
        return new FaintBall();
    }
}
