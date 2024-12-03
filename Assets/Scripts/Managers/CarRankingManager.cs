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
    public List<CarLapCounter> carList { get; private set; } = new List<CarLapCounter>();

    public Dictionary<int, string> ranking = new Dictionary<int, string>();

    private int lapsCompleted;
    string text;
    public int LapsCompleted => lapsCompleted;

    private int lapsToComplete = 2;

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


    static public int Partition(List<CarLapCounter> arr, int left, int right)
    {
        // Comprobar que left y right est�n dentro de los l�mites de la lista
        if (arr == null || arr.Count == 0 || left < 0 || right >= arr.Count || left > right)
        {
            return -1;  // Indicar que no se puede proceder
        }

        CarLapCounter pivot = arr[(left + right) / 2];  // Tomamos el auto en el centro como pivote

        while (left <= right) // Modificar para que el ciclo no se repita indefinidamente
        {
            // Buscar un elemento mayor que el pivote en la izquierda
            while (arr[left].lapsCompleted > pivot.lapsCompleted ||
                   (arr[left].lapsCompleted == pivot.lapsCompleted && arr[left].PassedCheckPointNumber > pivot.PassedCheckPointNumber) ||
                   (arr[left].lapsCompleted == pivot.lapsCompleted && arr[left].PassedCheckPointNumber == pivot.PassedCheckPointNumber && arr[left].TimeAtLastCheckPointPassed < pivot.TimeAtLastCheckPointPassed))
            {
                left++;
            }

            // Buscar un elemento menor que el pivote en la derecha
            while (arr[right].lapsCompleted < pivot.lapsCompleted ||
                   (arr[right].lapsCompleted == pivot.lapsCompleted && arr[right].PassedCheckPointNumber < pivot.PassedCheckPointNumber) ||
                   (arr[right].lapsCompleted == pivot.lapsCompleted && arr[right].PassedCheckPointNumber == pivot.PassedCheckPointNumber && arr[right].TimeAtLastCheckPointPassed > pivot.TimeAtLastCheckPointPassed))
            {
                right--;
            }

            if (left <= right)
            {
                // Intercambiar elementos
                CarLapCounter temp = arr[right];
                arr[right] = arr[left];
                arr[left] = temp;
                left++;
                right--;
            }
        }

        return left;  // Regresar left como el nuevo �ndice de partici�n
    }

    static public void QuickSort(List<CarLapCounter> arr, int left, int right)
    {
        if (left < right)
        {
            int pivot = Partition(arr, left, right); // Obtener �ndice de partici�n

            if (left < pivot - 1) // Verificar que el rango tiene m�s de un elemento
            {
                QuickSort(arr, left, pivot - 1); // Recursi�n en la mitad izquierda
            }

            if (pivot < right) // Verificar que el rango tiene m�s de un elemento
            {
                QuickSort(arr, pivot, right); // Recursi�n en la mitad derecha
            }
        }
    }




    void OnPassCheckPoint(CarLapCounter carLapCounter)
    {
        if (carLapCounter.lapsCompleted >= LapsToComplete)
        {
            if (!carLapCounter.isRaceCompleted) // Solo si a�n no termin� la carrera
            {
                carLapCounter.isRaceCompleted = true;
                carsFinishedRace++; // Incrementar cuando un auto completa la carrera

                // Finalizar el ranking para este auto
                int finalPosition = carList.IndexOf(carLapCounter) + 1;

                if (!ranking.ContainsKey(finalPosition))
                {
                    ranking.Add(finalPosition, carLapCounter.gameObject.name);
                }
                else
                {
                    while (ranking.ContainsKey(finalPosition)) //Si contiene el puesto que quiere usar, aumenta hasta que no haya nada y lo guarda ah�
                    {
                        finalPosition++;
                    }
                    ranking.Add(finalPosition, carLapCounter.gameObject.name);
                }

                // Actualizar la UI
                boardUIHandler.UpdateList(ranking.ToList());

                Debug.Log($"Carros terminados: {carsFinishedRace} / {carList.Count}");

                // Si todos los autos terminaron, cambiar de escena
                if (carList.First(car => car.gameObject.tag == "Player" && car.lapsCompleted == lapsToComplete))
                {
                    SceneManager.LoadScene("LevelResume");
                }
            }
        }
        else
        {
            QuickSort(carList, 0, carList.Count - 1);

            /* carList = carList.OrderByDescending(car => car.lapsCompleted) //Ordnena primero de mayor a menor en base a las vueltas
                              .ThenByDescending(car => car.PassedCheckPointNumber) // Si son iguales lo hace en base a quien tiene mas checkpoints
                              .ThenBy(car => car.TimeAtLastCheckPointPassed) //Si son iguales lo hace en base al tiempo en el que pasaron el checkpoint
                              .ToList(); //Lo hace lista */

            ranking.Clear();

            for (int i = 0; i < carList.Count; i++)
            {
                ranking[i + 1] = carList[i].gameObject.name;
            }

            // Actualizar la UI con el ranking actual
            boardUIHandler.UpdateList(ranking.ToList());

            // Actualizar posici�n individual del auto
            carPosition = carList.IndexOf(carLapCounter) + 1;

            carLapCounter.SetCarPosition(carPosition);

            // El líder actual es el primer auto en la lista ordenada
            var currentLeader = carList.FirstOrDefault();

            // Verifica si el líder ha completado al menos una vuelta
            if (currentLeader != null && currentLeader.lapsCompleted > 0)
            {
                // Solo ejecuta si el líder actual cambia
                if (!lapCompletedTriggered)
                {
                    lapCompletedTriggered = true;
                    StateManager.Instance.OnLapCompleted();
                }
            }

            // Restablecer el triggereo para la pr�xima vuelta
            if (carLapCounter.PassedCheckPointNumber == 0)
            {
                lapCompletedTriggered = false;
            }

        }
    }

}
    




