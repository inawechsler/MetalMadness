using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button Race;


    
    private void Awake()
    {
        Race.onClick.AddListener(onRace);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void onRace()
    {
        SceneManager.LoadScene("Race");
    }

    void onRumble()
    {

    }
}
