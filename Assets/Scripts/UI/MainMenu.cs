using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button Race, Exit;


    
    private void Awake()
    {
        Race.onClick.AddListener(onRace);
        Exit.onClick.AddListener(OnExit);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void onRace()
    {
        SceneManager.LoadScene("Race3");
    }

    void OnExit()
    {
        Application.Quit();
    }
    void onRumble()
    {

    }
}
