using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateCollider : MonoBehaviour
{
    int counter;
    IState state;
    public List<TopDownController> upgrades { get; private set; } = new List<TopDownController>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<TopDownController>() != null)
        {
            if (state == null) return;
            var controller = collision.gameObject.GetComponent<TopDownController>();
            upgrades.Add(controller);
            state.EnterState(controller);
        }
    }
    void Update()
    {
        if (SceneNameManager.Instance.IsRaceScene(SceneManager.GetActiveScene()))
        {
            foreach (TopDownController car in upgrades)
            {
                if (state != null)
                    state.UpdateState(car);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<TopDownController>() != null)
        {
            if (state == null) return;
            var controller = collision.gameObject.GetComponent<TopDownController>();
            upgrades.Remove(controller);
            state.ExitState(controller);

        }
    }


    public void SetCurrentState(IState newState)
    {
        if (newState == null) Debug.Log("ASA");
        state = newState;
    }
}



