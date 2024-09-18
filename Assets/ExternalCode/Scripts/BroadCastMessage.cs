using System;
using TMPro;
using UnityEngine;

public class BroadCastMessage : MonoBehaviour
{
    public static Action<string> ReceivedMessage;

    public Transform MessageContainer;
    public GameObject MessageContainerPrefab;
    private void Start()
    {
        ReceivedMessage += messageReceive;
    }

    public void messageReceive(string obj)
    {
        var textbox = Instantiate(MessageContainerPrefab,MessageContainer);
        textbox.GetComponentInChildren<TextMeshProUGUI>().text = obj;
    }

    private void OnDisable()
    {
        ReceivedMessage -= messageReceive;
    }
}
