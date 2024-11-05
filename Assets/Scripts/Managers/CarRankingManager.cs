using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarRankingManager : MonoBehaviour
{
    public static CarRankingManager Instance;

    public int carPosition = 0;
    public List<CarLapCounter> carList {  get; private set; } = new List<CarLapCounter>();

    public Dictionary<int, string> ranking = new Dictionary<int, string>();

    private int lapsCompleted;
    string text;
    public int LapsCompleted => lapsCompleted;

    private int lapsToComplete = 6;

    private int carsFinishedRace = 0;

    public List<KeyValuePair<int, string>> tempRanking { get; private set; } = new List<KeyValuePair<int, string>>();
    public int LapsToComplete => lapsToComplete;

    private bool lapCompletedTriggered = false;

    private LeadeBoardUIHandler boardUIHandler;
   

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void Init()
    {
        if (SceneNameManager.Instance.IsRaceScene(SceneManager.GetActiveScene()))
        {
                  
            boardUIHandler = GameObject.FindWithTag("Leaderboard").GetComponent<LeadeBoardUIHandler>();

        

       
            CarLapCounter[] carLapCountersArr = FindObjectsOfType<CarLapCounter>();

            carList = carLapCountersArr.ToList();

            foreach (CarLapCounter lapCounter in carList)
            {
                lapCounter.OnCheckPointPassed.AddListener(OnPassCheckPoint);
            }
        }
    }

    private void Update()
    {
     
    }
    public int SetLapsCompleted(int value)
    {

       return lapsCompleted += value;

    }



    void OnPassCheckPoint(CarLapCounter carLapCounter)
    {
        if (carLapCounter.lapsCompleted >= LapsToComplete)
        {
            if (!carLapCounter.isRaceCompleted) // Solo si aún no ha completado la carrera
            {
                carLapCounter.isRaceCompleted = true;
                carsFinishedRace++; // Incrementar cuando un coche completa la carrera

                // Finalizar el ranking para este coche
                int finalPosition = carList.IndexOf(carLapCounter) + 1;

                if (!ranking.ContainsKey(finalPosition))
                {
                    ranking.Add(finalPosition, carLapCounter.gameObject.name);
                }
                else
                {
                    while (ranking.ContainsKey(finalPosition))
                    {
                        finalPosition++;
                    }
                    ranking.Add(finalPosition, carLapCounter.gameObject.name);
                }

                // Actualizar la UI
                boardUIHandler.UpdateList(ranking.ToList());

                Debug.Log($"Carros terminados: {carsFinishedRace} / {carList.Count}");

                // Si todos los coches han terminado, cambiar de escena
                if (carsFinishedRace == carList.Count)
                {
                    SceneManager.LoadScene("LevelResume");
                }
            }
        }
        else
        {
            carList = carList.OrderByDescending(car => car.lapsCompleted)
                             .ThenByDescending(car => car.PassedCheckPointNumber)
                             .ThenBy(car => car.TimeAtLastCheckPointPassed)
                             .ToList();

            ranking.Clear();
            for (int i = 0; i < carList.Count; i++)
            {
                ranking[i + 1] = carList[i].gameObject.name;
            }

            // Actualizar la UI con el ranking actual
            boardUIHandler.UpdateList(ranking.ToList());

            // Actualizar posición individual del coche
            carPosition = carList.IndexOf(carLapCounter) + 1;
            carLapCounter.SetCarPosition(carPosition);

            if (carPosition == 1 && !lapCompletedTriggered)
            {
                if(lapsCompleted > 0) StateManager.Instance.OnLapCompleted();

                lapCompletedTriggered = true;
            }

            // Restablecer el disparo para la próxima vuelta
            if (carLapCounter.PassedCheckPointNumber == 0)
            {
                lapCompletedTriggered = false;
            }

        }
    }
}
    




