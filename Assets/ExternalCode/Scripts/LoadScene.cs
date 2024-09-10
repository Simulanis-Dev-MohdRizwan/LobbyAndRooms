using FishNet.Object;
using FishNet.Managing.Scened;
using FishNet;
using FishNet.Connection;
using UnityEngine;
public class LoadScene : NetworkBehaviour
{

    public SceneLoadData ServerSceneRef;
    public SceneLoadData ClientSceneRef;
    public SceneLoadData TempHolder;

    public int receivedClientId;

    public int thisClientId;

    public bool serverStarted;
    public override void OnStartServer()
    {
        base.OnStartServer();
        //LoadSceneOnServer(InstanceFinder.NetworkManager.ClientManager.Connection.ClientId);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        thisClientId = InstanceFinder.NetworkManager.ClientManager.Connection.ClientId;
    }
    [ServerRpc(RequireOwnership = false, RunLocally = true)]
    public void LoadSceneOnServer(int conn)
    {
        if (!InstanceFinder.NetworkManager.ServerManager.Started) { Debug.Log("Server not started"); };
        SceneLoadData sld = new SceneLoadData("ExternalCode/Scenes/Game");
        LoadOptions loadOptions = new LoadOptions
        {
            AllowStacking = true,
        };
        sld.Options = loadOptions;
        InstanceFinder.SceneManager.LoadConnectionScenes(GetConnFromID(conn),sld);
        Debug.Log($"scene loading successfull for {receivedClientId} ");
        //SendSceneLoadDataToServer(sld);
    }

    [ServerRpc(RequireOwnership =false,RunLocally =true)]
    public void LoadScenForClient(int id)
    {
        receivedClientId = id;
        LoadSceneOnServer(id);
        Debug.Log($"received {id} for server");
    }

    public  NetworkConnection GetConnFromID(int clientId)
    {
        if (InstanceFinder.NetworkManager.ServerManager.Clients.ContainsKey(clientId))
        {
            Debug.Log($"the id {clientId} exist in dictionary");
            return InstanceFinder.NetworkManager.ServerManager.Clients[(int)clientId];
        }
        else
        {
            Debug.Log($"the id {clientId} does not exixt in dictionary");
            return null;
        }
    }

    [ContextMenu("load scene")]
    public void LoadSceneFromClientSide()
    {
      LoadSceneOnServer(thisClientId);
    }
}



