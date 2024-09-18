using FishNet;
using FishNet.Broadcast;
using FishNet.Connection;
using FishNet.Transporting;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatFuntionality : MonoBehaviour
{
    public string Text;
    public int roomNumber;
    public static Action<int> GetRoomNumber;
    private void OnEnable()
    {
        GetRoomNumber = ReceiveRoomNumber;
        InstanceFinder.ClientManager.RegisterBroadcast<ChatBroadCast>(OnChatBroadcast);
        InstanceFinder.ClientManager.RegisterBroadcast<ChatBroadCast>(BrodCastMessage);
    }

    private void OnDisable()
    {
        InstanceFinder.ClientManager.UnregisterBroadcast<ChatBroadCast>(OnChatBroadcast);
        InstanceFinder.ClientManager.UnregisterBroadcast<ChatBroadCast>(BrodCastMessage);
    }

    public void ReceiveRoomNumber(int _roomNumber)
    {
        roomNumber = _roomNumber;
    }

    private void OnChatBroadcast(ChatBroadCast cast, Channel channel)
    {
        Debug.Log("Registered");
        int ConnID = cast.UserId;
        int roomNumber = cast.RoomNumber;  
        Debug.Log(ConnID+" "+roomNumber);
    }

    public void BrodCastMessage(ChatBroadCast msg, Channel channel)
    {
        //networkConnection = InstanceFinder.ClientManager.Connection;
        InstanceFinder.ServerManager.Broadcast(msg, false);
        Debug.Log(" " + roomNumber);
    }

    [ContextMenu("Send Message to server")]
    public void SendChat()
    {
        var networkID = InstanceFinder.NetworkManager.ClientManager.Connection.ClientId;
        SendMessage(networkID, roomNumber, Text);
    }
   public void SendMessage(int NetworkID, int RoomNumber, string message)
    {
        ChatBroadCast msg = new ChatBroadCast()
        {
            Message = message,
            RoomNumber = RoomNumber,
            UserId = NetworkID
        };

        if(InstanceFinder.IsServerStarted)
        {
            InstanceFinder.ServerManager.Broadcast(msg);
            Debug.Log("Message Send To server");
        }

        else if(InstanceFinder.IsClientStarted)
        {
            InstanceFinder.ClientManager.Broadcast(msg);
            Debug.Log("Message Send To server");
        }
               //Debug.Log("Message Send From Server");
    }

    public struct ChatBroadCast : IBroadcast
    {
        public int UserId;
        public int RoomNumber;
        public string Message;
    }
}



