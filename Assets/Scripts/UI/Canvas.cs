using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class Canvas : MonoBehaviour, IBoxObserver
{
    //  Start is called before the first frame update
    [SerializeField] Image velocity;
    TopDownController car;
    [SerializeField] GameObject lightCanvas, canvas;
    [SerializeField] Image[] raceLights;



    public void OnBoxEntered(EntityType type, CarUpgrades carUpgrades)
    {
        if (type == EntityType.Ai) return;

        canvas.gameObject.SetActive(false);
    }

    public void OnBoxExit(EntityType type, CarUpgrades carUpgrades)
    {
        if (type == EntityType.Ai) return;

        canvas.gameObject.SetActive(true);
    }

    void Start()
    {
        velocity.fillAmount = 0;
        car = GameObject.FindGameObjectWithTag("Player").GetComponent<TopDownController>();

        LevelManager.Instance.LightSet(raceLights, lightCanvas);
    }
    //    Update is called once per frame
    void Update()
    {
        velocity.fillAmount = car.GetSpeed() / 20;
    }
}
