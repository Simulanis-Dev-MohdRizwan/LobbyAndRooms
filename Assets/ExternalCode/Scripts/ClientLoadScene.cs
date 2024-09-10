using FishNet;
using FishNet.Object;
using UnityEngine;

public class ClientLoadScene : NetworkBehaviour
{
    public LoadScene loadScene;
    public int thisClientID;
    public override void OnStartClient()
    {
        base.OnStartClient();
        thisClientID = InstanceFinder.NetworkManager.ClientManager.Connection.ClientId;
    }

    [ContextMenu("Load the Scene")]
    public void RequestSceneLoad()
    {
        loadScene.LoadScenForClient(thisClientID);
    }
}
