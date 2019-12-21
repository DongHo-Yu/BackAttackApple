using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class StartProd : MonoBehaviour
{
    private void Start()
    {
        Text text = this.GetComponentInChildren<Text>();
        ProdText(text);
    }

    private void ProdText(Text t)
    {
        if (this.gameObject != null)
        {
            t.DOFade(0.0f, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                t.DOFade(1.0f, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    if (this.gameObject != null)
                    {
                        ProdText(t);
                    }
                });
            });
        }
    }

    public void Prod_End()
    {
        StageManager.Instance.StartGame();
        Destroy(this.gameObject);
    }
}
