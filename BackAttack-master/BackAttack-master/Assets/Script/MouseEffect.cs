using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseEffect : MonoBehaviour
{
    [SerializeField] Sprite[] EffectSprite;

    private Image MyImage;
    private SpriteRenderer MySprite;

    private WaitForSeconds Updatetime = new WaitForSeconds(0.025f);
    private bool mIsSprite;
    private void Awake()
    {
        if(this.GetComponent<SpriteRenderer>() != null)
        {
            MySprite = this.GetComponent<SpriteRenderer>();
            mIsSprite = true;
        }
        else if (this.GetComponent<Image>() != null)
        {
            MyImage = this.GetComponent<Image>();
            mIsSprite = false;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(Effect());
    }

    private IEnumerator Effect()
    {
        for(int i = 0; i < EffectSprite.Length; i++)
        {
            if(mIsSprite == true)
                MySprite.sprite = EffectSprite[i];
            else
                MyImage.sprite = EffectSprite[i];

            yield return Updatetime;
        }
        Destroy(this.gameObject);
    }

}
