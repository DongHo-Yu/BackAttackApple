using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartCtrl : MonoBehaviour
{

    public List<GameObject> HeartImages;
    public Text TimeText;


    private WaitForSeconds UpdateTime = new WaitForSeconds(1.0f);


    private void Start()
    {
        if (DataManager.Instance.HeartAmount >= 5)
        {
            TimeText.gameObject.SetActive(false);
        }

        TimeText.text = DataManager.Instance.Minute.ToString("00") + ":" + DataManager.Instance.Second.ToString("00");
        StartCoroutine(TimeCalc());
        ShowHeartImage(DataManager.Instance.HeartAmount);
    }

    IEnumerator TimeCalc()
    {
        yield return UpdateTime;

        if(DataManager.Instance.HeartAmount < 5)
        {
            if(DataManager.Instance.Second == 0 && DataManager.Instance.Minute ==0)
            {
                DataManager.Instance.Second = 0;
                DataManager.Instance.Minute = 10;
                DataManager.Instance.HeartAmount++;
            }
            if(DataManager.Instance.Second > 0)
            {
                DataManager.Instance.Second -= 1;
                
            }
            else
            {
                DataManager.Instance.Second = 59;
                DataManager.Instance.Minute -= 1;
            }
            TimeText.text = DataManager.Instance.Minute.ToString("00") + ":" + DataManager.Instance.Second.ToString("00");
        }
        else
        {
            TimeText.text = "";
        }

        StartCoroutine(TimeCalc());
    }

    public void ShowHeartImage(int amount)
    {
        for (int i = 0; i < HeartImages.Count; i++)
        {
            HeartImages[i].transform.GetChild(1).gameObject.SetActive(false);
        }
        for (int i = 0; i < amount; i++)
        {
            HeartImages[i].transform.GetChild(1).gameObject.SetActive(true);
        }
    }
}


