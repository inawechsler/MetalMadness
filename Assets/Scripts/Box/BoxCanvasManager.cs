using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

public class BoxCanvasManager : MonoBehaviour, IBoxObserver
{
    private GameObject boxCanvas;
    [SerializeField] Button purchaseButton;
    [SerializeField] List<Button> upgradeButtons;
   
    private List<IUpgrade> upgradeList = new List<IUpgrade>();

    private Dictionary<string, IUpgrade> upgradeDictionary = new Dictionary<string, IUpgrade>();

    IUpgrade currentUpgrade;

    private Image selectedImage;

    private bool hasExited = false;
    public UnityEvent onClickedPurchased { get; private set; } = new UnityEvent();

    CarUpgrades carUpgrades;

    private void Awake()
    {
        purchaseButton.onClick.AddListener(OnBoxExit);

        carUpgrades = GameObject.FindWithTag("Player").GetComponent<CarUpgrades>();

        selectedImage = GameObject.FindWithTag("SelectedImage").GetComponent<Image>();

        //antiSlipperyState.onClick.AddListener( delegate { ManageButton("WheelsSpikes", antiSlipperyState);} );

        //antiSlowState.onClick.AddListener( delegate { ManageButton("WheelsChains", antiSlowState);} );

        //speedBost.onClick.AddListener( delegate { ManageButton("SpeedBoost", speedBost);} );

        foreach(var button in upgradeButtons)
        {
            button.onClick.AddListener(delegate { ManageButton((button.gameObject.name), button); });
        }

        boxCanvas = GameObject.FindWithTag("ShopCanvas");

        boxCanvas.SetActive(false);




        selectedImage.gameObject.SetActive(false);
    }

    public void OnBoxEntered()
    {
        boxCanvas.SetActive(true);

        hasExited = false;

        

        foreach(var upgrade in FindObjectsOfType<MonoBehaviour>().OfType<IUpgrade>().ToList())
        {
            upgradeList.Add(upgrade);
        }

        foreach (var upgrade in upgradeList)
        {
            if (upgradeDictionary.ContainsKey(upgrade.GetType().Name))
            {
                continue;
            }

            upgradeDictionary.Add(upgrade.GetType().Name, upgrade);

        }
    }
    void AssignUpgrade(IUpgrade upgrade)
    {
        carUpgrades.AddUpgrade(upgrade);
    }

    public void OnBoxExit()
    {
        if(hasExited) return;

        Debug.Log(currentUpgrade.GetType().Name);   
        AssignUpgrade(currentUpgrade);
        boxCanvas.SetActive(false);
        onClickedPurchased?.Invoke();
        hasExited = true;
    }
    void ManageButton(string upgradeToApply, Button button)
    {
        if (upgradeDictionary.TryGetValue(upgradeToApply, out IUpgrade upgrade))
        {
            currentUpgrade = upgrade;

            if(button != null)
            {
                RectTransform buttonRectTransform = button.GetComponent<RectTransform>();

                Vector2 buttonWorldPosition = buttonRectTransform.position;

                selectedImage.rectTransform.position = new Vector3(buttonWorldPosition.x + 38.7f, buttonWorldPosition.y + 42f);

                selectedImage.gameObject.SetActive(true);
            }
            else
            {
                Debug.Log("skd");
            }
        }


    }

}
