using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    [SerializeField] private float ChangeTime;
    [SerializeField] private Text TextGuide;

    private bool isReady;
     
    void Awake()
    {
        FadeInOut.Instance.FadeOut();
        isReady = false;

        StartCoroutine(Prod_Text());
        StartCoroutine(LoadScene());

        Invoke("Ready", ChangeTime);
    }

    private void Ready()
    {
        isReady = true;
    }

    private IEnumerator Prod_Text()
    {
        WaitForSeconds ProdTime = new WaitForSeconds(0.8f);

        string[] ProdText = {"" , "."  , ".."  ,"..." };
        string ProdStr = "Loading ";

        int ProdTextCount = 0;
        TextGuide.text = ProdStr;

        while (true)
        {
            yield return ProdTime;
            TextGuide.text = ProdStr + ProdText[ProdTextCount++];
            ProdTextCount = ProdTextCount >= ProdText.Length ? 0 : ProdTextCount;
        }
    }


   private  IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation NextScene = SceneManager.LoadSceneAsync("Scene_Title");
        NextScene.allowSceneActivation = false;

        while (!NextScene.isDone)
        {
            yield return null;
            if (NextScene.progress >= 0.9f)
            {
                if (isReady) break;
            }
        }

        FadeInOut.Instance.FadeIn(() => NextScene.allowSceneActivation = true, FadeInOut.eFadeActiveOption.None);
    }

}
