using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetLeaderBoardInfo : MonoBehaviour
{
    public TextMeshProUGUI posText, driverText;
    // Start is called before the first frame update
    void Start()
    {

        //sprite.gameObject.SetActive(false);
    }

    public void SetDriverText(string text)
    {

        driverText.text = text;
    }

    public void SetPosText(int pos)
    {
        posText.text = pos.ToString();
    }

}
