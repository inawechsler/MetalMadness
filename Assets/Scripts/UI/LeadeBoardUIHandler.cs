using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LeadeBoardUIHandler : MonoBehaviour
{
    public GameObject leaderboardItem;
    CarLapCounter[] carLapCountersArr;
    SetLeaderBoardInfo[] setLeaderBoardInfos;
    // Start is called before the first frame update
    void Start()
    {
        VerticalLayoutGroup layoutGroup = GetComponentInChildren<VerticalLayoutGroup>();

        carLapCountersArr = CarRankingManager.Instance.carList.ToArray();

        setLeaderBoardInfos = new SetLeaderBoardInfo[carLapCountersArr.Length];

        for (int i = 0; i < carLapCountersArr.Length; i++)
        {
            GameObject leaderboardInfoGO = Instantiate(leaderboardItem, layoutGroup.transform);

            setLeaderBoardInfos[i] = leaderboardInfoGO.GetComponent<SetLeaderBoardInfo>();

            setLeaderBoardInfos[i].SetPosText(i + 1);
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
        foreach (var rank in rankingList)
        {
        
            // Instanciar un nuevo elemento para cada posición en el ranking
            GameObject leaderboardInfoGO = Instantiate(leaderboardItem, transform);

            SetLeaderBoardInfo info = leaderboardInfoGO.GetComponent<SetLeaderBoardInfo>();
            info.SetDriverText(rank.Value);  // Nombre del coche
            info.SetPosText(rank.Key);       // Posición en el ranking
        }
    }
}
