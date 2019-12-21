using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PopUpButtons : MonoBehaviour
{ 
    public Button ClearYes;
    public Button ClearNo;

    public Button NoClearOk;

    public Button AdvertiseMentYes;
    public Button AdvertiseMentNo;

    public Button CommingSoon;
    public Button NoAds;

    private PopUpCtrl Pc;

    [HideInInspector] public int StageID;

    public void Awake()
    {
        Pc = GetComponent<PopUpCtrl>();

        ClearNo.onClick.AddListener(() => Pc.PopUpActive(Pc.ClearPopUp, -1, false));
        NoClearOk.onClick.AddListener(() => Pc.PopUpActive(Pc.NoClearPopUp, -1, false));

        CommingSoon.onClick.AddListener(() => Pc.PopUpActive(Pc.CommingSoonPopUp, -1, false));
        NoAds.onClick.AddListener(() => Pc.PopUpActive(Pc.NoAdsPopUp, -1, false));

        AdvertiseMentNo.onClick.AddListener(() => Pc.PopUpActive(Pc.AdvertiseMentPopUp, -1, false));
        AdvertiseMentYes.onClick.AddListener(() =>
        {
            Pc.AllPopUpActive(false);
            bool t = UnityAdsManager.instance.ShowAds(
         () => DataManager.Instance.HeartAmount = 5, // 제대로 광고본년들 보상
         () => DataManager.Instance.OpenPopup(PopUpCtrl.ePopupType.NoAds, true), // 광고 보려고는 햇으나 제대로 광고가 나오지 않은 의리잇는놈들
         () => DataManager.Instance.OpenPopup(PopUpCtrl.ePopupType.NoAds, true)  // 스킵한 개씌벌럼들 
         );
        });
    }

    public void ClearYesClick()
    {
        if (DataManager.Instance.HeartAmount <= 0)
        {
            Pc.PopUpActive(Pc.ClearPopUp, -1, false);
            Pc.PopUpActive(Pc.AdvertiseMentPopUp, -1, true);
        }
        else
        {
            //    System.Action Func = () =>
            //{
            Pc.PopUpActive(Pc.ClearPopUp, -1, false);
            DataManager.Instance.Selected_StageID = StageID;
            FadeInOut.Instance.FadeIn(() => SceneManager.LoadScene("Scene_Game"), FadeInOut.eFadeActiveOption.None);
            DataManager.Instance.IsLock = true;
            //};
            //if(StageID > 5)
            //{
            //UnityAdsManager.instance.ShowAds(Func, Func, Func);
            //}
            //else
            //{
            //    Func();
            //}
            ////}
        }
    }


    public void ClearNoClick()
    {
        Pc.PopUpActive(Pc.NoClearPopUp, -1 ,false);
    }
}