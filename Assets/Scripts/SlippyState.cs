using UnityEngine;

public class SlippyState : MonoBehaviour, IState
{
    public float slipperyDrift = 1f;
    private bool isOnState = false;

    [SerializeField] StateManager stateManager;

    public void UpdateState(TopDownController controller)
    {
        if (controller.isOnState)
        {
            controller.SetDriftFactor(slipperyDrift);
            controller.rb2D.drag = 0;
            controller.SetAccelerationInput(Mathf.Clamp(controller.GetAccelerationInput(), 0, 1f));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("AI"))
        {
            var controller = collision.gameObject.GetComponent<TopDownController>();
            controller.isOnState = true;  // Activar el estado resbaladizo
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("AI"))
        {
            var controller = collision.gameObject.GetComponent<TopDownController>();
            controller.isOnState = false;  // Desactivar el estado resbaladizo
            stateManager.ChangeCurrentState(stateManager.normalState);  // Cambiar al estado normal
            Debug.Log(collision.gameObject.name + " ha salido del estado Slippy y ha cambiado a Normal.");

        }
    }
}
