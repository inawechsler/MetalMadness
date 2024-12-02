using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;
using TMPro;

public class BoxCanvasManager : MonoBehaviour, IBoxObserver
{
    private GameObject boxCanvas;
    [SerializeField] Button purchaseButton;
    [SerializeField] List<Button> upgradeButtons;
    Button Exit;


    private List<UpgradeItem> upgradeItemList = new List<UpgradeItem>();

    Button localButtonTemp;

    private List<IUpgrade> upgradeList = new List<IUpgrade>();

    private Dictionary<string, IUpgrade> upgradeDictionary = new Dictionary<string, IUpgrade>();

    IUpgrade currentUpgrade;

    private Image selectedImage;

    private bool hasExited = false;
    public UnityEvent onClickedPurchased { get; private set; } = new UnityEvent();

    CarUpgrades carUpgrades;

    LeadeBoardUIHandler boardUIHandler;

    private TextMeshProUGUI engineText;

    private Stack<ShopMemento> shopMementoStack = new Stack<ShopMemento>(5);

    private void Start()
    {
        Exit = GameObject.FindWithTag("ExitCanvas").GetComponent<Button>();

        upgradeItemList = GameObject.FindWithTag("ShopCanvas").GetComponentsInChildren<UpgradeItem>().ToList();

        carUpgrades = GameObject.FindWithTag("Player").GetComponent<CarUpgrades>();

        engineText = GameObject.FindWithTag("EngineText").GetComponent<TextMeshProUGUI>();

        selectedImage = GameObject.FindWithTag("SelectedImage").GetComponent<Image>();

        boxCanvas = GameObject.FindWithTag("ShopCanvas");

        boxCanvas.SetActive(false);

        boardUIHandler = FindAnyObjectByType<LeadeBoardUIHandler>();

        selectedImage.gameObject.SetActive(false);



        purchaseButton.onClick.AddListener(BoxExitDispatcher);

        Exit.onClick.AddListener(BoxExitDispatcher);

        foreach (var item in upgradeItemList)
        {
            if (item.Button == null) Debug.Log(item.gameObject.name);
            item.Button.onClick.AddListener(delegate { ManageButton(item.upgrade, item.Button); }); //Por cada boton subscribe en el click a ManageButton pasando como parametros el nombre del boton y el Button como tal

        }

    }



    public void OnBoxEntered(EntityType type, CarUpgrades carUpgrades)
    {
        if (type == EntityType.Ai) return; //Si recibe enum AI se va

 
        boxCanvas.SetActive(true);

        engineText.text = LevelManager.Instance.enginePiecesCollected.ToString();

        foreach(var item in upgradeItemList)
        {
            if(LevelManager.Instance.enginePiecesCollected <= item.priceOnShop)
            {
                Debug.Log(item.priceOnShop + item.gameObject.name + " " + LevelManager.Instance.enginePiecesCollected);
                item.Button.interactable = false;
            } else
            {
                item.Button.interactable = true;

            }
        }

        hasExited = false;

        foreach(var upgrade in FindObjectsOfType<MonoBehaviour>().OfType<IUpgrade>().ToList())
        {
            upgradeList.Add(upgrade); //Por cada IUpgrade lo agrega a upgradeList
        }

        foreach (var upgrade in upgradeList)
        {
            if (upgradeDictionary.ContainsKey(upgrade.GetType().Name))
            {
                continue; //Si tiene la Key, pasa al siguiente
            }

            upgradeDictionary.Add(upgrade.GetType().Name, upgrade); //Agrega al diccionario el nombre del upgrade y el IUpgrade

        }
        StartCoroutine(WaitForRestoreKey());
    }
    void AssignUpgrade(IUpgrade upgrade) 
    {
        carUpgrades.AddUpgrade(upgrade);
        boardUIHandler.UpdateImage(upgrade, carUpgrades);
    }

    void BoxExitDispatcher()//En purchase llamo a este evento
    {
        OnBoxExit(EntityType.Player, carUpgrades); //LLama a OnBoxExit

        selectedImage.gameObject.SetActive(false);
    }

    public void OnBoxExit(EntityType type, CarUpgrades carUpgrades)//Lógica de asignado de upgrade y Oculto el canvas
    {
        if (type == EntityType.Ai) return;
        if(hasExited) return;

        onClickedPurchased?.Invoke(); //Invoco el evento llamado en BoxShop
        boxCanvas.SetActive(false);
        hasExited = true;
        if (currentUpgrade == null) return;

        AssignUpgrade(currentUpgrade);


        ShopMemento savedMemento = SaveState(currentUpgrade, localButtonTemp);
        shopMementoStack.Push(savedMemento);

        var upgradeItem = upgradeItemList.First(item => item.upgrade == currentUpgrade);

        if (upgradeItem != null)
        {
            LevelManager.Instance.SpendEnginePieces(upgradeItem.priceOnShop);
        }
    }
    void ManageButton(IUpgrade upgradeToApply, Button button) //Se encarga de mostrar el objeto seleccionado y guardar el upgrade seleccionado, no lo aplica, solo lo guarda
    {
        currentUpgrade = upgradeToApply;

        if (button != null)
        {
            localButtonTemp = button;

            RectTransform buttonRectTransform = button.GetComponent<RectTransform>();

            Vector2 buttonWorldPosition = buttonRectTransform.position;

            selectedImage.rectTransform.position = new Vector3(buttonWorldPosition.x + 38.7f, buttonWorldPosition.y + 42f);

            selectedImage.gameObject.SetActive(true);
        }
    }


    private IEnumerator WaitForRestoreKey()
    {
        while (boxCanvas.activeSelf) // Solo escucha mientras el Canvas está activo
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (!shopMementoStack.isEmpty())
                {
                    ShopMemento lastMemento = shopMementoStack.Pop(); // Obtén el último memento
                    RestoreState(lastMemento); // Restaura el estado
                }
                else
                {
                    Debug.Log("No hay estados guardados para restaurar.");
                }
            }
            yield return null; // Espera un frame antes de continuar
        }
    }

    private ShopMemento SaveState(IUpgrade upgrade, Button button)
    {
        IUpgrade upgradeToSave = upgrade;
        Button buttonToSave = button;

        return new ShopMemento(upgradeToSave, buttonToSave);
    }

    private void RestoreState(ShopMemento upgradeToRestore)
    {
        if (upgradeToRestore == null)
        {
            Debug.LogError("El estado guardado es null. No se puede restaurar.");
            return;
        }
        if (upgradeToRestore.upgrade is null) Debug.Log("pgrade");
        if (upgradeToRestore.button is null) Debug.Log("button");
        //Debug.Log(upgradeToRestore.upgrade.GetType().Name + " " + upgradeToRestore.button.gameObject.name);
        ManageButton(upgradeToRestore.upgrade, upgradeToRestore.button);

    }
}
