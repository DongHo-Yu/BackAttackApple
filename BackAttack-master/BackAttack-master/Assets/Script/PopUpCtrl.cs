using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpCtrl : MonoBehaviour {
    public enum ePopupType
    {
        Clear,
        NoClear,
        Advertise,
        Quit , 
        CommingSoon,
        NoAds
    }
    //public delegate void PopUPHandler();
    //public event PopUPHandler PopUpEvent;

    public GameObject ClearPopUp;
    public GameObject NoClearPopUp;
    public GameObject AdvertiseMentPopUp;
    public GameObject QuitPopUp;
    public GameObject MenuPopUp;

    public GameObject CommingSoonPopUp;
    public GameObject NoAdsPopUp;

    //public GameObject Dim;

    private PopUpButtons popupButton;

    private string ReadyText;

    private void OnEnable()
    {
        AllPopUpActive(false);
    }
    private void Start()
    {
        popupButton = GetComponent<PopUpButtons>();
        ReadyText = ClearPopUp.GetComponentInChildren<Text>().text;
    }
    public void AllPopUpActive(bool value)
    {
        ClearPopUp.SetActive(value);
        NoClearPopUp.SetActive(value);
        AdvertiseMentPopUp.SetActive(value);
        QuitPopUp.SetActive(value);
        MenuPopUp.SetActive(value);
        CommingSoonPopUp.SetActive(value);
        NoAdsPopUp.SetActive(value);
        //Dim.SetActive(value);
    }

    public bool IsPopUp()
    {
        if (ClearPopUp.activeSelf || NoClearPopUp.activeSelf || AdvertiseMentPopUp.activeSelf || QuitPopUp.activeSelf || MenuPopUp.activeSelf || CommingSoonPopUp.activeSelf || NoAdsPopUp.activeSelf)
        {
            return true;
        }
        return false;
    }
    public void PopUpActive(GameObject PopUp, int StageID = -1, bool value = true)
    {
        AllPopUpActive(false);
        PopUp.SetActive(value);
        if (StageID != -1)
        {
            PopUp.GetComponentInChildren<Text>().text = ReadyText.Replace("%d", StageID.ToString());
        }
    }
    public void PopUpActive(ePopupType type , bool Active)
    {
        AllPopUpActive(false);
        switch (type)
        {
            case ePopupType.Clear:
                ClearPopUp.SetActive(Active);
                break;
            case ePopupType.NoClear:
                NoClearPopUp.SetActive(Active);
                break;
            case ePopupType.Advertise:
                AdvertiseMentPopUp.SetActive(Active);
                break;
            case ePopupType.Quit:
                QuitPopUp.SetActive(Active);
                break;
            case ePopupType.CommingSoon:
                CommingSoonPopUp.SetActive(Active);
                break;
            case ePopupType.NoAds:
                NoAdsPopUp.SetActive(Active);
                break;
        }
    }




    public void CheckPopUp(int isClear,int StageID)
    {
        if (StageID == 1)
        {
            PopUpActive(ClearPopUp,1);
            popupButton.StageID = StageID;
            DataManager.Instance.Selected_StageID = StageID;
        }
        else
        {
            if (DataManager.Instance.GetStageData(StageID -1).IsClear == 1)
            {
                PopUpActive(ClearPopUp , StageID);
                popupButton.StageID = StageID;
            }
            else
            {
                PopUpActive(NoClearPopUp);
            }
        }

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
