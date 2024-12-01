using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.Timeline;

public class StateManager : MonoBehaviour
{
    public static StateManager Instance;
    public IState slipperyState;
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
            GrafoDij = GameObject.FindWithTag("Managers").GetComponent<TDAGraph>();
            FindAvailableStates();

            //foreach(var surf in surfaces)
            //{
            //    tileMaps.Add(surf.GetComponent<Tilemap>());
            //}
            
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


        }
        UpdateGraph();
        // Iniciar cooldown
        StartCoroutine(ResetCooldown());
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
