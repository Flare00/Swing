using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerSystem
{
    private static MultiplayerSystem INSTANCE;

    public static void CreateInstance(bool coop)
    {
        if (INSTANCE != null)
        {
            MultiplayerSystem.RemoveInstance();
        }
        INSTANCE = new MultiplayerSystem(coop);
    }

    public static void RemoveInstance()
    {
        if (INSTANCE != null)
        {
            INSTANCE.ClearPlayers();
        }
        INSTANCE = null;
    }

    public static MultiplayerSystem getInstance()
    {
        return INSTANCE;
    }

    protected MultiplayerSystem(bool coop)
    {
        this.players = new List<IMultiplayerInterface>();
        this.coop = coop;
    }


    //Multiplayer Local ONLY for now
    private bool coop = false;
    private List<IMultiplayerInterface> players;

    public void SubscribePlayer(IMultiplayerInterface player)
    {
        if (players.FindIndex(p => p.PlayerId() == player.PlayerId()) < 0)
        {
            players.Add(player);
        }
    }

    public void UnsubscribePlayer(IMultiplayerInterface player)
    {
        int id = players.FindIndex(p => p.PlayerId() == player.PlayerId());
        if (id >= 0)
        {
            players.RemoveAt(id);
        }
    }

    public void ClearPlayers()
    {
        players.Clear();
    }

    public bool SendData(MultiplayerData data)
    {
        int id = players.FindIndex(p => p.PlayerId() == data.Sender.PlayerId());
        if(id >= 0)
        {
            if (coop) //Edit Sended Ball if Coop or versus mode.
            {
                data.Data.TransformBall();
            } 
            else
            {
                data.Data.TransformBallVersus();
            }

            if(id == players.Count -1 )
            {
                players[0].ReceiveData(data);
            }
            else
            {
                players[id + 1].ReceiveData(data);
            }
            return true;
        }
        return false;
    }

    public void SendGameOver(int idPlayer)
    {
        if (coop)
        {
            foreach (IMultiplayerInterface imi in players)
            {
                if (imi.PlayerId() != idPlayer)
                    imi.ReceiveGameOver();
            }
        }
        
    }

    public bool IsCoop() { return coop; }


}
