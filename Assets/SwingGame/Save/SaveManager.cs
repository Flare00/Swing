using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour {

    public static SaveManager instance {get; private set; }
    public GameData gameData;
    private FileDataHandler fileHandler;

    
    private void Awake(){
        if(instance!=null){
            Debug.LogError("Trying to create a new savemanager");
        }
        instance = this;
        fileHandler = new FileDataHandler(Application.persistentDataPath,"savedGame.txt",false);
        NewGame();
        Debug.Log(Application.persistentDataPath);
    }

    public void NewGame(){
        this.gameData = new GameData();
    }

    public void LoadGame(){
        
        gameData = fileHandler.Load();
        if (this.gameData==null){
            Debug.Log("No data found");
            NewGame();
        }
    }

    public void UpdateData(GameZone gz,GameState gs){
        this.gameData.score = gs.Score;
        this.gameData.multiplicator = gs.Multiplicator;
        this.gameData.level = gs.Level;
        this.gameData.ballBeforeLvUp = gs.NbBallBeforeLevelUp;
        this.gameData.nbBallDrop = gs.NbBallDrop;
        this.gameData.countPowerUp = gs.CountPowerUp;
        this.gameData.time = gs.Time;
        
        for(int i=0;i<GameZone.HeightPlayGround;i++){
            for(int j=0;j<GameZone.LengthPlayGround;j++){
                gameData.playgroundZoneBalls[i].setValue(j,gz.Playground[i][j].Ball);
            }
        }
        for(int i=0;i<GameZone.HeightPrediction;i++){
            for(int j=0;j<GameZone.LengthPlayGround;j++){
                gameData.predictionZoneBalls[i].setValue(j, gz.Prediction[i][j].Ball);
            }
        }
        for(int i=0;i<GameZone.NbSwings;i++){
            gameData.swingState[i] = gz.Swings[i].State;
        }

        
        //gameData.playgroundZoneBalls[0].testBall = new NormalBall(1,1);

    }

    public void SaveGame(){
        fileHandler.Save(gameData);

    }

    private void OnApplicationQuit(){
        SaveGame();
    }

}