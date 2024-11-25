using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNameManager : MonoBehaviour
{
    public static SceneNameManager Instance;

    [SerializeField] private List<string> raceScenes = new List<string>(); // Lista de nombres de escenas

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
    public bool IsRaceScene(Scene scene)
    {
        return raceScenes.Contains(scene.name);
    }
}
