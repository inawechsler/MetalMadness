using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.Timeline;

public class StateManager : MonoBehaviour
{
    public static StateManager Instance;
    public IState slipperyState;
    public IState electricState;
    public IState slowState;
    [SerializeField] TopDownController[] topDownController;
    private TDAGraph GrafoDij;

    private bool canAssignStates = true; // Bandera para controlar el cooldown

    [SerializeField] public List<Tilemap> tileMaps { get; set; }

    [SerializeField] private List<GameObject> surfaces; // Las superficies
    [SerializeField] private List<IState> availableStates; // La lista de estados disponibles

    private bool lapChangeCooldown = false;

    [SerializeField] private int lapsToTriggerChange = 1;
    private int currentLap = 0;

    public UnityEvent<IState> stateChanged = new UnityEvent<IState>();
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
            electricState = GameObject.FindWithTag("TileState").GetComponent<WindyState>();
            slipperyState = GameObject.FindWithTag("TileState").GetComponent<SlippyState>();
            slowState = GameObject.FindWithTag("TileState").GetComponent<SlowState>();
            GrafoDij = GameObject.FindWithTag("Managers").GetComponent<TDAGraph>();
            FindAvailableStates();
            AssignRandomStates();
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

    public void UpdateGraph()
    {
        foreach (var surface in surfaces)
        {
            GrafoDij.UpdateGraphWeights(surface.GetComponent<Tilemap>());
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            AssignRandomStates();
        }
    }

    private void AssignRandomStates()
    {
        if (!canAssignStates) return; // Si está en cooldown, no hacer nada

        canAssignStates = false; // Activar cooldown

        foreach (GameObject surface in surfaces)
        {
            int randomStateIndex = Random.Range(0, availableStates.Count);

            IState randomState = availableStates[randomStateIndex];
            var stateColl = surface.GetComponent<StateCollider>();

            stateColl.SetCurrentState(randomState);

            var particleSystem = surface.GetComponent<ParticleSystem>();

            randomState.ClimateStateSet(particleSystem);

            StartCoroutine(SetState(randomState, stateColl));

        }

        StartCoroutine(ResetCooldown());
    }
    private IEnumerator SetState(IState state, StateCollider surface)
    {
        yield return new WaitForSeconds(2f); // Esperar 9 segundos

        TrackManager.Instance.SetStateImage(state, surface);
    }
    private IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(9f); // Esperar 9 segundos
        canAssignStates = true; // Resetear cooldown
    }

    public void OnLapCompleted()
    {
        currentLap++;
        AssignRandomStates();
    }

}
