using System;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class FadeInOut : MonoBehaviour
{
    public static FadeInOut Instance;
    public enum eFadeActiveOption { None, True, False }

    private Image FadeImg;
    private bool IsFade;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = (FadeInOut)this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        FadeImg = this.GetComponent<Image>();
        IsFade = false;
    }

    public void FadeKill()
    {
        FadeImg.DOKill();
        IsFade = false;
    }
    public void FadeOn()
    {
        FadeKill();
        FadeImg.color = new Color(0, 0, 0, 1);
        FadeImg.enabled = true;
    }
    public void FadeOff()
    {
        FadeKill();
        FadeImg.color = new Color(0, 0, 0, 0);
        FadeImg.enabled = false;
    }

    public void FadeKillIn(float Duration = 1.0f)
    {
        FadeImg.DOKill();
        IsFade = false;
        FadeInFunc(null, Duration, eFadeActiveOption.False);
    }

    public void FadeKillOut(float Duration = 1.0f)
    {
        FadeImg.DOKill();
        IsFade = false;
        FadeOutFunc(null, Duration, eFadeActiveOption.False);
    }

    public void FadeIn(eFadeActiveOption FadeOption = eFadeActiveOption.False)
    {
        FadeInFunc(null, 1.0f, FadeOption);
    }
    public void FadeOut(eFadeActiveOption FadeOption = eFadeActiveOption.False)
    {
        FadeOutFunc(null, 1.0f, FadeOption);
    }
    public void FadeIn(float Duration, eFadeActiveOption FadeOption = eFadeActiveOption.False)
    {
        FadeInFunc(null, Duration, FadeOption);
    }
    public void FadeOut(float Duration, eFadeActiveOption FadeOption = eFadeActiveOption.False)
    {
        FadeOutFunc(null, Duration, FadeOption);
    }
    public void FadeIn(Action Func, eFadeActiveOption FadeOption = eFadeActiveOption.False)
    {
        FadeInFunc(Func, 1.0f, FadeOption);
    }
    public void FadeOut(Action Func, eFadeActiveOption FadeOption = eFadeActiveOption.False)
    {
        FadeOutFunc(Func, 1.0f, FadeOption);
    }
    public void FadeIn(Action Func, float Duration, eFadeActiveOption FadeOption = eFadeActiveOption.False)
    {
        FadeInFunc(Func, Duration, FadeOption);
    }
    public void FadeOut(Action Func, float Duration, eFadeActiveOption FadeOption = eFadeActiveOption.False)
    {
        FadeOutFunc(Func, Duration, FadeOption);
    }



    private void FadeInFunc(Action Func = null, float Duration = 1.0f, eFadeActiveOption FadeOption = eFadeActiveOption.False)
    {
        if (!IsFade)
        {
            FadeImg.color = new Color32(0, 0, 0, 0);
            FadeImg.enabled = true;
            IsFade = true;

            FadeImg.DOFade(1.0f, Duration).OnComplete(() =>
            {
                IsFade = false;
                if (Func != null) Func();
                OptionFunc(FadeOption);
            });
        }
    }

    private void FadeOutFunc(Action Func = null, float Duration = 1.0f, eFadeActiveOption FadeOption = eFadeActiveOption.False)
    {
        if (!IsFade)
        {
            FadeImg.color = new Color32(0, 0, 0, 255);
            FadeImg.enabled = true;
            IsFade = true;

            FadeImg.DOFade(0.0f, Duration).OnComplete(() =>
            {
                IsFade = false;
                if (Func != null) Func();
                OptionFunc(FadeOption);
            });
        }
    }


    private void OptionFunc(eFadeActiveOption Option)
    {
        switch (Option)
        {
            case eFadeActiveOption.None:
                break;
            case eFadeActiveOption.True:
                FadeImg.enabled = true;
                break;
            case eFadeActiveOption.False:
                FadeImg.enabled = false;
                break;

        }
    }

}
