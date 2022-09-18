using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMultiplayerInterface 
{
    public int PlayerId();
    public void ReceiveData(MultiplayerData data);
}
