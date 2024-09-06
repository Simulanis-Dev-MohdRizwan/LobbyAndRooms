using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientLoadScene : NetworkBehaviour
{
    public LoadScene scene;

    [ContextMenu("Load the Scene")]
    public void RequestSceneLoad()
    {

            scene.LoadSceneUsingRPC(base.Owner);
        
    }
}
