using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEditor; 
using System.Collections.Generic; 
public class SceneNameManager : MonoBehaviour
{
    public static SceneNameManager Instance;
    [SerializeField] List<SceneAsset> raceScemes = new();
    List<string> raceNames = new();

    void Awake()
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

        foreach (SceneAsset asset in raceScemes)
        {
            if (asset != null)
            {
                raceNames.Add(asset.name);
            }
        }
    }

    public bool IsRaceScene(Scene scene)
    {
        return raceNames.Contains(scene.name);
    }

    void Update()
    {

    }
}
