using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour
{
    public static StateManager Instance;
    public IState slipperyState;
    public IState slowState;
    [SerializeField] TopDownController[] topDownController;

    [SerializeField] private List<GameObject> surfaces; // Las superficies
    [SerializeField] private List<IState> availableStates; // La lista de estados disponibles

    private bool lapChangeCooldown = false;

    [SerializeField] private int lapsToTriggerChange = 1;
    private int currentLap = 0;
    // Start is called before the first frame update

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
    public void Init()
    {
        if (SceneNameManager.Instance.IsRaceScene(SceneManager.GetActiveScene()))
        {
            topDownController = FindObjectsOfType<TopDownController>();
            slipperyState = GameObject.FindWithTag("States").GetComponent<SlippyState>();
            slowState = GameObject.FindWithTag("States").GetComponent<SlowState>();
            FindAvailableStates();
        }
    }

    private void FindAvailableStates()
    {
        availableStates = FindObjectsOfType<MonoBehaviour>().OfType<IState>().ToList();

        if (availableStates.Count == 0)
        {
            Debug.LogWarning("No se encontraron estados disponibles.");
        }
    }

    private void AssignRandomStates()
    {
        foreach (GameObject surface in surfaces)
        {
            int randomStateIndex = Random.Range(0, availableStates.Count); //Del 0 a la cantidad de IStates que encuentre

            IState randomState = availableStates[randomStateIndex]; //Devuelve el estado de la lista con el indice random

            var stateColl = surface.GetComponent<StateCollider>();

            stateColl.SetCurrentState(randomState);

        }
    }

    public void OnLapCompleted()
    {
        currentLap++;
        AssignRandomStates();
    }

}
