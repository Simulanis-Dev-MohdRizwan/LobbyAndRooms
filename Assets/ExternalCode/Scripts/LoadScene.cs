using FishNet.Object;
using FishNet.Managing.Scened;
using FishNet;
using FishNet.Connection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SearchService;
public class LoadScene : NetworkBehaviour
{
    public int connectionId;
    public bool clientStarted;

    public SceneLoadData sceneReference;

    public SceneLoadData sceneLoadDataOnServer;
    public override void OnStartServer()
    {
        base.OnStartServer();
       
        LoadSceneOnServer(null);
    }

    public override void OnStartClient()
    {
       base.OnStartClient();
       connectionId = InstanceFinder.ClientManager.Connection.ClientId;
    }
    public void LoadSceneOnServer(NetworkConnection conn)
    {
        int i = 0;
        SceneLoadData sld = new SceneLoadData("ExternalCode/Scenes/Game");
        Debug.Log(i++);
        LoadOptions loadOptions = new LoadOptions
        {
            AllowStacking = true,
        };
        Debug.Log(i++);
        sld.Options = loadOptions;
        Debug.Log(i++);
        InstanceFinder.SceneManager.LoadConnectionScenes(conn, sld);
        Debug.Log(i++);
        sceneReference = sld;
    }

    [ServerRpc(RequireOwnership =false,RunLocally =true)]
    public void LoadSceneUsingRPC(NetworkConnection conn)
    {
        LoadSceneOnServer(conn);
    }

}
