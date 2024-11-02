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


    public bool onTrack, offTrack, canFinish;
    float elapsedCollisionTime;
    private Rigidbody2D rb;
    float maxSpeedonTrack = 35f;
    float maxSpeedoffTrack = 10f;
    public float raycastDistance = 1f;
    private CarUpgrades upgrades;
    // Start is called before the first frame update

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        upgrades = GetComponent<CarUpgrades>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {



        offTrack = !onTrack;
 
        CheckWhereIsRunning();
        ManageLapTime();




    }



    void CheckWhereIsRunning()
    { 
        Vector2 rayDirection = Vector2.right.normalized;

        Vector2 rayOrigin = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, raycastDistance);


        Debug.DrawRay(rayOrigin, rayDirection * raycastDistance, Color.red);
        if (hit.collider != null)
        {

            if (hit.collider.gameObject.CompareTag("Track"))
            {
                onTrack = true;

            }
            else
            {
                onTrack = false;
            }
        }
    }

    void ManageLapTime()
    {
        if (elapsedCollisionTime > 8f)
        {
            canFinish = false;
        }
        else
        {
            elapsedCollisionTime += Time.deltaTime;
        }
    }


    //public float GetSpeed()
    //{
    //    return speed;
    //}
    //void ManageSpeed()
    //{
    //    if (onTrack)
    //    {
    //        if (speed < maxSpeedonTrack)
    //        {
    //            speed += .5f;
    //        }

    //    }
    //    else
    //    {
    //        if (speed >= maxSpeedoffTrack)
    //        {
    //            speed -= .5f;
    //            //rb.velocity = -direction * speed;
    //        } else
    //        {
    //            speed += .5f;
    //        }
    //    }

    //    rb.velocity = -direction * speed;
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FinishLine"))
        {
            onTrack = true;
            if (!canFinish)
            {
                canFinish = true;
                //GameManager.Instance.SetLaps(1);
                elapsedCollisionTime = 0f;
            }
        }
    }
    
}
