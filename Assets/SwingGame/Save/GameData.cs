using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData{
    public Ball[][] playgroundZoneBalls;
    public Ball[][] predictionZoneBalls;

    public ulong score;
    public int multiplicator;
    public int level;
    public int ballBeforeLvUp;
    public int countPowerUp;
    public float time;

    public GameData(){
        predictionZoneBalls = new Ball[GameZone.HeightPrediction][];
        playgroundZoneBalls = new Ball[GameZone.HeightPlayGround][];
        for (int i = 0; i < GameZone.HeightPrediction; i++)
        {
            predictionZoneBalls[i] = new Ball[GameZone.LengthPlayGround];
        }
        for (int i = 0; i < GameZone.HeightPlayGround; i++)
        {
            playgroundZoneBalls[i] = new Ball[GameZone.LengthPlayGround];
        }
    }
}