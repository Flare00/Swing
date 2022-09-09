using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DummyBallMission
{
    [SerializeField] public int Weight;
    [SerializeField] public int Type;

    public static Ball DummyToBall(DummyBallMission db)
    {
        Ball res = null;
        if (db != null)
        {
            switch (db.Type)
            {
                case 1000:
                    res = new JokerBall();
                    break;
                case 1001:
                    res = new BombBall();
                    break;
                case 1002:
                    res = new StarBall();
                    break;
                case 1003:
                    res = new GoldenStarBall();
                    break;
                case 1004:
                    res = new StingBall();
                    break;
                case 1005:
                    res = new ZapHorizontalBall();
                    break;
                case 1006:
                    res = new ZapDiagonalBall();
                    break;
                case 1007:
                    res = new CutterBall();
                    break;
                default:
                    res = new NormalBall(db.Weight, db.Type);
                    break;
            }
        }
        return res;
    }

    public static DummyBallMission BallToDummy(Ball b)
    {
        DummyBallMission res = new DummyBallMission();
        res.Type = b.IdMaterial;
        res.Weight = b.Weight;
        return res;
    }

}

