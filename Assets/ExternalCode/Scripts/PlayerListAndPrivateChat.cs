using FishNet;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerListAndPrivateChat : NetworkBehaviour
{
    public List<int> playerlist;
    public int thisRoomId;

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
