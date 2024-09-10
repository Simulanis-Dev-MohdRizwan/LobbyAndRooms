using FishNet.Connection;
using FishNet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ToolScript 
{
    public static NetworkConnection GetConnFromID(int clientId)
    {
        if (InstanceFinder.NetworkManager.ServerManager.Clients.ContainsKey(clientId))
        {
            Debug.Log($"the id {clientId} exist in dictionary");
            return InstanceFinder.NetworkManager.ServerManager.Clients[(int)clientId];
        }
        else
        {
            Debug.Log($"the id {clientId} does not exixt in dictionary");
            return null;
        }
    }
}
