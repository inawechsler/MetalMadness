using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
public class LeadeBoardUIHandler : MonoBehaviour
{
    public GameObject leaderboardItem;
    CarLapCounter[] carLapCountersArr;
    SetLeaderBoardInfo[] setLeaderBoardInfos;
    [SerializeField] private Sprite Mud, Ice, Speed, Temp;

    [SerializeField] private Image carImage;
    [SerializeField] private Image AIboostImage;
    [SerializeField] private Image PJUpgradeImage;
    [SerializeField] private Image PJBoostImage;
    int carCount;
    // Start is called before the first frame update
    void Start()
    {
        VerticalLayoutGroup layoutGroup = GetComponentInChildren<VerticalLayoutGroup>();

        carLapCountersArr = CarRankingManager.Instance.carList.ToArray();

        carCount = carLapCountersArr.Length;

        setLeaderBoardInfos = new SetLeaderBoardInfo[carLapCountersArr.Length];

        for (int i = 0; i < carLapCountersArr.Length; i++)
        {
            GameObject leaderboardInfoGO = Instantiate(leaderboardItem, layoutGroup.transform);

            setLeaderBoardInfos[i] = leaderboardInfoGO.GetComponent<SetLeaderBoardInfo>();

            setLeaderBoardInfos[i].SetPosText(i + 1);
        }
    }
    public void UpdateImage(IUpgrade upgrade, CarUpgrades controller)
    {
        switch (upgrade)
        {
            case SpeedBoost:
                Temp = Speed;
                break;
            case WheelsChains:
                Temp = Mud;
                break;
            case WheelsSpikes:
                Temp = Ice;
                break;

        }
        

        if (controller.CompareTag("AI"))
        {
            carImage.sprite = controller.gameObject.GetComponent<SpriteRenderer>().sprite;
            AIboostImage.sprite = Temp;
        } else
        {
            if(Temp == Speed)
            {
                PJBoostImage.sprite = Temp;
            }else
            {
                PJUpgradeImage.sprite = Temp;
            }

        }

    }

    public void UpdateList(List<KeyValuePair<int, string>> rankingList)
    {
        // Limpia los elementos antiguos de la UI
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Actualiza la UI basándose en el ranking
        var limitedRankingList = rankingList.Take(carCount);

        // Actualiza la UI con el ranking limitado a la cantidad de autos
        int position = 1;
        foreach (var rank in limitedRankingList)
        {
            GameObject leaderboardInfoGO = Instantiate(leaderboardItem, transform);
            SetLeaderBoardInfo info = leaderboardInfoGO.GetComponent<SetLeaderBoardInfo>();

            info.SetDriverText(rank.Value);  // Nombre del coche
            info.SetPosText(position);       // Posición 
            position++;
        }
    }
}
