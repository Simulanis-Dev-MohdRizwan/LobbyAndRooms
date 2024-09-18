using FishNet.Object;
using FishNet.Managing.Scened;
using FishNet;
using FishNet.Connection;
using UnityEngine;
using FirstGearGames.LobbyAndWorld.Global.Canvases;
using System.Globalization;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using FishNet.Object.Synchronizing;
using FishNet.CodeGenerating;
public class LoadScene : NetworkBehaviour
{
    public static LoadScene instance;
    public List<Scene> SceneLoaded = new List<Scene>();
 
    [AllowMutableSyncType] public SyncDictionary<int, SceneLoadData> SceneData = new();
    [AllowMutableSyncType] public SyncDictionary<int, ClientInformations> clientinfo = new();

    public static Action<ClientInformations> PlayerSpawned;
    public static Action<NetworkConnection> PlayerRemoved;


    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        InstanceFinder.SceneManager.OnLoadEnd += RegisterScene;
        PlayerSpawned += PlayerAddedToList;
        PlayerRemoved += PlayerRemovedFromList;
    }

    private void OnDisable()
    {
        InstanceFinder.SceneManager.OnLoadEnd -= RegisterScene;
        PlayerSpawned -= PlayerAddedToList;
        PlayerRemoved -= PlayerRemovedFromList;
    }
    private void PlayerRemovedFromList(NetworkConnection informations)
    {
        if (clientinfo.ContainsKey(informations.ClientId))
        {
            clientinfo.Remove(informations.ClientId);
            Debug.Log($"player {informations.ClientId} removed");
        }
    }

    private void PlayerAddedToList(ClientInformations informations)
    {
        clientinfo.Add(informations._playerId, informations);
        Debug.Log($"Player {informations._playerId} added in room {informations._roomId}" );
    }

    private void RegisterScene(SceneLoadEndEventArgs args)
    {
        if (!base.IsServerInitialized) return;
        foreach(var scene in args.LoadedScenes)
        {
            SceneLoaded.Add(scene);
            Debug.Log("Scene added in list");
            GameObject[] roomData = scene.GetRootGameObjects();
            foreach (GameObject room in roomData)
            {
                if(room.name == "PlayerList")
                {
                   var  _playerListAndPrivateChat = room.GetComponent<PlayerListAndPrivateChat>();
                   _playerListAndPrivateChat.thisRoomId = CreatedRoomId;
                   _playerListAndPrivateChat.SetRoomId(CreatedRoomId);
                }
            }
            Debug.Log("find the GameObject");
            //roomData.GetComponent<PlayerListAndPrivateChat>().thisRoomId = CreatedRoomId;
        }
    }
    private int CreatedRoomId;
    public void LoadRoom(NetworkConnection connection,NetworkObject player,int roomId)
    {
        LoadSceneOnServer(connection, player,roomId);
    }
    SceneLoadData sld;
    [ServerRpc(RequireOwnership = false,RunLocally = false)]
    public void LoadSceneOnServer(NetworkConnection conn,NetworkObject player,int RoomId)
    {
        if (!SceneData.ContainsKey(RoomId))
        {
            SceneLookupData sceneLookupData = new SceneLookupData("ExternalCode/Scenes/Game");
            sld = new SceneLoadData(sceneLookupData);
            sld.Options.AllowStacking = true;
            sld.MovedNetworkObjects = new[] { player };
            InstanceFinder.SceneManager.LoadConnectionScenes(conn, sld);
            Debug.Log("new room" + RoomId);
            SceneData.Add(RoomId,sld);
            CreatedRoomId = RoomId;
            //PlayerListAndPrivateChat.SetThisRoomNumber?.Invoke(RoomId);
        }

        else
        {
            sld = SceneData[RoomId];
            sld.Options.AllowStacking = false;
            sld.MovedNetworkObjects = new[] { player };
            InstanceFinder.SceneManager.LoadConnectionScenes(conn, sld);
            Debug.Log("old room" + RoomId);
        }
        PlayerAddedToList(new ClientInformations(conn, conn.ClientId, RoomId));
        Debug.Log($"scene loading successfull for {conn.ClientId} ");
    }

}

[System.Serializable]
public class ClientInformations
{
    public NetworkConnection Connection;
    public int _playerId;
    public int _roomId;

    public ClientInformations() { }
    public ClientInformations(NetworkConnection conn, int playerId , int roomId )
    {
        Connection = conn;
        _playerId = playerId;
        _roomId = roomId;
    }
    
}



