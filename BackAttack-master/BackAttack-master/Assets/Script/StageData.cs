using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class StageData : MonoBehaviour
{
    [SerializeField] GameObject Clear;
    [SerializeField] GameObject NoClear;
    [SerializeField] Text TextStage;

    private int StageID;
    private int IsClear;
    private PopUpCtrl Pc;
    private void Awake()
    {
        Pc = GameObject.Find("PopUp").GetComponent<PopUpCtrl>();
        //this.GetComponent<Button>().onClick.AddListener(() => ShowPopUp(StageID, IsClear));
        this.GetComponent<Button>().onClick.AddListener(() => Check(IsClear, StageID));

    }

    public void SetStage(int stageNumber, int isClear)
    {
        StageID = stageNumber;
        IsClear = isClear;

        TextStage.text = StageID.ToString();

        if(StageID == 1)
        {
            Clear.SetActive(true);
            NoClear.SetActive(false);

            if (IsClear == 1)
            {
                Clear.GetComponentInChildren<Text>().enabled = true;
            }
            else
            {
                Clear.GetComponentInChildren<Text>().enabled = false;
            }
        }
        else
        {
            if (DataManager.Instance.GetStageData(StageID - 1).IsClear == 0)
            {
                Clear.SetActive(false);
                NoClear.SetActive(true);
            }
            else
            {
                Clear.SetActive(true);
                NoClear.SetActive(false);
                if (IsClear == 1)
                {
                    Clear.GetComponentInChildren<Text>().enabled = true;
                }
                else
                {
                    Clear.GetComponentInChildren<Text>().enabled = false;
                }
            }
        }
    }

    public void StageChange()
    {
        if (StageID != 1 && DataManager.Instance.GetStageData(StageID - 1).IsClear == 0) return;

        DataManager.Instance.Selected_StageID = StageID;
        FadeInOut.Instance.FadeIn(() => SceneManager.LoadScene("Scene_Game"), FadeInOut.eFadeActiveOption.None);        
    }
    public void Check(int isClear, int Stage)
    {
        Pc.CheckPopUp(isClear, StageID);
    }
}