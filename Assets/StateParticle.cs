using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateParticle : MonoBehaviour
{
    public ParticleSystem particleSystem { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();

        particleSystem.Stop();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
