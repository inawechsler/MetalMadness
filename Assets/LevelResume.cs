using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelResume : MonoBehaviour
{
    public Dictionary<int, string> ranking;
    [SerializeField] private Button button;
    [SerializeField] TextMeshProUGUI[] textMeshPros;

    void Start()
    {
        button.onClick.AddListener(LoadNextRace);

        ranking = LevelManager.Instance.ranking;
    }

    void Update()
    {
        // Reinicia el índice en cada actualización
        int i = 0;

        // Itera sobre el diccionario y actualiza los textos
        foreach (var car in ranking)
        {
            if (i >= textMeshPros.Length)
            {
                // Si el diccionario tiene más elementos que los textos disponibles, rompe el bucle
                break;
            }

            textMeshPros[i].text = $"{car.Key}. {car.Value}";
            i++;
        }

        // Si hay menos entradas en el diccionario que textos, limpia los textos restantes
        for (; i < textMeshPros.Length; i++)
        {
            textMeshPros[i].text = "";
        }
    }

    void LoadNextRace()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Race2");
    }
}