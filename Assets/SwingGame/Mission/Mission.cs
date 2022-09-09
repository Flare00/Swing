using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Mission
{

    [SerializeField] public DummyBallMission[][] InitialPlayground;
    [SerializeField] public List<DummyBallMission>[] InitialPrediction;
    private List<Ball>[] _movingPrediction;

    public Mission() { }
    public Mission(DummyBallMission[][] initialPlayground, List<DummyBallMission>[] initialPrediction)
    {
        this.InitialPlayground = initialPlayground;
        this.InitialPrediction = initialPrediction;
        this._movingPrediction = new List<Ball>[initialPrediction.Length];

        this.reset();
    }

    public void reset()
    {
        for (int i = 0, max = this.InitialPrediction.Length; i < max; i++)
        {
            this._movingPrediction[i] = new List<Ball>();
            for (int j = 0, maxJ = this.InitialPrediction[i].Count; j < maxJ; j++)
            {
                this._movingPrediction[i].Add(DummyBallMission.DummyToBall(this.InitialPrediction[i][j]));
            }
        }
    }

    public Ball[][] GetBallPlayground()
    {
        Ball[][] res = new Ball[9][];
        for (int i = 0; i < 9; i++)
        {
            res[i] = new Ball[8];
            for (int j = 0; j < 8; j++)
            {
                res[i][j] = DummyBallMission.DummyToBall(this.InitialPlayground[i][j]);
            }
        }
        return res;
    }

    public Ball getNextPrediction(int column)
    {
        Ball res = null;
        if (this._movingPrediction[column].Count > 0)
        {
            res = this._movingPrediction[column][0];
            this._movingPrediction[column].RemoveAt(0);
        }
        return res;
    }
}
