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

    public void LoadSceneOnServer(NetworkConnection conn)
    {
        SceneLoadData sld = new SceneLoadData("ExternalCode/Scenes/Game");
        LoadOptions loadOptions = new LoadOptions
        {
            AllowStacking = true,
        };
        sld.Options = loadOptions;
        InstanceFinder.SceneManager.LoadConnectionScenes(conn, sld);
        sceneReference = sld;
    }

    [ContextMenu("Load Scene on client")]

    [ServerRpc(RunLocally = true, RequireOwnership = false)]
    public void SetRefServer()
    {
        sceneLoadDataOnServer = sceneReference;
        //base.SceneManager.LoadConnectionScenes(base.Owner, sceneLoadDataOnServer);
        Debug.Log("1");
    }

    [ObserversRpc(ExcludeOwner = true, BufferLast = true)]
    public void TargetLoadScene(NetworkConnection conn, SceneLoadData sld)
    {
        LoadSceneOnClient(sld,conn);
        Debug.Log("2");
    }


    public void LoadSceneOnClient(SceneLoadData sld,NetworkConnection conn)
    {
        SceneLoadData scene = sceneReference;
        base.SceneManager.LoadConnectionScenes(conn,scene);
        Debug.Log("3");
    }




}
