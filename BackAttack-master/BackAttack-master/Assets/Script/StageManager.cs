using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using DG.Tweening;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    public List<GameObject> StageObjectList;
    public Slider PlayerHp;

    public GameObject StartProd;
    public GameObject Clear;
    public GameObject Failed;

    public delegate void GameEvent();
    public event GameEvent GameStartEvent;

    private int EnemyCount = 0;
    private PlayerCtrl mPlayer;

    private BoxCollider2D[] _allBoxes;

    private WaitForSeconds UpdateTime = new WaitForSeconds(1.0f);

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Create Stage 
        Instantiate(StageObjectList[DataManager.Instance.Selected_StageID - 1]);
 
        PlayerHp.minValue = 0;
        PlayerHp.maxValue = 100;
        PlayerHp.value = 100;

        StartProd.SetActive(true);
        Failed.SetActive(false);
        Clear.SetActive(false);

        FadeInOut.Instance.FadeOut(() => DataManager.Instance.IsLock = false);
        SoundManager.Instance.Play_InGame_Bgm();
        DataManager.Instance.IsGameStart = true;

        _allBoxes = _allBoxes = FindObjectsOfType(typeof(BoxCollider2D)) as BoxCollider2D[];
        StartCoroutine(TimeCalc());
    }

    public BoxCollider2D[] GetAllBox() { return _allBoxes; }

    public PlayerCtrl GetPlayer() { return mPlayer; }
    public void SyncPlayer(PlayerCtrl p)
    {
        mPlayer = p;
    }
    public void SyncEnemy()
    {
        EnemyCount++;
    }
    IEnumerator TimeCalc()
    {
        yield return UpdateTime;

        if (DataManager.Instance.HeartAmount < 5)
        {
            if (DataManager.Instance.Second == 0 && DataManager.Instance.Minute == 0)
            {
                DataManager.Instance.Second = 0;
                DataManager.Instance.Minute = 10;
                DataManager.Instance.HeartAmount++;
            }
            if (DataManager.Instance.Second > 0)
            {
                DataManager.Instance.Second -= 1;

            }
            else
            {
                DataManager.Instance.Second = 59;
                DataManager.Instance.Minute -= 1;
            }
         }

        StartCoroutine(TimeCalc());
    }

    public void PlayerHit(float Damage, float duration =.5f, float str = 0.025f,int vib = 1,float randomness = 10 )
    {
        if (PlayerHp.value <= 0) return;
        PlayerHp.value -= Damage;
        mPlayer.PlayerHit();
        Camera.main.DOKill();
        Camera.main.DOShakePosition(duration,str, vib, randomness);

        if (PlayerHp.value <= 0)
        {
            mPlayer.PlayerDie();
            Failed.SetActive(true);
            Clear.SetActive(false);
            Camera.main.DOKill();
            Camera.main.DOShakePosition(0.25f, 3, 30);
            Camera.main.GetComponent<CameraCtrl>().FinishCameraMoveUpdate();
            DataManager.Instance.HeartAmount-=1;
        }
    }
    public void EnemyDie()
    {
        SoundManager.Instance.Play_EnemyDie();
        EnemyCount--;
        if (EnemyCount <= 0)
        {
            Clear.SetActive(true);
            Failed.SetActive(false);
            mPlayer.Finish();
            DataManager.Instance.SetStageData(DataManager.Instance.Selected_StageID, 1);
        }
    }

    public void StartGame()
    {
        GameStartEvent();
    }

    public void ToTitle()
    {
        FadeInOut.Instance.FadeIn(() => SceneManager.LoadScene("Scene_Title"), FadeInOut.eFadeActiveOption.None);
        DataManager.Instance.IsLock = true;
    }
}