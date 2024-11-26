using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCollider : MonoBehaviour
{
    public IState currentState { get; private set; }

    public void GetTileState()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.forward, 1, 1);

        if (hit.collider.gameObject.name == "State")
        {

            var tileOnUpgrade = hit.collider.GetComponent<StateCollider>();
            currentState = tileOnUpgrade.state;
            Debug.Log(currentState != null);
        } else
        {

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
