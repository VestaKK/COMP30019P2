
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField]
    private Image _progressImage;
    [SerializeField]
    private float _defaultSpeed = 1f;
    [SerializeField]
    private Gradient _colorGradient;
    [SerializeField]
    private UnityEvent<float> _onProgress;
    [SerializeField]
    private UnityEvent _onCompleted;

    private Coroutine _animationCoroutine;

    private void Start()
    {
        if (_progressImage.type != Image.Type.Filled)
        {
            Debug.LogError($"{name}'s ProgressImage is not of type \"Filled\" so it cannot be used as a progress bar. Disabling this Progress Bar.");
            enabled = false;
        #if UNITY_EDITOR
            EditorGUIUtility.PingObject(this.gameObject);
        #endif
        }
    }

    public void SetProgress(float _progress)
    {
        SetProgress(_progress, _defaultSpeed);
    }

    public void SetProgressInstant(float _progress)
    {
        _progressImage.fillAmount = _progress;
    }

    public void SetProgress(float Progress, float Speed)
    {
        if(!gameObject.activeInHierarchy) {
            if(_animationCoroutine != null) {
                StopCoroutine(_animationCoroutine);
            }
            return;
        }

        if (Progress < 0 || Progress > 1)
        {
            Debug.LogWarning($"Invalid progress passed, expected value is between 0 and 1, got {Progress}. Clamping.");
            Progress = Mathf.Clamp01(Progress);
        }
        if (Progress != _progressImage.fillAmount)
        {
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
            }

            _animationCoroutine = StartCoroutine(AnimateProgress(Progress, Speed));
        }
    }

    private IEnumerator AnimateProgress(float Progress, float Speed)
    {
        float time = 0;
        float initialProgress = _progressImage.fillAmount;

        while (time < 0.5)
        {
            _progressImage.fillAmount = Mathf.Lerp(initialProgress, Progress, time);
            time += Time.unscaledDeltaTime * Speed;

            _progressImage.color = _colorGradient.Evaluate(_progressImage.fillAmount);

            _onProgress?.Invoke(_progressImage.fillAmount);
            yield return null;
        }

        _progressImage.fillAmount = Progress;
        _progressImage.color = _colorGradient.Evaluate(_progressImage.fillAmount);

        _onProgress?.Invoke(Progress);
        _onCompleted?.Invoke();
    }

    public Image ProgressImage { get => this._progressImage; set => this._progressImage = value; }
}