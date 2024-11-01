using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FacadeManager : MonoBehaviour
{
    public static FacadeManager Instance;
    // Start is called before the first frame update
    
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    public void Init()
    {
        CarRankingManager.Instance.Init();
        StateManager.Instance.Init();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
