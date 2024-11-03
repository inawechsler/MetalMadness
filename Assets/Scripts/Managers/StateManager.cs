using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour
{
    public static StateManager Instance;
    public IState state;
    public IState slipperyState;
    public IState slowState;
    [SerializeField] TopDownController[] topDownController;

    [SerializeField] private List<GameObject> surfaces; // Las superficies
    [SerializeField] private List<IState> availableStates; // La lista de estados disponibles

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
            AssignRandomStates();
        }
    }

    private void FindAvailableStates()
    {
        // Encuentra todos los objetos que implementen la interfaz IState
        availableStates = FindObjectsOfType<MonoBehaviour>().OfType<IState>().ToList();

        if (availableStates.Count == 0)
        {
            Debug.LogWarning("No se encontraron estados disponibles.");
        }
    }

    private void AssignRandomStates()
    {
        // Recorre todas las superficies y asigna un estado aleatorio de la lista de estados disponibles
        foreach (GameObject surface in surfaces)
        {
            int randomStateIndex = Random.Range(0, availableStates.Count);
            IState randomState = availableStates[randomStateIndex];

            ChangeCurrentState(slowState);

            Debug.Log($"Superficie {surface.name} ahora tiene el estado: {randomState.GetType().Name}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneNameManager.Instance.IsRaceScene(SceneManager.GetActiveScene()))
        {
            foreach (TopDownController car in topDownController)
            {
                state.UpdateState(car);
            }
        }
    }

    public void ChangeCurrentState(IState newState)
    {
        if (newState == null)
        {
            Debug.LogError("El  estado es nulo, no se puede cambiar el estado.");
            return; 
        }

        state = newState; 
    }

    public void OnLapCompleted()
    {
        currentLap++;
        AssignRandomStates();
        // Cada 3 vueltas

    }
}
