using System.Collections;
using System.Collections.Generic;
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

    public int LapsCompleted => lapsCompleted;

    private int lapsToComplete = 3;

    private int carsFinishedRace = 0;

    public int LapsToComplete => lapsToComplete;

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
                lapCounter.OnCheckPointPassed += OnPassCheckPoint;
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
                    UnityEngine.SceneManagement.SceneManager.LoadScene("LevelResume");
                }
            }
        }
        else
        {
            // Mientras la carrera sigue en progreso, ordenar y mostrar posiciones actuales
            carList = carList.OrderByDescending(index => index.lapsCompleted)
                             .ThenByDescending(index => index.PassedCheckPointNumber)
                             .ThenBy(index => index.TimeAtLastCheckPointPassed)
                             .ToList();

            carPosition = carList.IndexOf(carLapCounter) + 1;

            carLapCounter.SetCarPosition(carPosition);

            // Actualizar el ranking dinámico mientras la carrera está en progreso
            boardUIHandler.UpdateList(carList.Select(car => new KeyValuePair<int, string>(carList.IndexOf(car) + 1, car.gameObject.name)).ToList());
        }
    }
}
    




