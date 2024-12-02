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
    private Sprite normalSprite;


    void ChangeSprite(Sprite sprite)
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
    }
    public void SetState(IState newState)
    {

        if(newState.GetType().Name == "SlippyState")
        {
            state = State.SlippyState;
        }
        else if(newState.GetType().Name == "SlowState")
        {
            state = State.SlowState;
        } else
        {
            state = State.NoCurrent;
        }

        Sprite newSprite = state switch
        {
            State.SlowState => SlowState,
            State.SlippyState => slipperyState,
            State.NoCurrent => normalSprite,
            _ => null,
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

    private void Start()
    {
        normalSprite = GetComponent<SpriteRenderer>().sprite;
    }
    // Update is called once per frame

}


public enum State
{
    SlowState, SlippyState, NoCurrent
}