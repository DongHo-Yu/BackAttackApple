using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class EnemyCtrl : MonoBehaviour {

    public GameObject Laser;
    private DynamicLight2D.DynamicLight LaserLight;

    [SerializeField] private Sprite[] BodySprite;
    [SerializeField] private SpriteRenderer Body;
    [SerializeField] private GameObject ShotEffect;
    [SerializeField] private GameObject BloodEffect;

    public GameObject RotateObject;
    public GameObject bullet;

    public Transform FirePos;
    public Transform Head;

    public enum eDir
    {
        None,
        Left,
        Right,
        LeftStop,
        RightStop
    }
    [Space(16)]
    public eDir DirState;

    [Space(16)]
    public Vector3 RightMaxPos;
    public Vector3 LeftMaxPos;

    private float AngleSafe;
    float angle;
    [Space(16)]
    public float Speed;

    private float DetectRange = .5f;

    private float RotateSpeed = 100;
    private bool isRot;
    private bool isLock = true;
    private bool isDie = false;

    public float Damage = 10;
    public bool IsStartDown;

    private WaitForSeconds WalkTime = new WaitForSeconds(0.15f);


    private void Awake()
    {
        StageManager.Instance.GameStartEvent += UnLock;
         
        Rigidbody2D[] obj = this.GetComponentsInChildren<Rigidbody2D>();

        for (int i = 0; i < obj.Length; i++)
        {
            obj[i].GetComponent<BoxCollider2D>().isTrigger = true;
         }
         
        this.GetComponent<BoxCollider2D>().isTrigger = false;
        if(Laser != null && Laser.GetComponent<DynamicLight2D.DynamicLight>() != null)
        {
            LaserLight = Laser.GetComponent<DynamicLight2D.DynamicLight>();
            LaserLight.InsideFieldOfViewEvent += Attack;
            LaserLight.OnExitFieldOfView += AttackFinish;

            Material newMaterial = new Material(Laser.GetComponent<DynamicLight2D.DynamicLight>().LightMaterial);
            Laser.GetComponent<DynamicLight2D.DynamicLight>().LightMaterial = newMaterial;
            Laser.GetComponent<DynamicLight2D.DynamicLight>().LightMaterial.color = mColOriginal;
        }
    }

    private void Start()
    {
        StageManager.Instance.SyncEnemy();
        this.transform.parent = null; 
    }

    private void OnDestroy()
    {
        if(StageManager.Instance != null) StageManager.Instance.GameStartEvent -= UnLock;
    }

    private IEnumerator WalkAnimation()
    {
        int Count = 0;
        while(true)
        {
            Body.sprite = BodySprite[Count++];
            if (Count >= BodySprite.Length) Count = 0;
            yield return WalkTime;
            if(isDie == true)
            {
                Body.sprite = BodySprite[0];
                break;
            }
        }
    }


    private void UnLock()
    {
        isLock = false;
        isDie = false;
       if (DirState != eDir.None)  StartCoroutine("WalkAnimation");

    }
    void OnEnable()
    {
        isRot = false;
        angle = RotateObject.transform.eulerAngles.z;
        AngleSafe = angle;
        RightMaxPos.y = transform.position.y;
        LeftMaxPos.y = transform.position.y;



        if(DirState == eDir.Left)
        {
            transform.localScale = new Vector3(-2, 2, 1);
        }
        else if(DirState == eDir.Right)
        {
            transform.localScale = new Vector3(2, 2, 1);
        }
        else if(DirState == eDir.LeftStop || DirState == eDir.RightStop)
        {
            RightMaxPos = transform.position;
            LeftMaxPos = transform.position;
        }


    }

    void Update()
    {
        if (isLock) return;

        switch (DirState)
        {
            case eDir.Left:
                Move(LeftMaxPos, Vector3.left , eDir.Right);
                break;            
            case eDir.Right:
                Move(RightMaxPos, Vector3.right, eDir.Left,2);
                break;
            case eDir.LeftStop:
                Move(LeftMaxPos, Vector3.left , eDir.LeftStop);
                break;
            case eDir.RightStop:
                Move(RightMaxPos, Vector3.right, eDir.RightStop);            
                break; ;
        }
        //if (mIsAttacking == false)
        //    ResetDir(DirSafe);
        //else
        //    SetPlayerLook();
    }

    void Move(Vector3 DirectionPos,Vector3 dir,eDir ChangeDir = eDir.Right,float EnemyDirScaleValue = -2)
    {
        float dis = Vector2.Distance(transform.position, DirectionPos);
        if (dis >= DetectRange)
        {
            transform.Translate(dir * Speed * Time.deltaTime);
        }
        else //TurnPoint에 도착했다면
        {
            Rot(ChangeDir);
        }
    }
    void Rot(eDir ChangeDir)
    {
        if (isRot) return;
        isRot = true;
        StartCoroutine(co_Rot(ChangeDir));
        StopCoroutine("WalkAnimation");
        Body.sprite = BodySprite[0];
    }
    IEnumerator co_Rot(eDir ChangeDir)
    {
        switch(ChangeDir)
        {
            //오른쪽 
            case eDir.Left: 
                angle = 0;
                AngleSafe = angle;

                while (true)
                {
                    yield return null; 
                    angle += .5f;
                    AngleSafe = angle;

                    RotateObject.transform.eulerAngles = new Vector3(0, 0, angle);
                    if (angle > 65) break;
                    //}
                }
                StartCoroutine(co_AfterRotProcess(ChangeDir, angle));
                break;
            //왼쪽
            case eDir.Right: 
                angle = 0;
                AngleSafe = angle;

                while (true)
                {
                    yield return null; 
                    angle -= .5f;
                    AngleSafe = angle;

                    RotateObject.transform.eulerAngles = new Vector3(0, 0, angle);
                    if (angle < -65) break;
                    //}
                }
                StartCoroutine(co_AfterRotProcess(ChangeDir, angle));
                break;
            case eDir.LeftStop:
                angle = 0;
                AngleSafe = angle;
              
                while (true)
                {
                    yield return null;
                    if (IsStartDown)
                    {
                        angle += .5f;
                        AngleSafe = angle;

                        RotateObject.transform.eulerAngles = new Vector3(0, 0, angle);
                        if (angle > 65)
                        {
                            IsStartDown = false;
                         }
                    }
                    else
                    {
                        angle -= .5f;
                        AngleSafe = angle;

                        RotateObject.transform.eulerAngles = new Vector3(0, 0, angle);
                        if (angle < -65)
                        {
                            IsStartDown = true;
                        }
                    }

                }
                 isRot = false;
                break;
            case eDir.RightStop:
                angle = 0;
                AngleSafe = angle;

                while (true)
                {
                    yield return null; 
                    if(IsStartDown)
                    {
                        angle -= .5f;
                        AngleSafe = angle;

                        RotateObject.transform.eulerAngles = new Vector3(0, 0, angle);
                        if (angle < -65)
                        {
                            IsStartDown = false;
                        }
                    }
                    else
                    {
                        angle += .5f;
                        AngleSafe = angle;

                        RotateObject.transform.eulerAngles = new Vector3(0, 0, angle);
                        if (angle > 65)
                        {
                            IsStartDown = true;
                        }
                    }
                }
                isRot = false;
                break;
        }
    }
    IEnumerator co_AfterRotProcess(eDir ChangeDir,float angle)
    {
        if (ChangeDir == eDir.Left) //오른쪽
        { 
            angle = -90;
            AngleSafe = angle;

            transform.localScale = new Vector3(-2, 2, 1);
            while (true)
            {
                yield return null;
                //if (mIsAttacking == false)
                //{
                    angle += .5f;
                    AngleSafe = angle;

                    RotateObject.transform.eulerAngles = new Vector3(0, 0, angle);
                    if (angle == 0) break;
                //}
            } 
            DirState = ChangeDir;
            isRot = false;
        }
        else if (ChangeDir == eDir.Right) //왼쪽
        { 
            angle = 90;
            AngleSafe = angle;

            transform.localScale = new Vector3(2, 2, 1);
            while (true)
            {
                yield return null;
                //if (mIsAttacking == false)
                //{
                    angle -= .5f;
                    AngleSafe = angle;

                    RotateObject.transform.eulerAngles = new Vector3(0, 0, angle);
                    if (angle == 0) break;
                //}
            } 
            DirState = ChangeDir;
            isRot = false;
        }
        StartCoroutine("WalkAnimation");
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(RightMaxPos, 1);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(LeftMaxPos, 1);
    }
#endif


    public float CalculateAngle(Vector3 from, Vector3 to)
    {
        return Quaternion.FromToRotation(Vector3.up, to - from).eulerAngles.z;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDie) return;

        //if (collision.transform.tag.Equals("Player"))
        //{
            StopCoroutine("WalkAnimation");
            Body.sprite = BodySprite[0];

            isDie = true;
           if(Laser != null) Laser.SetActive(false); 
            this.GetComponent<Rigidbody2D>().simulated = false;
            this.GetComponent<BoxCollider2D>().enabled = false;

            Rigidbody2D[] obj = this.GetComponentsInChildren<Rigidbody2D>();

            for (int i = 0; i < obj.Length; i++)
            {
                obj[i].gameObject.transform.parent = null;
                obj[i].bodyType = RigidbodyType2D.Dynamic;
                obj[i].GetComponent<BoxCollider2D>().isTrigger = false;
                obj[i].AddForce(new Vector2(Random.Range(-5, 5), Random.Range(-5, 5)) * 300, ForceMode2D.Force);

                obj[i].transform.tag = "ghost";
                obj[i].gameObject.layer = LayerMask.NameToLayer("Dead");
            }
            Camera.main.DOKill();
            Camera.main.DOShakePosition(0.25f, 3, 30);
            StageManager.Instance.EnemyDie();

        //}
    }

 


    private void ResetDir(eDir dir)
    {
        switch (dir)
        {
            case eDir.Left:
                transform.localScale = new Vector3(2, 2, 1);
                break;
            case eDir.Right:
                transform.localScale = new Vector3(-2, 2, 1);
                break;
        }
        RotateObject.transform.eulerAngles = new Vector3(0, 0, AngleSafe);         
    }

    private bool mIsAttacking;
    private bool mIsInside;
    private void AttackFinish(GameObject go, DynamicLight2D.DynamicLight Light)
    {
        if (go.transform.CompareTag("Player"))
        {
            mIsInside = false;
            mIsAttacking = false;
        }
    }

    private void Attack(GameObject[] g, DynamicLight2D.DynamicLight light)
    {
        if (isLock) return;
        foreach (GameObject gs in g)
        {
            if (gs.transform.CompareTag("Player"))
            {
                mIsInside = true;
                if (mAttack == false)
                {
                    StartCoroutine(AttackProd());
                    mIsAttacking = true;
                }
            }

        }
    }

    private bool mAttack = false;
    private WaitForSeconds AttackTime = new WaitForSeconds(0.05f);

    private Color32 mColOriginal = new Color32(0, 0, 0, 16);
    private Color32 mColAttack = new Color32(255, 255, 0, 16);

    private IEnumerator AttackProd()
    {
        mAttack = true;
        yield return null;
        AttackStart();
        Laser.GetComponent<DynamicLight2D.DynamicLight>().LightMaterial.color = mColAttack;
        yield return AttackTime;
        Laser.GetComponent<DynamicLight2D.DynamicLight>().LightMaterial.color = mColOriginal;
        yield return AttackTime;
        mAttack = false;
        if (mIsInside == false && mIsAttacking == true) mIsAttacking = false;
    }
    private void AttackStart()
    {
        SoundManager.Instance.Play_Gun();

        //SetPlayerLook();
        GameObject Shot = Instantiate(ShotEffect) as GameObject;
        Shot.transform.position = FirePos.position;
        Shot.transform.parent = FirePos;
        Shot.transform.localRotation = Quaternion.Euler(0, 0, -180);
        Shot.transform.localPosition = new Vector3(0.65f, 0.15f);

        GameObject Bullet = Instantiate(bullet) as GameObject;
        Bullet.transform.position = FirePos.position;
        Bullet.transform.localRotation = Quaternion.identity;
        Bullet.transform.DOMove(StageManager.Instance.GetPlayer().transform.position, 0.05f).OnComplete(() => Destroy(Bullet));

        if (DataManager.Instance.isBlood)
        {
            GameObject Blood = Instantiate(BloodEffect) as GameObject;
            Blood.transform.position = StageManager.Instance.GetPlayer().transform.position;
            Blood.transform.eulerAngles = new Vector3(0, 0, -angle);
        }
        StageManager.Instance.PlayerHit(Damage);
    }

    //private void SetPlayerLook()
    //{
    //    Vector3 PlayerPos = StageManager.Instance.GetPlayer().transform.position;
    //    Vector3 dir = PlayerPos - Laser.transform.position;

    //    if (Laser.transform.position.x < PlayerPos.x)
    //        this.transform.localScale = new Vector3(2, 2, 2);
    //    else
    //        this.transform.localScale = new Vector3(-2, 2, 2);

    //    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    //    RotateObject.transform.localEulerAngles = new Vector3(0, 0, angle);        
    //}

}
