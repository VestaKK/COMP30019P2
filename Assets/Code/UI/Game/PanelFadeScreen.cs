using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PanelFadeScreen : MonoBehaviour
{
    private Image panelImage;

    private void Awake()
    {
        panelImage = transform.gameObject.GetComponent<Image>();
    }

    public void FadeEffect() 
    {
        Color temp = panelImage.color;
        temp.a = 0.3f;
        panelImage.color = temp;
        StartCoroutine(FadeEffectCoroutine());
    }

    private IEnumerator FadeEffectCoroutine()
    {   
        while (panelImage.color.a > 0.05)
        {
            Color temp = panelImage.color;
            temp.a = temp.a - 0.3f * Time.deltaTime;
            panelImage.color = temp;
            yield return null;
        }

        Color final = panelImage.color;
        final.a = 0;
        panelImage.color = final;

        yield return null;
    }
}
