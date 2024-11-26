using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Car : MonoBehaviour
{


    public bool onTrack;
    public float raycastDistance = 1f;
    public CarUpgrades upgrades;
    // Start is called before the first frame update

    private void Awake()
    {
    }
    void Start()
    {
        upgrades = GetComponent<CarUpgrades>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckWhereIsRunning();
    }



    void CheckWhereIsRunning()
    { 
        Vector2 rayDirection = Vector2.right.normalized;

        Vector2 rayOrigin = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, raycastDistance);


        Debug.DrawRay(rayOrigin, rayDirection * raycastDistance, Color.red);
        if (hit.collider != null)
        {

            if (hit.collider.gameObject.CompareTag("Track") || hit.collider.gameObject.CompareTag("TileState"))
            {
                onTrack = true;

            }
            else
            {
                onTrack = false;
            }
        }
    }
}
