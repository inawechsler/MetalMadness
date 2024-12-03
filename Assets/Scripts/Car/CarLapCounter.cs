using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Events;

public class CarLapCounter : MonoBehaviour
{
    private StateManager stateManager;
     public TextMeshProUGUI Laptext { get; private set; }

    [SerializeField] TextMeshProUGUI text;
    
    int passedCheckPointNumber = 0;

    bool showText;

    public int lapsCompleted;

    public int PassedCheckPointNumber => passedCheckPointNumber;

    float timeAtLastCheckPointPassed = 0;

    public float TimeAtLastCheckPointPassed => timeAtLastCheckPointPassed;

    int numberOfPassedCheckpoints = 0;

    public bool isRaceCompleted;

    bool hideRoutineRunning = false;
    float hideUITime = 0;

    int carPosition = 0;

    private void Start()
    {
        Laptext = GameObject.FindWithTag("Text").GetComponent<TextMeshProUGUI>();
    }
    public int SetCarPosition(int carPosition)
    {
        return this.carPosition = carPosition;

    }

    public UnityEvent<CarLapCounter> OnCheckPointPassed;


    private IEnumerator ShowPosition(float timeToVanish)
    {
        hideUITime += timeToVanish;

        text.text = carPosition.ToString();

        showText = true;

        if (!hideRoutineRunning)
        {
            hideRoutineRunning = true;

            yield return new WaitForSeconds(hideUITime);

            hideRoutineRunning = false;

            showText = false;
        }

    }



    private void Update()
    {
        if (showText)
        {
            text.gameObject.SetActive(true);
        }
        else
        {
            text.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            if (isRaceCompleted)
            {
                return;
            }
  

            CheckPoints checkPoints = collision.GetComponent<CheckPoints>();

            if (passedCheckPointNumber + 1 == checkPoints.checkPointNumber)
            {
                passedCheckPointNumber = checkPoints.checkPointNumber;

                
                numberOfPassedCheckpoints++;

                timeAtLastCheckPointPassed = Time.time;


                if (checkPoints.isFinishLine)
                {
                    lapsCompleted++;
                    passedCheckPointNumber = 0;
                    
                    if (gameObject.CompareTag("Player"))
                    {
                        CarRankingManager.Instance.SetLapsCompleted(1);
                        Laptext.text = "" + CarRankingManager.Instance.LapsCompleted.ToString() + " of " + CarRankingManager.Instance.LapsToComplete.ToString();
                        PickeableManager.Instance.SpawnCurrency();
                    }


                }

                OnCheckPointPassed?.Invoke(this);

                float timeToVanish = isRaceCompleted ? 100f : 3f;

                StartCoroutine(ShowPosition(timeToVanish));

            }
        }
    }
}
