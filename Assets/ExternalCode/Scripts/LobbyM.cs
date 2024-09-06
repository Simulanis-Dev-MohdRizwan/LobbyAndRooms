using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LobbyM : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("loadscene")]
    public void UnityLoadScene()
    {
        SceneManager.LoadScene("ExternalCode/Scenes/Game", LoadSceneMode.Additive);
    }
}
