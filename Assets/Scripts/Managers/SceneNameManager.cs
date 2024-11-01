using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneNameManager : MonoBehaviour
{
    public static SceneNameManager Instance;
    [SerializeField] List<SceneAsset> raceScemes = new();
    List<string> raceNames = new();
    // Start is called before the first frame update
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
    // Update is called once per frame
    void Update()
    {
        
    }
}
