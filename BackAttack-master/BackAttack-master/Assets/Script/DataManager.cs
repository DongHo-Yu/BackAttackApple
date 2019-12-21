using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;




public partial class DataManager : Singleton<DataManager>
{
    [HideInInspector] public int Selected_StageID;
   [HideInInspector] public bool IsGameStart = false;
    [HideInInspector] public bool IsLock = false;

    private Stage[] StageData;
    private HeartCtrl hc;
    private PopUpCtrl pc;


    public bool IsAutoSave;
     [HideInInspector] public bool IsAds = false;

    protected override void Awake()
    {
        base.Awake();
//#if UNITY_ANDROID
        Application.targetFrameRate = 60;
//#endif

        JsonContainer.JsonLoad(out StageData);
    }

    private void Start()
    {      
        Init();
    }

    public void TitleInit()
    {
        hc = GameObject.Find("Heart").GetComponent<HeartCtrl>();
        pc = GameObject.Find("PopUp").GetComponent<PopUpCtrl>();

        IsGameStart = false;
        IsLock = false;
    }

    public int GetStageCount()
    {
        return StageData.Length;
    }

    public Stage GetStageData(int StageID)
    {
        return StageData[StageID - 1];
    }

    /// <summary>
    /// Auto Save
    /// </summary>
    public void SetStageData(int StageID, int IsClear)
    {
        StageData[StageID - 1].IsClear = IsClear;
        JsonHelper.SaveJson(StageData, "Json_Stage");
    }

    public void OpenPopup(PopUpCtrl.ePopupType popupType , bool Active)
    {
        pc.PopUpActive(popupType, Active);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && IsLock == false)
        {
            if(IsGameStart == false)
            {
                if (pc.IsPopUp())
                {
                    pc.AllPopUpActive(false);
                }
                else
                {
                    pc.PopUpActive(PopUpCtrl.ePopupType.Quit, true);
                }
            }
            else
            {
                IsLock = true;
                FadeInOut.Instance.FadeIn(() => SceneManager.LoadScene("Scene_Title"), FadeInOut.eFadeActiveOption.None);
            }
        }
         
    }    
}
public partial class DataManager //: Singleton<DataManager>
{
    private int m_heartamount;
    public int HeartAmount
    {
        get
        {
            //return m_heartamount;
            return PlayerPrefs.GetInt("HeartAmount", 5);
        }
        set
        {
            PlayerPrefs.SetInt("HeartAmount", value);
            //m_heartamount = 5;
            //m_heartamount = value;
            if (hc != null) hc.ShowHeartImage(HeartAmount);
            //PlayerPrefs.SetInt("HeartAmount", m_heartamount);
        }
    }

    private int m_minute;
    public int Minute
    {
        get
        {
            //return m_minute;
            return PlayerPrefs.GetInt("Minute", 9);
        }
        set
        {
            PlayerPrefs.SetInt("Minute", value);
            //if (m_minute > 10) m_minute = 10;
            //m_minute = value;
            //PlayerPrefs.SetInt("Minute", m_minute);
        }
    }

    private int m_second;
    public int Second
    {
        get
        {
            //return m_second;
            return PlayerPrefs.GetInt("Second", 59);
        }
        set
        {
            //if (m_second == 0 && Minute == 0)
            //{
            //    m_second = 0;
            //    Minute = 10;
            //    if (HeartAmount > 5)
            //        HeartAmount++;
            //}
            //if (m_second > 0)
            //{
            //    m_second = value;
            //}
            //else
            //{
            //    m_second = 59; // 씨발
            //    Minute -= 1;
            //}
            PlayerPrefs.SetInt("Second", value);
        }
    }

    private int m_ComPareHour;
    public int CompareHour
    {
        get
        {
            return PlayerPrefs.GetInt(s_Hour, DateTime.Now.Hour);
        }
        set
        {
            m_ComPareHour = value;
            PlayerPrefs.SetInt(s_Hour, m_ComPareHour);
        }
    }

    private int m_CompareMinute;
    public int CompareMinute
    {
        get
        {
            return PlayerPrefs.GetInt(s_Minute, DateTime.Now.Minute);
        }
        set
        {
            m_CompareMinute = value;
            PlayerPrefs.SetInt(s_Minute, m_CompareMinute);
        }
    }

    private int m_CompareSecond;
    public int CompareSecond
    {
        get
        {
            return PlayerPrefs.GetInt(s_Second, DateTime.Now.Second);
        }
        set
        {
            m_CompareSecond = value;
            PlayerPrefs.SetInt(s_Second, m_CompareSecond);
        }
    }

    private int m_CompareYear;
    public int CompareYear
    {
        get
        {
            return PlayerPrefs.GetInt(s_Year, DateTime.Now.Year);
        }
        set
        {
            m_CompareYear = value;
            PlayerPrefs.SetInt(s_Year, m_CompareYear);
        }
    }

    private int m_CompareMonth;
    public int CompareMonth
    {
        get
        {
            return PlayerPrefs.GetInt(s_Month, DateTime.Now.Month);
        }
        set
        {
            m_CompareMonth = value;
            PlayerPrefs.SetInt(s_Month, m_CompareMonth);
        }
    }

    private int m_CompareDay;
    public int CompareDay
    {
        get
        {
            return PlayerPrefs.GetInt(s_Day, DateTime.Now.Day);
        }
        set
        {
            m_CompareDay = value;
            PlayerPrefs.SetInt(s_Day, m_CompareDay);
        }
    }
    private const string StrHead = "Compare";

    private const string s_Year = StrHead + "Year";
    private const string s_Month = StrHead + "Month";
    private const string s_Day = StrHead + "Day";
    private const string s_Hour = StrHead + "Hour";
    private const string s_Minute = StrHead + "Minute";
    private const string s_Second = StrHead + "Second";

    private int timeCalDay;
    private int timeCalHour;
    private int timeCalMinute;
    private int timeCalSecond;

    [HideInInspector] public bool isBlood;
    private void Init()
    {
       // PlayerPrefs.DeleteAll();
        HeartAmount = PlayerPrefs.GetInt("HeartAmount", 5);
        Second = PlayerPrefs.GetInt("Second", 59);
        Minute = PlayerPrefs.GetInt("Minute", 9);
        TimeCompareCheck();
        HeatCalc();
    }
    void OnApplicationQuit()
    {
        DateTime QuitDateTime = DateTime.Now;

        CompareYear = QuitDateTime.Year;
        CompareMonth = QuitDateTime.Month;
        CompareDay = QuitDateTime.Day;
        CompareHour = QuitDateTime.Hour;
        CompareMinute = QuitDateTime.Minute;
        CompareSecond = QuitDateTime.Second;

        //PlayerPrefs.Save();
#if UNITY_EDITOR
        if (IsAutoSave == false)
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("PlayerPrefs Save Data All Delete !");
        }
#endif
    }

    void TimeCompareCheck()
    {
        DateTime StartDateTime = DateTime.Now;
        DateTime CompareDateTime = new DateTime(CompareYear, CompareMonth, CompareDay, CompareHour, CompareMinute, CompareSecond);

        //Test
        //DateTime StartDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 47, 04);
        //DateTime CompareDateTime = new DateTime(DateTime.Now.Year, 11, 26, DateTime.Now.Hour, 10, 38);
        System.TimeSpan timeCal = StartDateTime - CompareDateTime;

        timeCalDay = timeCal.Days;
        timeCalHour = timeCal.Hours;
        timeCalMinute = timeCal.Minutes;
        timeCalSecond = timeCal.Seconds;
    }

    public void HeatCalc()
    {
        int TempSecond = 0;
        int TempMinute = 0;
        int HeartPlus = 0;
        if(Second > timeCalSecond) //현재 초 (0)- 시간차
        {
            Second -= timeCalSecond;
        }
        else                       //현재초 - 시간차(0)
        {
            if(Minute > 0)
            {
                //minute 에서 빌려올수있을때
                Minute -= 1;
                Second += 60;
                Second -= timeCalSecond;
            }
            else if(Minute == 0)
            {
                //minute 값을 바꿔야될때
                Minute = 9;
                Second += 60; //싀벌 여기
                Second -= timeCalSecond;
                HeartAmount++;
            }
        }
        

        if(Minute > timeCalMinute)
        {
            Minute -= timeCalMinute;
        }
        else//시간차이가 더클때
        {
            TempMinute = timeCalMinute - Minute;
            HeartPlus = TempMinute / 10;
            Minute = TempMinute % 10;
        }
        //맥스 5까지만 쳐오르게
        for(int i=0;i<HeartPlus;i++)
        {
            if (HeartAmount < 5)
                HeartAmount++;
        }
    }
}