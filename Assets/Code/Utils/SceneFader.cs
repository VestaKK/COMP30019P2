using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;
public class SceneFader : MonoBehaviour {

	#region FIELDS
	public Image fadeOutUIImage;
    [SerializeField] Splash splash;
    public bool fading = false;

	public enum FadeDirection
	{
		In, //Alpha = 1
		Out // Alpha = 0
	}

	#endregion

	#region MONOBHEAVIOR
	void OnEnable()
	{
		StartCoroutine(Fade(FadeDirection.Out));
	}
	#endregion
		
	#region FADE
	private IEnumerator Fade(FadeDirection fadeDirection) 
	{
        fading = true;
		float alpha = (fadeDirection == FadeDirection.Out)? 1 : 0;
		float fadeEndValue = (fadeDirection == FadeDirection.Out)? 0 : 1;
		if (fadeDirection == FadeDirection.Out) {
			while (alpha >= fadeEndValue)
			{
				SetColorImage (ref alpha, fadeDirection);
				yield return null;
			}
			fadeOutUIImage.enabled = false; 
		} else {
			fadeOutUIImage.enabled = true; 
			while (alpha <= fadeEndValue)
			{
				SetColorImage (ref alpha, fadeDirection);
				yield return null;
			}
		}
        fading = false;
	}
	#endregion

	#region HELPERS
    public IEnumerator FadeAndHide() {
		yield return Fade(FadeDirection.In);
        if(UIManager.instance.currentPanel != null)
            UIManager.instance.currentPanel.Hide();
    }

    public IEnumerator FadeAndShow(UIPanel next) {
        UIManager.instance.Show(next, 0f);
		yield return Fade(FadeDirection.Out);
    }

    public IEnumerator FadeTransition(UIPanel next) {
        yield return StartCoroutine(FadeAndHide());
        yield return StartCoroutine(FadeAndShow(next));
    }

	public IEnumerator FadeAndLoadScene(FadeDirection fadeDirection, string sceneToLoad) 
	{
		yield return Fade(fadeDirection);
		SceneManager.LoadScene(sceneToLoad);
	}

	private void SetColorImage(ref float alpha, FadeDirection fadeDirection)
	{
		fadeOutUIImage.color = new Color (fadeOutUIImage.color.r,fadeOutUIImage.color.g, fadeOutUIImage.color.b, alpha);
		alpha += Time.deltaTime * (1.0f / splash.fadeTime) * ((fadeDirection == FadeDirection.Out)? -1 : 1) ;
	}
	#endregion
}