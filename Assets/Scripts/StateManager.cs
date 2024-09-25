using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    IState state;
    public IState slipperyState;
    public IState normalState;
    [SerializeField] TopDownController[] topDownController;
    [SerializeField] public GameObject car;

    [SerializeField] private List<GameObject> surfaces; // Las superficies
    [SerializeField] private List<IState> availableStates; // La lista de estados disponibles

    [SerializeField] private int lapsToTriggerChange = 1;
    private int currentLap = 0;
    // Start is called before the first frame update

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        FindAvailableStates();

        normalState = GetComponent<NormalState>();
        slipperyState = GetComponent<SlippyState>();
    }
    void Start()
    {
        ChangeCurrentState(slipperyState);

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

            ChangeCurrentState(randomState);

            Debug.Log($"Superficie {surface.name} ahora tiene el estado: {randomState.GetType().Name}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (TopDownController car in topDownController)
        {
            state.UpdateState(car);
        }

    }

    public void ChangeCurrentState(IState newState)
    {
        if (newState == null)
        {
            Debug.LogError("El  estado es nulo, no se puede cambiar el estado.");
            return;  // Salir si el nuevo estado es nulo
        }

        state = newState;  // Cambiar el estado actual
        Debug.Log("El estado ha cambiado a: " + state.GetType().Name);
    }

    public void OnLapCompleted()
    {
        currentLap++;
        AssignRandomStates();
        // Cada 3 vueltas

    }
}
