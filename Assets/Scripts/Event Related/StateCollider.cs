using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateCollider : MonoBehaviour
{
    int counter;
    public IState state { get; private set; }

    [SerializeField] TextMeshProUGUI ZoneText;
    [SerializeField] TextMeshProUGUI StateText;
    public List<TopDownController> carListInZone { get; private set; } = new List<TopDownController>(); //Autos en esta superficie

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<TopDownController>() != null)
        {
            if (state == null) return;
            var controller = collision.gameObject.GetComponent<TopDownController>();
            carListInZone.Add(controller);
            state.EnterState(controller);
        }
    }
    void Update()
    {
        if (SceneNameManager.Instance.IsRaceScene(SceneManager.GetActiveScene()))
        {
            foreach (TopDownController car in carListInZone)
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
            carListInZone.Remove(controller);
            state.ExitState(controller);

        }
    }


    public void SetCurrentState(IState newState)
    {
        if (carListInZone.Count != 0)
        {
            StartCoroutine(WaitAndSetState(newState)); //Si hay autos en la zona llama a corrutina
        }
        else
        {
            if (newState == null) Debug.Log("Estado inválido"); //Si está vacía ejecuta directamente el cambio de estados
            state = newState;
            ZoneText.text = state.GetType().Name;
            StateText.text = state.GetType().Name;
        }
    }

    private IEnumerator WaitAndSetState(IState newState)
    {
        // Espera hasta que la lista esté vacía
        while (carListInZone.Count != 0)
        {
            yield return null; // Se queda ahi hasta que la lista esté vacía
        }

        // Cuando no haya autos en la zona, cambia el estado
        state = newState;

        ZoneText.text = state.GetType().Name;
        StateText.text = state.GetType().Name;
    }
}



