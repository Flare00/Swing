using System.Collections.Generic;
using SwingGame.Media;
using UnityEngine;

[System.Serializable]
public class GameData: ISerializationCallbackReceiver{

    [System.Serializable]
    public class BallValue{
        public int weight;
        public int material;
        public PuType type;

        public BallValue(){
            weight=-1;
            material=0;
        }
    }

    [System.Serializable]
    public class SerializableBallArray {
        
        [SerializeField] public BallValue[] balls;
        [SerializeField] public Ball testBall;
        public int i;

        public SerializableBallArray(int size){
            this.balls = new BallValue[size];
            for(int k=0;k<size;k++){
                this.balls[k] = new BallValue();
            }
            i=0;
        }

        public void setValue(int k, Ball value){
            if(value==null){
                this.balls[k].weight =-1;
            }
            if(value!=null){
                this.balls[k].weight = value.Weight;
                this.balls[k].material = value.IdMaterial;
                this.balls[k].type = value.Type;
            }
        }
        public BallValue getValue(int k){
            return this.balls[k];
        }
        /*
        public Ball this[int i]{
            set{
                ballsWeight[i] = value.Weight;
                ballsMaterial[i] = value.IdMaterial;
            }
        }*/

    }
    
    public void OnBeforeSerialize() {
        //Debug.Log(playgroundZoneBalls[0].testBall.IdMaterial);
    }
    public void OnAfterDeserialize() {}

    [SerializeField] public SerializableBallArray[] playgroundZoneBalls;
    [SerializeField] public SerializableBallArray[] predictionZoneBalls;

    public ulong score;
    public int multiplicator;
    public int level;
    public int ballBeforeLvUp;
    public int countPowerUp;
    public int nbBallDrop;
    public float time;
    public int[] swingState;

    public GameData(){
        //Debug.Log(typeof(NormalBall).IsSerializable);
        predictionZoneBalls = new SerializableBallArray[GameZone.HeightPrediction];
        playgroundZoneBalls = new SerializableBallArray[GameZone.HeightPlayGround];
        swingState = new int[GameZone.NbSwings];
        for (int i = 0; i < GameZone.HeightPrediction; i++)
        {
            predictionZoneBalls[i] = new SerializableBallArray(GameZone.LengthPlayGround);
        }
        for (int i = 0; i < GameZone.HeightPlayGround; i++)
        {
           playgroundZoneBalls[i] = new SerializableBallArray(GameZone.LengthPlayGround);
        }

    
    }
}