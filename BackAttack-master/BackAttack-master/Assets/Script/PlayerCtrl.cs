using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;  

using DG.Tweening;

public class PlayerCtrl : MonoBehaviour
{
    public enum eDir
    {
        None,
        Left,
        Right,
        Up,
        Down
    }

    [SerializeField] private GameObject MouseDrag;
    [SerializeField] private GameObject DieEffect;

    private Transform mMyTrans;
    private Rigidbody2D mMyRigid;

    private Vector3 mDir;
    private eDir mDirState;

    private const float mMoveSpeed = 50.0f;
    private Vector3 mMouseStartPos;
    private Vector3 mMouseEndPos;
    private bool mIsLock = true;
    private bool mIsFinish = false;

    private void Awake()
    {
        mMyTrans = this.transform;
        mMyRigid = this.GetComponent<Rigidbody2D>();

        MouseDrag = Instantiate(MouseDrag) as GameObject;

        StageManager.Instance.SyncPlayer(this);
        StageManager.Instance.GameStartEvent += UnLock;
    }

    private void OnEnable()
    { 
        mDir = Vector3.zero;
        mDirState = eDir.None;

        Camera.main.GetComponent<CameraCtrl>().StartCameraMoveUpdate(mMyTrans);
    }

    private void OnDestroy()
    {
        if (StageManager.Instance != null) StageManager.Instance.GameStartEvent -= UnLock;
    }

    private void Start()
    {
        this.transform.parent = null;
    }

    private void UnLock()
    {
        mIsLock = false;
    }
    public void Finish()
    {
        mIsFinish = true;
    }

    private void Update()
    {
        if (mIsLock) return;

        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            MouseDrag.SetActive(true);
            mMouseStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            MouseDrag.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            MouseDrag.transform.position = new Vector3(MouseDrag.transform.position.x, MouseDrag.transform.position.y, 0);
        }
        if (Input.GetMouseButton(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            MouseDrag.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            MouseDrag.transform.position = new Vector3(MouseDrag.transform.position.x, MouseDrag.transform.position.y, 0);
        }
        if (Input.GetMouseButtonUp(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            MouseDrag.SetActive(false);
            mMouseEndPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float Angle = CalculateAngle(mMouseEndPos, mMouseStartPos);

            // Left
            if ((Angle >= 270 && Angle <= 315) || (Angle >= 225 && Angle <= 270)) Move(1);
            // Right
            if ((Angle >= 45 && Angle <= 90) || (Angle >= 90 && Angle <= 135)) Move(2);
            // Up
            if ((Angle >= 135 && Angle <= 180) || (Angle >= 180 && Angle <= 225)) Move(3);
            // Down
            if ((Angle >= 0 && Angle <= 45) || (Angle >= 315 && Angle <= 360)) Move(4);
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.W)) Move(3);
        if (Input.GetKeyDown(KeyCode.A)) Move(1);
        if (Input.GetKeyDown(KeyCode.S)) Move(4);
        if (Input.GetKeyDown(KeyCode.D)) Move(2);
#endif
    }

    public float CalculateAngle(Vector3 from, Vector3 to)
    {
        return Quaternion.FromToRotation(Vector3.up, to - from).eulerAngles.z;
    }

    private void FixedUpdate()
    {
        if (mIsLock) return;
        mMyRigid.MovePosition(mMyTrans.position + mDir * mMoveSpeed * Time.deltaTime);
    }

    public void Move(int Dir)
    {
        if (mDirState != eDir.None) return;

        switch (Dir)
        {
            // Left
            case 1:
                mDir = Vector3.left;
                mDirState = eDir.Left;

                mMyTrans.localScale = new Vector3(-1,mMyTrans.localScale.y, 1);
                 break;
            // Right
            case 2:
                mDir = Vector3.right;
                mDirState = eDir.Right;

                mMyTrans.localScale = new Vector3(1, mMyTrans.localScale.y, 1);
                break;
            // Top
            case 3:
                mDir = Vector3.up;
                mDirState = eDir.Up;

                mMyTrans.localScale = new Vector3(mMyTrans.localScale.x, 1, 1);
                mMyTrans.rotation = Quaternion.Euler(0, 0, 90 * mMyTrans.localScale.x);
                break;
            // Bottom
            case 4:
                mDir = Vector3.down;
                mDirState = eDir.Down;

                mMyTrans.localScale = new Vector3(mMyTrans.localScale.x, 1, 1);
                mMyTrans.rotation = Quaternion.Euler(0, 0, 90 * -mMyTrans.localScale.x);
                break;
        }
        SoundManager.Instance.Play_PlayerMove();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (mIsFinish)
        {
            if (collision.transform.tag.Equals("ghost") == false)
            {
                mIsLock = true;
            }
        }

        switch (collision.transform.tag)
        {
            case "Wall_Left":
                MoveReset();
                 if (collision.transform.position.y > mMyTrans.position.y)
                {
                    mMyTrans.localScale = new Vector3(-1, 1, 1);
                    mMyTrans.rotation = Quaternion.Euler(0, 0, -90);
                }
                 else
                {
                    mMyTrans.localScale = new Vector3(1, 1, 1);
                    mMyTrans.rotation = Quaternion.Euler(0, 0, -90);
                }
                break;
            case "Wall_Right":
                MoveReset();
                 if (collision.transform.position.y > mMyTrans.position.y)
                {
                    mMyTrans.localScale = new Vector3(1, 1, 1);
                    mMyTrans.rotation = Quaternion.Euler(0, 0, 90);
                }
                 else
                {
                    mMyTrans.localScale = new Vector3(-1, 1, 1); ;
                    mMyTrans.rotation = Quaternion.Euler(0, 0, 90);
                }
                break;
            case "Wall_Top":
                MoveReset();
                 if (collision.transform.position.x > mMyTrans.position.x)
                {
                    mMyTrans.localScale = new Vector3(1, -1, 1);
                    mMyTrans.rotation = Quaternion.identity;
                }
                 else
                {
                    mMyTrans.localScale = new Vector3(-1, -1, 1);
                    mMyTrans.rotation = Quaternion.identity;
                }
                break;
            case "Wall_Bottom":
                MoveReset();

                if (collision.transform.position.x > mMyTrans.position.x)
                {
                    mMyTrans.localScale = new Vector3(1, 1, 1); 
                    mMyTrans.rotation = Quaternion.identity;
                }
                else
                {
                    mMyTrans.localScale = new Vector3(-1, 1, 1); 
                    mMyTrans.rotation = Quaternion.identity;
                }
                break;
            case "Floor_Horizon":
                 if (collision.transform.position.y > mMyTrans.position.y)
                {
                    if (collision.transform.position.x > mMyTrans.position.x)
                    {
                        mMyTrans.localScale = new Vector3(1, -1, 1);
                        mMyTrans.rotation = Quaternion.identity;
                    }
                     else
                    {
                        mMyTrans.localScale = new Vector3(-1, -1, 1);
                        mMyTrans.rotation = Quaternion.identity;
                    }
                }
                 else
                {
                    if (collision.transform.position.x > mMyTrans.position.x)
                    {
                        mMyTrans.localScale = new Vector3(1, 1, 1);
                        mMyTrans.rotation = Quaternion.identity;                       
                    }
                     else
                    {
                        mMyTrans.localScale = new Vector3(-1, 1, 1);
                        mMyTrans.rotation = Quaternion.identity;                       
                    }
                }
                break;
            case "Floor_Vertical":
                 if (collision.transform.position.x > mMyTrans.position.x)
                {
                     if (collision.transform.position.y > mMyTrans.position.y)
                    {
                        mMyTrans.localScale = new Vector3(1,1,1);
                        mMyTrans.rotation = Quaternion.Euler(0, 0, 90);
                    }
                     else
                    {
                        mMyTrans.localScale = new Vector3(-1, 1, 1);
                        mMyTrans.rotation = Quaternion.Euler(0, 0, 90); 
                    }
                }
                 else
                {
                     if (collision.transform.position.y > mMyTrans.position.y)
                    {
                        mMyTrans.localScale = new Vector3(-1, 1, 1);
                        mMyTrans.rotation = Quaternion.Euler(0, 0, -90);
                    }
                     else
                    {
                        mMyTrans.localScale = new Vector3(1, 1, 1);
                        mMyTrans.rotation = Quaternion.Euler(0, 0, -90);
                    }
                }
                break;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        switch (collision.transform.tag)
        {
            case "Wall_Left":
                if (mDirState == eDir.Left) MoveReset();
                break;
            case "Wall_Right":
                if (mDirState == eDir.Right) MoveReset();
                break;
            case "Wall_Top":
                if (mDirState == eDir.Up) MoveReset();
                break;
            case "Wall_Bottom":
                if (mDirState == eDir.Down) MoveReset();
                break;
            case "Floor_Horizon":
                if (collision.transform.position.y > mMyTrans.position.y)
                {
                    if (mDirState == eDir.Up) MoveReset();

                    if (mDirState == eDir.None)
                    {
                        if (collision.transform.position.x > mMyTrans.position.x)
                        {
                            mMyTrans.localScale = new Vector3(1, -1, 1);
                            mMyTrans.rotation = Quaternion.identity;
                        }
                        else
                        {
                            mMyTrans.localScale = new Vector3(-1, -1, 1);
                            mMyTrans.rotation = Quaternion.identity;
                        }
                    }
                }
                else
                {
                    if (mDirState == eDir.Down) MoveReset();

                    if (mDirState == eDir.None)
                    {
                        if (collision.transform.position.x > mMyTrans.position.x)
                        {
                            mMyTrans.localScale = new Vector3(1, 1, 1);
                            mMyTrans.rotation = Quaternion.identity;
                        }
                        else
                        {
                            mMyTrans.localScale = new Vector3(-1, 1, 1);
                            mMyTrans.rotation = Quaternion.identity;
                        }
                    }
                }
                break;
            case "Floor_Vertical":
                if (collision.transform.position.x > mMyTrans.position.x)
                {
                    if (mDirState == eDir.Right) MoveReset();

                    if (mDirState == eDir.None)
                    {
                        if (collision.transform.position.y > mMyTrans.position.y)
                        {
                            mMyTrans.localScale = new Vector3(1, 1, 1);
                            mMyTrans.rotation = Quaternion.Euler(0, 0, 90);
                        }
                        else
                        {
                            mMyTrans.localScale = new Vector3(-1, 1, 1);
                            mMyTrans.rotation = Quaternion.Euler(0, 0, 90);
                        }
                    }
                }
                else
                {
                    if (mDirState == eDir.Left) MoveReset();

                    if (mDirState == eDir.None)
                    {
                        if (collision.transform.position.y > mMyTrans.position.y)
                        {
                            mMyTrans.localScale = new Vector3(-1, 1, 1);
                            mMyTrans.rotation = Quaternion.Euler(0, 0, -90);
                        }
                        else
                        {
                            mMyTrans.localScale = new Vector3(1, 1, 1);
                            mMyTrans.rotation = Quaternion.Euler(0, 0, -90);
                        }
                    }
                }
                break;
        }
    }


    private void MoveReset()
    {
        mDir = Vector3.zero;
        mDirState = eDir.None; 
    }

    public void PlayerDie()
    {
        this.GetComponent<BoxCollider2D>().enabled = false;
        this.GetComponentInChildren<SpriteRenderer>().DOFade(0.0f, 0.25f);
        DieEffect.SetActive(true);
        SoundManager.Instance.Play_PlayerDie();
        mIsLock = true;
        this.gameObject.layer = 0;
    }

    public void PlayerHit()
    {
        if (mHit) return;
        StartCoroutine("HitProd");
    }

    private bool mHit = false;
    private WaitForSeconds HitTime = new WaitForSeconds(0.05f);
    private IEnumerator HitProd()
    {
        if(DataManager.Instance.isBlood)
        {
            mHit = true;
            this.GetComponentInChildren<SpriteRenderer>().color = Color.red;
            yield return HitTime;
            this.GetComponentInChildren<SpriteRenderer>().color = Color.white;
            yield return HitTime;
            mHit = false;
        }
        else
        {
            mHit = true;
            this.GetComponentInChildren<SpriteRenderer>().color = Color.green;
            yield return HitTime;
            this.GetComponentInChildren<SpriteRenderer>().color = Color.white;
            yield return HitTime;
            mHit = false;

        }
    }
}
