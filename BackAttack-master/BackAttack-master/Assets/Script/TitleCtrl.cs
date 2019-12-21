using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleCtrl : MonoBehaviour
{
    [SerializeField] private List<StageData> StageList;
    [SerializeField] private GameObject Button_Next;
    [SerializeField] private GameObject Button_Before;
    [SerializeField] private GameObject MouseEffect;
    private int MaxPage;
    private int CurrentPage = 0;


    private void Start()
    {
        CurrentPage = PlayerPrefs.GetInt("Page", 0);
        MaxPage = DataManager.Instance.GetStageCount() / StageList.Count;

        DataManager.Instance.TitleInit();
        FadeInOut.Instance.FadeOut(() => DataManager.Instance.IsLock = false);
        SoundManager.Instance.Play_Title_Bgm();
        SetPage();
        SetShowButton();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject obj = Instantiate(MouseEffect) as GameObject;
            obj.transform.SetParent(this.transform);
            obj.transform.position = Input.mousePosition;
            SoundManager.Instance.Play_Gun();
        }
    }

    private void SetPage()
    {
        PlayerPrefs.SetInt("Page", CurrentPage);
        PlayerPrefs.Save();

        for (int i = 0; i < StageList.Count; i++)
        {
            int StageID = i + (CurrentPage * StageList.Count) + 1;
            StageList[i].SetStage(StageID, DataManager.Instance.GetStageData(StageID).IsClear);
        }
    }
    private void SetShowButton()
    {
        Button_Next.SetActive(true);
        Button_Before.SetActive(true);

        //if (CurrentPage >= MaxPage - 1) Button_Next.SetActive(false);
        if (CurrentPage <= 0) Button_Before.SetActive(false);
    }

    public void NextPage()
    {
        if (CurrentPage >= MaxPage - 1)
        {
            DataManager.Instance.OpenPopup(PopUpCtrl.ePopupType.CommingSoon , true);
            return;
        }
        CurrentPage++;
        SetPage();
        SetShowButton();
    }

    public void BeforePage()
    {
        if (CurrentPage <= 0)  return;

        CurrentPage--;
        SetPage();
        SetShowButton();
    }
  
}
