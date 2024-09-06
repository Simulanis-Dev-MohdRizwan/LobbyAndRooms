using FishNet;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerM : NetworkBehaviour
{
    public bool connectedToServer;
    public int thisClientId;
    public NetworkObject thisNetworkObject;
    private void Start()
    {
        //base.OnStartClient();
        connectedToServer = true;
        thisNetworkObject = this.gameObject.GetComponent<NetworkObject>();
        thisClientId = thisNetworkObject.ObjectId;
        //ClientInfo.AddTolist?.Invoke(thisClientId, thisNetworkObject);
    }

    private void OnDestroy()
    {
        //ClientInfo.RemoveFromList?.Invoke(thisClientId, thisNetworkObject);
    }
    //public override void OnStopClient()
    //{
    //    ClientInfo.RemoveFromList?.Invoke(thisClientId, thisNetworkObject);
    //    //base.OnStopClient();
    //}

    

}
