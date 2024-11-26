using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour, IBoxObserver
{
    public static LevelManager Instance;
    public GameObject canvasTrafficLight;
    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    public void OnBoxEntered(EntityType type, CarUpgrades carUpgrades)
    {
        if(type == EntityType.Ai) return;
        Time.timeScale = 0f;
    }

    public void OnBoxExit(EntityType type, CarUpgrades carUpgrades)
    {
        if (type == EntityType.Ai) return;
        Time.timeScale = 1f;
    }

    // Start is called before the first frame update
    public void LightSet(Image[] image, GameObject canvasToDeactivate)
    {
        StartCoroutine(ManageLights(image, canvasToDeactivate));
    }

    private IEnumerator ManageLights(Image[] image, GameObject canvasToDeactivate)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(.5f);

        image[0].color = Color.red;

        yield return new WaitForSecondsRealtime(1f);

        image[1].color = Color.red;

        yield return new WaitForSecondsRealtime(1f);

        foreach (var light in image)
        {
            light.color = Color.green;
        }
        yield return new WaitForSecondsRealtime(.5f);

        Time.timeScale = 1f;

        canvasToDeactivate.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
