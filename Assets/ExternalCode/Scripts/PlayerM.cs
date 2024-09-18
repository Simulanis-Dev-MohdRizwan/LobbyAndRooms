using FirstGearGames.LobbyAndWorld.Global.Canvases;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerM : NetworkBehaviour
{
    
    [SerializeField] GameObject ButtonContainer;
    [SerializeField] TextMeshProUGUI RoomNumbr;
    public int thisClientId;
    public NetworkObject thisNetworkObject;
    //public int RoomId;

    public int MyRoomId;
    public UnityEvent ifOwner;
    public UnityEvent ifNotOwner;
    [SerializeField] TextMeshProUGUI PlayerId;

    [Header("Message Canvas")]
    [SerializeField] TMP_InputField MessageBox;
    [SerializeField] Button SendMessageTmp;

    public int SelectedPlayer;

    public static Action OnPlayerJoined;
    //public static Action<int> OnPlayerLeft;
    private void Start()
    {
    }
    public override void OnStartClient()
    {
        base.OnStartClient();
        //PlayerListAndPrivateChat.addPlayerToList.Invoke(InstanceFinder.NetworkManager.ClientManager.Connection.ClientId);
        if (!base.IsOwner)
        {
            ButtonContainer.SetActive(false);
            MessageBox.transform.parent.gameObject.SetActive(false);
        }  
    }
    private void OnDisable()
    {
        SendMessageTmp.onClick.RemoveAllListeners();
        //OnPlayerLeft?.Invoke();
    }

    [ContextMenu("Play")]
    public void loadClient(int RoomId)
    {
        if (base.IsOwner)
        {
            RoomNumbr.text = RoomId.ToString();
            ButtonContainer.SetActive(false); 
            thisClientId = InstanceFinder.NetworkManager.ClientManager.Connection.ClientId;
            thisNetworkObject = this.GetComponent<NetworkObject>();
            LoadScene.instance.LoadRoom(LocalConnection, thisNetworkObject, RoomId);
            SendIDOnServer(thisClientId, RoomId);
            ChatFuntionality.GetRoomNumber(RoomId);
            ifOwner?.Invoke();
        }

        else if (!base.IsOwner)
        {
            ifNotOwner?.Invoke();
        }

        //PlayerListAndPrivateChat.addPlayerToList.Invoke(RoomId);

    }
    #region Player Id
    [ServerRpc(RequireOwnership =false,RunLocally =true)]
    public void SendIDOnServer(int playerID, int RoomId)
    {
        SetNameOnPlayer(playerID,RoomId);
    }
    [ObserversRpc(ExcludeOwner =false,BufferLast =true)]
    public void SetNameOnPlayer(int playerId,int RoomId)
    {
        PlayerId.text = playerId.ToString();
        MyRoomId = RoomId;
        //this.GetComponentInChildren<PlayerListAndPrivateChat>().thisRoomId = RoomId;
    }
    private void OnDestroy()
    {
        LoadScene.PlayerRemoved?.Invoke(LocalConnection);
    }
    #endregion

    #region Message Send Receive RPC's
    public void SendMessageFromPlayer()
    {
        string message = MessageBox.text;
        int myId = thisClientId;
        int roomId = MyRoomId;

        SendMessageToServer(message, myId,roomId,null);
        Debug.Log($" Send Message from {myId} and room {roomId} AND {this.MyRoomId} ");
    }

    [ServerRpc(RequireOwnership = false, RunLocally = true)]
    public void SendMessageToServer(string message, int MyId, int RoomId,NetworkConnection conn)
    {
        if (conn == null)
        {
            SendMessageToEveryone(message, MyId, RoomId);
            Debug.Log("MESSAGE RECEIVED ON SERVER : " + message);
        }

        else
        {
            //SendMessageToTarget
        }
    }

    [ObserversRpc(ExcludeOwner = false,BufferLast =true )]
    public void SendMessageToEveryone(string message, int SenderId, int RoomID)
    {
        Debug.Log($"MESSAGE RECEIVED ON OBSERVER: {message} from: {SenderId} Room: {RoomID}");
        string messge = $"{SenderId} : {message}";
        BroadCastMessage.ReceivedMessage?.Invoke(messge);
        Debug.Log("Success: " + message);

    }

    [TargetRpc]
    public void SendMessageToTarget(NetworkConnection conn)
    {

    }


    #endregion

}
