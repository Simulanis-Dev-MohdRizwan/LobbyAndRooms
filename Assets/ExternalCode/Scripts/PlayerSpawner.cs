using FishNet;
using FishNet.CodeGenerating;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Transporting;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;
public class PlayerSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject Player;
    [ExcludeSerialization] private int clientId;

    public override void OnStartClient()
    {
        base.OnStartClient();
        clientId = InstanceFinder.NetworkManager.ClientManager.Connection.ClientId;
    }


    private void SpawnPlayer()
    {
        GameObject networkObject = Instantiate(Player);
        NetworkObject no = networkObject.GetComponent<NetworkObject>();
        InstanceFinder.NetworkManager.ServerManager.Spawn(no);

        Scene scene = this.gameObject.scene;
        UnitySceneManager.MoveGameObjectToScene(no.gameObject, scene);
    }

    [ContextMenu("spawnPlayer")]
    public void SpawnPlayerLocally()
    {
        SpawnPlayerOnServer(clientId);
    }
    [ServerRpc(RequireOwnership =false,RunLocally =true)]
    private void SpawnPlayerOnServer(int _clientId)
    {
        //clientId = _clientId;
        SpawnPlayerOnTarget(ToolScript.GetConnFromID(_clientId));
        //SpawnPlayer();
        Debug.Log("Spawning Player on Server");
    }

    [TargetRpc]
    [ObserversRpc(ExcludeOwner =false,BufferLast =true)]
    private void SpawnPlayerOnTarget(NetworkConnection connect)
    {
        SpawnPlayer();
        Debug.Log("Spawning Player on Target");
    }

}
