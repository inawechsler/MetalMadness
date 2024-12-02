using System;
using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    public static TrackManager Instance;

    [SerializeField] private List<SpriteRenderer> SpritesToUse = new List<SpriteRenderer>();
    private List<SpriteRenderer> Track = new List<SpriteRenderer>();

    // Diccionario para mapear IDs con las imágenes de SpritesToUse
    private Dictionary<int, Sprite> imageID = new Dictionary<int, Sprite>();

    // Diccionario para mapear estados con imágenes en Track
    private Dictionary<string, Dictionary<int, Sprite>> stateImageMap = new Dictionary<string, Dictionary<int, Sprite>>();

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
    void Start()
    {
        foreach (var item in new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>()))
        {
            if (item.GetComponent<StateImage>() != null)
            {
                if (item.GetComponent<StateImage>().state == State.NoCurrent)
                {
                    Track.Add(item);
                }
                else
                {
                    SpritesToUse.Add(item);
                }

            }
        }

        Debug.Log(Track.Count + "TR" + SpritesToUse.Count);
        //AssignIdToDict();
        //AssignStateToDict();

    }
    //void AssignIdToDict()
    //{
    //    foreach (var item in Track)
    //    {
    //        foreach (var toUse in SpritesToUse)
    //        {
    //            var itemImage = item.GetComponent<StateImage>();
    //            var toUseImage = toUse.GetComponent<StateImage>();
    //            if (itemImage.ID == toUseImage.ID)
    //            {
    //                imageID[toUseImage.ID] = toUse.sprite;
    //                Debug.Log($"Mapped ID {toUseImage.ID} to Sprite {toUse.sprite.name}");
    //            }
    //        }
    //    }
    //}

    //void AssignStateToDict()
    //{
    //    foreach (var item in SpritesToUse)
    //    {
    //        var imageState = item.GetComponent<StateImage>();

    //        if (imageState != null)
    //        {
    //            // Inicializar los diccionarios para cada estado
    //            if (!stateImageMap.ContainsKey("SlowState"))
    //            {
    //                stateImageMap.Add("SlowState", new Dictionary<int, Sprite>());
    //            }

    //            if (!stateImageMap.ContainsKey("SlippyState"))
    //            {
    //                stateImageMap.Add("SlippyState", new Dictionary<int, Sprite>());
    //            }
    //            // Asignar imágenes según el estado y el ID
    //            if (imageState.state == State.SlowState && imageID.TryGetValue(imageState.ID, out Sprite mudSprite))
    //            {
    //                stateImageMap["SlowState"][imageState.ID] = mudSprite;
    //            }
    //            else if (imageState.state == State.SlippyState && imageID.TryGetValue(imageState.ID, out Sprite iceSprite))
    //            {
    //                stateImageMap["SlippyState"][imageState.ID] = iceSprite;

    //            }
    //        }
    //    }
    //}

    public void SetStateImage(IState state, StateCollider stateCollider)
    {
        foreach (var track in Track)
        {
            var trackImage = track.GetComponent<StateImage>();
            if (trackImage.StateColliderID == stateCollider.ID)
            {
                trackImage.SetState(state);
            }
        }
    }
}

