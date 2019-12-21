using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeControler : MonoBehaviour
{
    public delegate void FadeHandler();
    public event FadeHandler FadeEvent;
    public float FadeTime;
    public bool isFade;
    Image FadeImage;

    void Awake()
    {
        isFade = false;
        FadeImage = GetComponent<Image>();
        StartCoroutine(TimerFadeIn(3.5f));
        StartCoroutine(TimerFadeOut(5.5f));
    }
    public IEnumerator TimerFadeIn(float time)
    {
        yield return new WaitForSeconds(time);
        FadeIn(FadeImage, FadeTime);
    }
    public IEnumerator TimerFadeOut(float time)
    {
        yield return new WaitForSeconds(time);
        FadeOut(FadeImage, FadeTime, FadeEvent);

    }
    //밝아짐
    public void FadeIn(Image img,float time)
    {
        if (isFade) return;
        StartCoroutine(Co_FadeIn(img, time));
    }
    public void FadeIn(Image img, float time,FadeHandler handle)
    {
        if (isFade) return;
        StartCoroutine(Co_FadeIn(img, time,handle));
    }
    //어두워짐
    public void FadeOut(Image img,float time)
    {
        if (isFade) return;
        StartCoroutine(Co_FadeOut(img, time));
    }

    public void FadeOut(Image img, float time,Color color)
    {
        if (isFade) return;
        StartCoroutine(Co_FadeOut(img, time,color));
    }
    public void FadeOut(Image img, float time,FadeHandler handle)
    {
        if (isFade) return;
        StartCoroutine(Co_FadeOut(img, time, handle));
    }
    public void FadeOut(Image img, float time,string SceneName)
    {
        if (isFade) return;
        StartCoroutine(Co_FadeOut(img, time, SceneName));
    }

    IEnumerator Co_FadeIn(Image img,float time)
    {
        isFade = true;
        img.color = new Color(0, 0, 0, 1);
        Color col = new Color(0, 0, 0, 1);
        float tempTime = 0;
        while (img.color.a > 0)
        {
            tempTime += Time.deltaTime / time;
            col.a = Mathf.Lerp(1, 0, tempTime);
            img.color = col;
            yield return null;
        }
        isFade = false;
    }
    IEnumerator Co_FadeIn(Image img, float time,FadeHandler handle)
    {
        isFade = true;
        img.color = new Color(0, 0, 0, 1);
        Color col = new Color(0, 0, 0, 1);
        float tempTime = 0;
        while (img.color.a > 0)
        {
            tempTime += Time.deltaTime / time;
            col.a = Mathf.Lerp(1, 0, tempTime);
            img.color = col;
            yield return null;
        }
        handle();
        isFade = false;
    }

    IEnumerator Co_FadeOut(Image img,float time)
    {
        isFade = true;
        Color col = new Color(0, 0, 0, 0);
        float tempTime = 0;
        while (img.color.a < 1)
        {
            tempTime += Time.deltaTime / time;
            col.a = Mathf.Lerp(0, 1, tempTime);
            img.color = col;
            yield return null;
        }
        isFade = false;        
    }
    IEnumerator Co_FadeOut(Image img, float time,FadeHandler handle)
    {
        isFade = true;
        Color col = new Color(0, 0, 0, 0);
        float tempTime = 0;

        while (img.color.a < 1)
        {
            tempTime += Time.deltaTime / time;
            col.a = Mathf.Lerp(0, 1, tempTime);
            img.color = col;
            yield return null;
        }
        handle();
        isFade = false;
    }

    IEnumerator Co_FadeOut(Image img, float time,Color color)
    {
        isFade = true;
        
        Color col =color;
        float tempTime = 0;

        while (img.color.a < 1)
        {
            tempTime += Time.deltaTime / time;
            col.a = Mathf.Lerp(0, 1, tempTime);
            img.color = col;
            yield return null;
        }
        isFade = false;
        
    }

    IEnumerator Co_FadeOut(Image img, float time, string SceneName)
    {
        isFade = true;

        Color col = new Color(0, 0, 0, 0);
        float tempTime = 0;

        while (img.color.a < 1)
        {
            tempTime += Time.deltaTime / time;
            col.a = Mathf.Lerp(0, 1, tempTime);
            img.color = col;
            yield return null;
        }
        isFade = false;
        //LoadingSceneManager.LoadScene(SceneName);
        
    }
}
