using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class CameraCtrl : MonoBehaviour
{
    private Vector3 mVelocity = Vector3.zero;
    private float mDampDuration = 0.025f;

    private Transform mMyTras;
    private Transform mTarget;

    private void Awake()
    {
        mMyTras = this.transform;
    }

    public void StartCameraMoveUpdate(Transform target)
    {
        mTarget = target;
        //StartCoroutine(CameraUpdate(target));
    }
    public void FinishCameraMoveUpdate()
    {
        mTarget = null;
        //StopAllCoroutines();
    }

    private void Update()
    {
        if (mTarget == null) return;
        Vector3 point = Camera.main.WorldToViewportPoint(mTarget.position);
        Vector3 delta = mTarget.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
        Vector3 destination = mMyTras.position + delta;

        mMyTras.position = Vector3.SmoothDamp(mMyTras.position, destination, ref mVelocity, mDampDuration);
    }

    private IEnumerator CameraUpdate(Transform target)
    {
        WaitForFixedUpdate fixedUpdate = new WaitForFixedUpdate();

        while (true)
        {
            if (target == null) break;
            moveCam(target.position);

            yield return fixedUpdate;
        }
    }

    public void moveCam(Vector3 target )
    {
        Vector3 point = Camera.main.WorldToViewportPoint(target);
        Vector3 delta = target - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
        Vector3 destination = mMyTras.position + delta;

        mMyTras.position = Vector3.SmoothDamp(mMyTras.position, destination, ref mVelocity, mDampDuration);
    }

}
  