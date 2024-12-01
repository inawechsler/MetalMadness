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

    Button localButtonTemp;

    private List<IUpgrade> upgradeList = new List<IUpgrade>();

    private Dictionary<string, IUpgrade> upgradeDictionary = new Dictionary<string, IUpgrade>();

    IUpgrade currentUpgrade;

    private Image selectedImage;

    private bool hasExited = false;
    public UnityEvent onClickedPurchased { get; private set; } = new UnityEvent();

    CarUpgrades carUpgrades;

    LeadeBoardUIHandler boardUIHandler;

    private Stack<ShopMemento> shopMementoStack = new Stack<ShopMemento>(5);

    private void Awake()
    {
        purchaseButton.onClick.AddListener(BoxExitDispatcher);

        carUpgrades = GameObject.FindWithTag("Player").GetComponent<CarUpgrades>();


        selectedImage = GameObject.FindWithTag("SelectedImage").GetComponent<Image>();

        foreach(var button in upgradeButtons)
        {
            button.onClick.AddListener(delegate { ManageButton((button.gameObject.name), button); }); //Por cada boton subscribe en el click a ManageButton pasando como parametros el nombre del boton y el Button como tal
        }

        boxCanvas = GameObject.FindWithTag("ShopCanvas");

        boxCanvas.SetActive(false);

        boardUIHandler = FindAnyObjectByType<LeadeBoardUIHandler>();

        selectedImage.gameObject.SetActive(false);
    }

    public void OnBoxEntered(EntityType type, CarUpgrades carUpgrades)
    {
        if (type == EntityType.Ai) return; //Si recibe enum AI se va
        boxCanvas.SetActive(true);

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
 
        AssignUpgrade(currentUpgrade);
        boxCanvas.SetActive(false);
        onClickedPurchased?.Invoke(); //Invoco el evento llamado en BoxShop
        hasExited = true;

        ShopMemento savedMemento = SaveState(currentUpgrade.GetType().Name, localButtonTemp);
        shopMementoStack.Push(savedMemento);
    }
    void ManageButton(string upgradeToApply, Button button) //Se encarga de mostrar el objeto seleccionado y guardar el upgrade seleccionado, no lo aplica, solo lo guarda
    {
        if (upgradeDictionary.TryGetValue(upgradeToApply, out IUpgrade upgrade))
        {
            currentUpgrade = upgrade;

            if(button != null)
            {
                localButtonTemp = button;

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

    private ShopMemento SaveState(string upgrade, Button button)
    {
        string upgradeToSave = upgrade;
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
