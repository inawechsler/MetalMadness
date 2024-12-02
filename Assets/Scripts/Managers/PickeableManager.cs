using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PickeableManager : MonoBehaviour
{
    public Tilemap tilemap;
    private EnginePieces[] engines;
    // Start is called before the first frame update
    void Start()
    {
        tilemap = GameObject.FindWithTag("Track").GetComponent<Tilemap>();
        SpawnCurrency();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnCurrency()
    {
        // Lista para almacenar las posiciones v�lidas dentro del Tilemap
        List<Vector3Int> validPositions = new List<Vector3Int>();

        // Recorre todas las posiciones dentro de los l�mites del Tilemap
        foreach (var position in tilemap.cellBounds.allPositionsWithin)
        {
            // Verifica si la celda tiene un tile
            if (tilemap.HasTile(position))
            {
                validPositions.Add(position);
            }
        }

        // Verifica si hay posiciones v�lidas
        if (validPositions.Count == 0)
        {
            Debug.LogWarning("No valid positions found in the Tilemap!");
            return;
        }

        // Determina cu�ntos EnginePieces crear
        int engineCount = Random.Range(2, 5);
        engines = new EnginePieces[engineCount];

        for (int i = 0; i < engineCount; i++)
        {
            // Selecciona una posici�n aleatoria de las v�lidas
            Vector3Int randomCell = validPositions[Random.Range(0, validPositions.Count)];

            // Convierte la posici�n de celda del Tilemap a una posici�n del mundo
            Vector3 worldPosition = tilemap.CellToWorld(randomCell) + tilemap.tileAnchor;

            // Instancia la pieza en la posici�n del mundo
            GameObject enginePrefab = Resources.Load<GameObject>("EnginePiece"); // Aseg�rate de tener un prefab llamado "EnginePiece" en Resources
            if (enginePrefab != null)
            {
                GameObject engineInstance = Instantiate(enginePrefab, worldPosition, Quaternion.identity);
                engines[i] = engineInstance.GetComponent<EnginePieces>();
            }
            else
            {
                Debug.LogError("EnginePiece prefab not found in Resources folder!");
            }
        }
    }
}
