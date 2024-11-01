using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapController : MonoBehaviour
{
    Transform car;
    [SerializeField] GameObject carRef;
    // Start is called before the first frame update
    void Start()
    {
        car = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //carRef.transform.position =  new Vector3(car.transform.position.x, car.transform.position.y, 0);
    }
}
