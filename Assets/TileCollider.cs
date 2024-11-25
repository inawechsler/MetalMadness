using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCollider : MonoBehaviour
{
    public IState currentState { get; private set; }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("States"))
        {

            var tileOnUpgrade = collider.GetComponent<StateCollider>();
            currentState = tileOnUpgrade.state;
            Debug.Log(currentState.GetType().Name);
        }
    
        Debug.Log(collider.gameObject.name);    
    }

    public bool GetWeight()
    {
        return currentState != null;
    }

    public Vector3Int GetPosition()
    {
        return Vector3Int.FloorToInt(transform.position);
    }
}
