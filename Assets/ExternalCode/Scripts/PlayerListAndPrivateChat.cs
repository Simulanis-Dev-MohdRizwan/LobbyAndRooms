using FishNet;
using FishNet.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerListAndPrivateChat : NetworkBehaviour
{
    [SerializeField] private Transform container;
    public GameObject Button;
    public List<int> playerlist;

    public static Action<int> addPlayerToList;
    private void OnEnable()
    {
        //PlayerM.OnPlayerJoined += PlayerJoined;
    }
    public void PlayerJoined(int id)
    {
        if (!playerlist.Contains(id))
        {
            playerlist.Add(id);
        }
    }

    private void OnDisable()
    {
        //PlayerM.OnPlayerJoined -= PlayerJoined;
    }
}
