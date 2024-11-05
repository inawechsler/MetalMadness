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
    public IState state;
    public IState slipperyState;
    public IState slowState;
    [SerializeField] TopDownController[] topDownController;

    [SerializeField] private List<GameObject> surfaces; // Las superficies
    [SerializeField] private List<IState> availableStates; // La lista de estados disponibles
    [SerializeField] TextMeshProUGUI[] ZoneText;
    [SerializeField] TextMeshProUGUI[] StateText;

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
        // Encuentra todos los objetos que implementen la interfaz IState
        availableStates = FindObjectsOfType<MonoBehaviour>().OfType<IState>().ToList();

        foreach (var state in availableStates)
        {
            if (availableStates.Contains(state))
            {
                availableStates.Remove(state);
            }
        }

        if (availableStates.Count == 0)
        {
            Debug.LogWarning("No se encontraron estados disponibles.");
        }
    }

    private void AssignRandomStates()
    {
        foreach (GameObject surface in surfaces)
        {
            int randomStateIndex = Random.Range(0, availableStates.Count);
  
            IState randomState = availableStates[randomStateIndex];

            Debug.Log(surface.name + "Has" + randomStateIndex + ": " + randomState.GetType().Name);
            ChangeCurrentState(randomState);
            ShowAvailableStates();


            for (int i = 0; i < ZoneText.Length; i++)
            {
                StateText[i].text = $"{randomState.GetType().Name}";
                ZoneText[i].text = $"{randomState.GetType().Name}";
            }
        }
    }


    private void ShowAvailableStates()
    {
        Debug.Log("Estados disponibles:");
        for (int i = 0; i < availableStates.Count; i++)
        {
            Debug.Log($"{i}: {availableStates[i].GetType().Name}");
        }
    }
    void Update()
    {
        if (SceneNameManager.Instance.IsRaceScene(SceneManager.GetActiveScene()))
        {
            foreach (TopDownController car in topDownController)
            {
                if(state != null)
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
        if (!lapChangeCooldown)
        {
            AssignRandomStates();
        }
        StartCoroutine(manageBoolState());

        // Cada 3 vueltas

    }

    IEnumerator manageBoolState()
    {
        lapChangeCooldown = true;

        yield return new WaitForSeconds(2f);

        lapChangeCooldown = false;
    }
}
