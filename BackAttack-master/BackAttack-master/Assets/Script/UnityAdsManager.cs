using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

/*
    =============== Unity Ads Guide ==================

    1. Unity 상단 메뉴바에서  Help 탭을 눌러서 Unity Services 를 킨다.
    2. Ads On 을 on 으로 킨다.
    3. Go To Dashboard 를 누른다. 그럼 웹페이지가 등장한다.
    4. 왼쪽 메뉴에서 프로젝트를 눌르고 뜨는 페이지에서 광고를 적용시킬 프로젝트를 찾는다.
    5. Monetization 를 눌르면 하위에 Placements 라는 탭을 킨다.
    6. 여기에 Android 랑 apple 이랑  광고 타입 ID 가 존재한다.
    7. 변수로 저장해둔다. 
     
*/


public class UnityAdsManager : MonoBehaviour
{
    public static UnityAdsManager instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public enum eAdsType
    {
        None,
        Rewoard,
        Skip,
        Banner
    }

    // ☆☆☆☆☆ Custom Your Ads ID ☆☆☆☆☆ 
    private const string Android_ID = "3373552";
    private const string Apple_ID = "3373553";
    
    private const string VideoSkip_ID        = "video";          // Skip Ok Ads 
    private const string VideoRewoard_ID     = "rewardedVideo";  // Not Skip Ads 
    private const string VideoBanner_ID      = "BackAttack_Banner";// Banner Ads 
    private const string VideoDefault_ID     = "rewardedVideo";  // Default Ads
    private string Video_ID;

    private Action RewardFunc , FailedFunc , SkipFunc;
    // ☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆

 
    void Start()
    {
        Video_ID = VideoDefault_ID;
        RewardFunc = FailedFunc = SkipFunc = () => { };
        Prepare();
    }
     
    public void ShowRewardedAd()
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
         }
     }
   

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                //
                // YOUR CODE TO REWARD THE GAMER
                // Give coins etc.
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }
 
    // 플랫폼별 광고 초기화
    private void Prepare()
    {
//#if UNITY_EDITOR
//        Advertisement.Initialize("1234", true);
//#elif UNITY_ANDROID
        Advertisement.Initialize(Android_ID , false);
//#elif UNITY_IOS
//        Advertisement.Initialize(Apple_ID);
//#endif
    }
     
    // 기본 광고 재생
    public bool ShowAds()
    {
        // Play Movie
        //if (CheckAds() && SupportedAds() &&IsShowAds() == false)
        if (CheckAds() && IsShowAds() == false)    
        {
            ShowOptions options = new ShowOptions();
            options.resultCallback = AdsCallBackEvent;

            Advertisement.Show(Video_ID, options);
            DataManager.Instance.IsAds = true;
            return true;
        }
        // Not Ready Movie  
        else
        {
            DataManager.Instance.OpenPopup(PopUpCtrl.ePopupType.NoAds, true);
            // Send Event   No Ready Ads 
            return false;
        }
    }

    /// <summary>
    /// 광고 재생 보상 설정
    /// </summary>
    /// <param name="Reward"> 리워드를 적용 처리 함수</param>
    /// <param name="Failed"> 제대로 영상이 표시 안됫을때 처리 함수</param>
    /// <param name="Skip"> 영상을 제대로 보지 않은 유저들 처리 함수</param>
    public bool ShowAds(Action Reward , Action Failed , Action Skip)
    {
        RewardFunc = Reward;
        FailedFunc = Failed;
        SkipFunc = Skip;
        return ShowAds();
    }
    // 광고 타입 변경
    public bool ShowAds(eAdsType adsType , Action Reward, Action Failed, Action Skip)
    {
        switch(adsType)
        {
            case eAdsType.Rewoard:
                Video_ID = VideoRewoard_ID;
                break;
            case eAdsType.Skip:
                Video_ID = VideoSkip_ID;
                break;
            case eAdsType.Banner:
                Video_ID = VideoBanner_ID;
                break;
        }
        RewardFunc = Reward;
        FailedFunc = Failed;
        SkipFunc = Skip;
        return ShowAds();
    }


    // 재생 가능한 광고가 존재 하는지 ?
    public bool CheckAds()
    {
        return Advertisement.IsReady(Video_ID);
    }
    // 이 플랫폼에서 광고를 지원 하는지 ?
    public bool SupportedAds()
    {
        return Advertisement.isSupported;
    }
    // 현재 광고를 재생 중인지 ?
    public bool IsShowAds()
    {
        return Advertisement.isShowing;
    }


    //// 광고 시청중 이벤트 발생시 이벤트를 수신하는 함수    
    private void AdsCallBackEvent(ShowResult result)
    {
        switch (result)
        {
            // 광고 시청이 완료 되었을때
            case ShowResult.Finished:
                // 유저에게 보상을 지급
                RewardFunc();
                DataManager.Instance.IsAds = false;
                break;
            // 광고 시청에 실패 했을때
            case ShowResult.Failed:
                // 광고 재생에 실패 했다. 보상 지급 x
                FailedFunc();
                break;
            // 광고 시청이 스킵 되었을때나 유저가 이상 행동을 하였을시
            default:
                // 유저가 광고를 제대로 보지않음 보상 지급 x
               SkipFunc();
                break;
        }

    }

 // ======= Test ==========
    public void TestAds()
    {
        ShowAds();
    }
    // ===================
}
