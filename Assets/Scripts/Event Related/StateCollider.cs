using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class StateCollider : MonoBehaviour
{
    int counter;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<TopDownController>() != null)
        {
            counter++;
            var controller = collision.gameObject.GetComponent<TopDownController>();
            StateManager.Instance.state.EnterState(controller);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<TopDownController>() != null)
        {
            var controller = collision.gameObject.GetComponent<TopDownController>();
            StateManager.Instance.state.ExitState(controller);

        }
    }
}
