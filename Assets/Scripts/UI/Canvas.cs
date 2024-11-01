using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class Canvas : MonoBehaviour
{
    //  Start is called before the first frame update
    [SerializeField] Image velocity;
    TopDownController car;
    void Start()
    {
        velocity.fillAmount = 0;
        car = GameObject.FindGameObjectWithTag("Player").GetComponent<TopDownController>();
    }
    //    Update is called once per frame
    void Update()
    {
        velocity.fillAmount = car.GetSpeed() / 20;
    }
}
