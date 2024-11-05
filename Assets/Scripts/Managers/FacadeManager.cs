using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FacadeManager : MonoBehaviour
{
    public static FacadeManager Instance;
   
    
    
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


   
    void Update()
    {
        
    }
}
