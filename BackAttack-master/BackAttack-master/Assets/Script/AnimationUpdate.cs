using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationUpdate : MonoBehaviour
{
    [SerializeField] private Sprite[] AnimationSprites;
    [SerializeField] private float    AnimationUpdateTime = 0.1f;

    private Image AnimationResource;
    private int SpriteCount;

    private void OnEnable()
    {
        AnimationResource = this.GetComponent<Image>();

        SpriteCount = 0;
        AnimationResource.sprite = AnimationSprites[SpriteCount];
        StartCoroutine(AnimationUpdateLoop());
    }

    private IEnumerator AnimationUpdateLoop()
    {
        WaitForSeconds AnimationTIme = new WaitForSeconds(AnimationUpdateTime);

        while(true)
        {
            yield return AnimationTIme;
            AnimationResource.sprite = AnimationSprites[SpriteCount++];
            SpriteCount = SpriteCount >= AnimationSprites.Length ? 0 : SpriteCount;
        }
    }

}
