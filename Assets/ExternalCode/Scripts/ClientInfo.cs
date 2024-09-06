using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Transporting;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientInfo : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;
    public int thisId;
    public int anotherId;
    public GameObject SceneManger;

    private void Start()
    {
        networkManager = this.GetComponent<NetworkManager>();
        //networkManager.ServerManager.OnRemoteConnectionState += RemoteConnectionChanged;
        InstanceFinder.ClientManager.OnClientConnectionState += ClientConnectionChanged;
      
    }

    private void ClientConnectionChanged(ClientConnectionStateArgs args)
    {
        if (args.ConnectionState == LocalConnectionState.Started)
        {

        }

    }

    private void RemoteConnectionChanged(NetworkConnection connection, RemoteConnectionStateArgs args)
    {
        if (args.ConnectionState == RemoteConnectionState.Started)
        {
            //thisId = connection.ClientId;
            //anotherId = args.ConnectionId;
        }
     
    }

    private void OnDisable()
    {
        networkManager.ClientManager.OnClientConnectionState -= ClientConnectionChanged;
    }
}


