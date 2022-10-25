using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TextDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text _interactText;
    [SerializeField] TMP_Text _fadingText;

    public IEnumerator DisplayFadingTextCoroutine(string Text, float duration) 
    {
        float startingAlpha = 0.8f;
        float timePassed = 0f;


        Color temp = _fadingText.color;
        temp.a = startingAlpha;
        _fadingText.color = temp;

        _fadingText.text = Text;

        while (timePassed < duration) 
        {
            temp = _fadingText.color;
            temp.a = startingAlpha * (1 - (timePassed/duration));
            timePassed += Time.deltaTime;
            _fadingText.color = temp;
            yield return null;
        }

        temp = _fadingText.color;
        temp.a = 0;
        _fadingText.color = temp;
        StopCoroutine("DisplayFadingTextCoroutine");
    }

    public void DisplayFadingText(string Text, float duration) 
    {
        StartCoroutine(DisplayFadingTextCoroutine(Text, duration));
    }

    public void DisplayInteractiveText(string message) 
    {
        Color temp = _interactText.color;
        temp.a = 1;
        _interactText.color = temp;
        _interactText.text = message;
    }

    public void DropInteractiveText()
    {
        Color temp = _interactText.color;
        temp.a = 0;
        _interactText.color = temp;
    }

}
