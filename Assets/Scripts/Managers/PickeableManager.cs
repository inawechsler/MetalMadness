using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

public class PickeableManager : MonoBehaviour
{
    public static PickeableManager Instance;
    public Tilemap tilemap;
    public GameObject enginePrefab; // Asigna el prefab desde el inspector
    public ObjectPool<EnginePieces> enginePool;
    private List<EnginePieces> activeEngines = new List<EnginePieces>();
    private bool hasSpawned;

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
            tilemap = GameObject.FindWithTag("Track").GetComponent<Tilemap>();

            if (tilemap == null)
            {
                tilemap = GameObject.FindWithTag("Grid").GetComponentInChildren<Tilemap>(tilemap.gameObject.name == "Track");
            }


            if (enginePrefab == null)
            {
                enginePrefab = GameObject.FindWithTag("Engine");
                if (enginePrefab == null)
                    Resources.Load("EnginePieces");
            }


            // Configura el ObjectPool
            enginePool = new ObjectPool<EnginePieces>(
                createFunc: CreateEnginePiece,
                actionOnGet: OnEnginePieceGet,
                actionOnRelease: OnEnginePieceRelease,
                actionOnDestroy: DestroyEnginePiece,
                collectionCheck: false, // Opcional: evita un chequeo adicional
                defaultCapacity: 10,
                maxSize: 20
            );
        }
        //SpawnCurrency();
    }

    private EnginePieces CreateEnginePiece()
    {
        GameObject obj = Instantiate(enginePrefab);
        return obj.GetComponent<EnginePieces>();
    }

    private void OnEnginePieceGet(EnginePieces engine)
    {
        engine.gameObject.SetActive(true);
    }

    private void OnEnginePieceRelease(EnginePieces engine)
    {
        engine.gameObject.SetActive(false);
    }

    private void DestroyEnginePiece(EnginePieces engine)
    {
        Destroy(engine.gameObject);
    }

    public void SpawnCurrency()
    {
        if (!hasSpawned)
        {
            List<Vector3Int> validPositions = new List<Vector3Int>();

            foreach (var position in tilemap.cellBounds.allPositionsWithin)
            {
                if (tilemap.HasTile(position))
                {
                    validPositions.Add(position);
                }
            }

            if (validPositions.Count == 0)
            {
                Debug.LogWarning("No valid positions found in the Tilemap!");
                return;
            }

            int engineCount = Random.Range(4, 7);
     

            for (int i = 0; i < engineCount; i++)
            {
                Vector3Int randomCell = validPositions[Random.Range(0, validPositions.Count)];
                Vector3 worldPosition = tilemap.CellToWorld(randomCell) + tilemap.tileAnchor;

                EnginePieces enginePiece = enginePool.Get();
                enginePiece.transform.position = worldPosition;

                activeEngines.Add(enginePiece);
            }

            StartCoroutine(ManageBool());
        }
       
    }

    IEnumerator ManageBool()
    {

        hasSpawned = true;

        yield return new WaitForSeconds(1);

        hasSpawned = false;
    }
    public void ReturnAllEngines()
    {
        foreach (EnginePieces engine in activeEngines)
        {
            enginePool.Release(engine);
        }
        activeEngines.Clear();
    }
}
