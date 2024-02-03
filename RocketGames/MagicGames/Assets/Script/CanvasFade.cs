using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFade : MonoBehaviour
{
    [SerializeField] float fadeDuration = .4f;
    
    public void Fade()
    {
        var canvasGroup = GetComponent<CanvasGroup>();

        StartCoroutine(DoFade(canvasGroup));
    }


    IEnumerator DoFade(CanvasGroup canvasGroup)
    {
        float counter = 0f;

        while (counter < fadeDuration)
        {
            counter += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, counter / fadeDuration);

            yield return null;
        }
    }
}
