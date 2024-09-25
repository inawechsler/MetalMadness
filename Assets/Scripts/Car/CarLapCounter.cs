using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class CarLapCounter : MonoBehaviour
{
    //private StateManager stateManager;

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
    //    stateManager = FindAnyObjectByType<StateManager>();
    }
    public int SetCarPosition(int carPosition)
    {
        return this.carPosition = carPosition;
    }

    public event Action<CarLapCounter> OnCheckPointPassed;


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
            if(isRaceCompleted)
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
                    //stateManager.OnLapCompleted();

                    TopDownController controller = FindAnyObjectByType<TopDownController>();

                    if (gameObject.CompareTag("Player"))
                    {
                        LevelManager.Instance.SetLapsCompleted(1);
                        GameManager.Instance.text.text = "Laps: " + LevelManager.Instance.LapsCompleted.ToString() + "/" + LevelManager.Instance.LapsToComplete.ToString();
                    }


                }

                OnCheckPointPassed?.Invoke(this);

                float timeToVanish = isRaceCompleted ? 100f : 3f;

                StartCoroutine(ShowPosition(timeToVanish));

            }
        }
    }
}
