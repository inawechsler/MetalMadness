using UnityEngine;

public class EnginePieces : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Picker"))
        {
            LevelManager.Instance.AddEnginePieces();
            Debug.Log(LevelManager.Instance.enginePiecesCollected);

            // Devuelve el objeto al pool
            PickeableManager.Instance.enginePool.Release(this);
        }
    }
}