using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCollider : MonoBehaviour
{
    public IState currentState { get; private set; }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<IState>() != null)
        {
            var tileOnUpgrade = collider.GetComponent<IState>();
            currentState = tileOnUpgrade;
        }
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
