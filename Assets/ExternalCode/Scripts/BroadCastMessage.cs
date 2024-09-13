using System;
using TMPro;
using UnityEngine;

public class BroadCastMessage : MonoBehaviour
{
    public static Action<string> ReceivedMessage;
    public TextMeshProUGUI TextMeshProUGUI;
    private void Start()
    {
        ReceivedMessage += messageReceive;
    }

    public void messageReceive(string obj)
    {
        TextMeshProUGUI.text = obj;
    }

    private void OnDisable()
    {
        ReceivedMessage -= messageReceive;
    }
}
