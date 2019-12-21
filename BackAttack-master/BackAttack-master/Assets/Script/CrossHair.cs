using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{

    private GameObject Player;

    private bool isFire;
    private bool isFollow;
    private float DetectRange = 2.5f;
    private int MovePoint = 0;
    private float FireDelay = .3f;

    public Vector3[] WayPoint;

    [Header("Cross Hair Parts")]
    [SerializeField] private GameObject Aim;
    [SerializeField] private GameObject Cicle;
    [SerializeField] private GameObject Shadow;
    public GameObject BloodEffect;

    private Animation ani;
    void Start()
    {
        Player = GameObject.Find("Player");
        ani = GetComponent<Animation>();
        InvokeRepeating("AttackPlayer", FireDelay, FireDelay);
    }

    
    public void FollowCtrl()
    {
        isFollow = !isFollow;
    }
    void OnEnable()
    {
        isFire = false;
        isFollow = false;
    }
    //화면에 금가는거
    //노랑배경
    //사운드
    //안쫓아올때 isFollow Flase일때 효과
    //쫓아올때(공격) isFollow true 일때효과 
    void Update()
    {
        FindNextPoint();
        DetectPlayer();
        transform.Translate((WayPoint[MovePoint] - transform.position).normalized * 5 * Time.deltaTime);
    }
    void FindNextPoint()
    {
        if (Vector3.Distance(transform.position, WayPoint[MovePoint]) <= .5f)
        {
            MovePoint = MovePoint >= WayPoint.Length - 1 ? 0 : MovePoint + 1;
        }
    }
    void DetectPlayer()
    {
        if (Vector3.Distance(transform.position, Player.transform.position) <= 4.5f)
        {
            if(isFollow)
            {
                transform.position = Vector3.Lerp(transform.position, Player.transform.position, Time.deltaTime * 20f);
                isFire = true;
            }
            StartCoroutine(AA());
        }
        else
            isFire = false;
    }
    IEnumerator AA()
    {
        isFollow = true;
        yield return new WaitForSeconds(1.5f);
        isFollow = false;
    }
    void AttackPlayer()
    {
        if (isFire)
        {
            if (!DataManager.Instance.isBlood)
            {
                GameObject Blood = Instantiate(BloodEffect) as GameObject;
                Blood.transform.position = StageManager.Instance.GetPlayer().transform.position;
                Blood.transform.eulerAngles = new Vector3(0, 0, 0);
                Blood.transform.localScale = new Vector3(8f, 8f, 1);
            }
            StageManager.Instance.PlayerHit(1, 0.025f, 10, 50);
            //안쫓아오게
            //ani.Play();
        }
    }
    void OnDrawGizmosSelected()
    {
        for (int i = 0; i < WayPoint.Length; i++)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(WayPoint[i], 1);
        }
    }
}