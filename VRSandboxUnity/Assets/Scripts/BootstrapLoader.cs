using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapLoader : MonoBehaviour
{
    public enum LevelsToLoad
    {
        RoomViewScene,
        RH2055
    }

    private string[] SceneNames =
    {
        "Rayzor-Floorplan-View",
        "RH2055"
    };

    public LevelsToLoad SceneLoadOnStart;
    public GameObject Player;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene(SceneNames[(int)SceneLoadOnStart]);
    }

    private void OnSceneLoaded(Scene loadedScene, LoadSceneMode sceneLoadingMode)
    {
        var spawnpoint = GameObject.FindGameObjectWithTag("PlayerSpawn");

        if(spawnpoint != null)
        {
            Player.transform.position = spawnpoint.transform.position;
            Player.transform.rotation = spawnpoint.transform.rotation;
        }
    }
}
