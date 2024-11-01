using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInputController : MonoBehaviour
{
    // Start is called before the first frame update

    TopDownController carMovement;

    void Start()
    {
        carMovement = GetComponent<TopDownController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputVector = Vector2.zero;

        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.y = Input.GetAxis("Vertical");

        
        


        carMovement.SetInputVector(inputVector);

    }
}
