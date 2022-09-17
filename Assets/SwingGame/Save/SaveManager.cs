using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour {
    public static SaveManager instance {get; private set; }
    private GameData gameData;
    private void Awake(){
        if(instance!=null){
            Debug.LogError("Trying to create a new savemanager");
        }
        instance = this;
    }

    public void NewGame(){
        this.gameData = new GameData();
    }

    public void LoadGame(){
        if (this.gameData==null){
            Debug.Log("No data found");
            NewGame();
        }
        //TODO read files and update game
    }

    public void updateData(GameZone gz,GameState gs){
        this.gameData.score = gs.Score;
        this.gameData.multiplicator = gs.Multiplicator;
        this.gameData.level = gs.Level;
        this.gameData.ballBeforeLvUp = gs.NbBallBeforeLevelUp;
        this.gameData.countPowerUp = gs.CountPowerUp;
        this.gameData.time = gs.Time;

        for(int i=0;i<GameZone.HeightPlayGround;i++){
            for(int j=0;j<GameZone.LengthPlayGround;j++){
                gameData.playgroundZoneBalls[i][j] = gz.Playground[i][j].Ball;
            }
        }
        for(int i=0;i<GameZone.HeightPrediction;i++){
            for(int j=0;j<GameZone.LengthPlayGround;j++){
                gameData.predictionZoneBalls[i][j] = gz.Prediction[i][j].Ball;
            }
        }

    }

    public void SaveGame(){

        //TODO write on a file

    }

    private void OnApplicationQuit(){
        SaveGame();
    }

}