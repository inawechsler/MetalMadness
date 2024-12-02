using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateParticle : MonoBehaviour
{
    public ParticleSystem partSystem { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        partSystem = GetComponent<ParticleSystem>();




    }
    // Update is called once per frame
    void Update()
    {
        if(partSystem == null ) { Debug.Log(partSystem.gameObject.name); }
    }
}
