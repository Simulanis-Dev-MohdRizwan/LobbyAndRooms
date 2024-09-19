using FishNet;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomInfo : NetworkBehaviour
{
    public List<int> playerlist;
    public int thisRoomId;

   // <summary> addToPlayer<playerId,RoomID> </summary>
    public static Action<int,int> addToPlayerList ;

    private void OnEnable()
    {
        addToPlayerList += AddToList;
    }

    private void AddToList(int playerID,int roomNumber)
    {
        if (roomNumber != thisRoomId) return;
        if (playerlist.Contains(playerID)) return;
        playerlist.Add(playerID);
    }

    public void SetRoomId(int roomId)
    {
        SetRoomIdOnObserver(roomId);
    }

    [ServerRpc(RequireOwnership =false,RunLocally =true)]
    public void SetRoomIDOnServer(int roomId)
    {
        SetRoomIdOnObserver(roomId);
    }

    [ObserversRpc(ExcludeOwner =false,BufferLast =true)]
    public void SetRoomIdOnObserver(int roomID)
    {
        thisRoomId = roomID;
    }
}
