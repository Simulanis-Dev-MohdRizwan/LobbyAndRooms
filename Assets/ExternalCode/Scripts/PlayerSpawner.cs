using FishNet;
using FishNet.Object;
using FishNet.Transporting;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;
public class PlayerSpawner : NetworkBehaviour
{
    [SerializeField] private NetworkObject player;


    private void OnEnable()
    {
        SpawnPlayer();
    }
    [ContextMenu("spawnPlayer")]
    public void SpawnPlayer()
    {
        NetworkObject playah = Instantiate(player);
        Scene scene = this.gameObject.scene;
        UnitySceneManager.MoveGameObjectToScene(playah.gameObject, scene);
        InstanceFinder.ServerManager.Spawn(playah.gameObject);
    }

}
