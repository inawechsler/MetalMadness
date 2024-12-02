using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateImage : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public State state; 
    [SerializeField] public int ID;
    [SerializeField] public int StateColliderID;
    [SerializeField] public Sprite slipperyState;
    [SerializeField] public Sprite SlowState;


    void ChangeSprite(Sprite sprite)
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
    }
    public void SetState(IState newState)
    {
        state = (newState.GetType().Name == "SlippyState") ? State.SlippyState : State.SlowState; 

        Sprite newSprite = state switch
        {
            State.SlowState => SlowState,
            State.SlippyState => slipperyState,
            _ => null
        };

        if (newSprite != null)
        {
            ChangeSprite(newSprite);
        }
        else
        {
            Debug.LogWarning($"No sprite found for state {state} in {gameObject.name}");
        }
    }


    // Update is called once per frame

}


public enum State
{
    SlowState, SlippyState, NoCurrent
}