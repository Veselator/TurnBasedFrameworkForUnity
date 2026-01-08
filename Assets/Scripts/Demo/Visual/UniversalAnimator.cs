using System;
using System.Collections;
using UnityEngine;

public class UniversalAnimator : MonoBehaviour
{
    // Класс, который содержит методы для любой анимации, которая может потребоваться
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
        {
            Debug.LogError("ПРОГРАММИСТ, ЮНИТИ - ГДЕ SУКА SPRITERENDERER?");
        }
    }

    public void Animate(Vector2 endPosition, float speed)
    {
        StartCoroutine(TransformPosition(endPosition, speed));
    }

    public void Animate(Vector2 endPosition, float speed, bool isNeedToDestroyAtEnd)
    {
        if (!isNeedToDestroyAtEnd) StartCoroutine(TransformPosition(endPosition, speed));
        else StartCoroutine(TransformPositionAndDestroy(endPosition, speed));
    }

    public void AnimateWithOffset(Vector2 offset, float speed, bool isNeedToDestroyAtEnd)
    {
        Vector2 endPosition = (Vector2)transform.position + offset;
        Animate(endPosition, speed, isNeedToDestroyAtEnd);
    }

    public IEnumerator TransformPosition(Vector2 endPosition, float speed)
    {
        Vector2 startPosition = transform.position;
        float distance = Vector2.Distance(startPosition, endPosition);
        float duration = distance / speed;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            float easedT = t * t;
            transform.position = Vector2.Lerp(startPosition, endPosition, easedT);
            yield return null;
        }

        transform.position = endPosition;
    }

    public IEnumerator TransformPositionAndDestroy(Vector2 endPosition, float speed)
    {
        yield return TransformPosition(endPosition, speed);
        Destroy(gameObject);
    }

    public void Animate(Vector3 endPosition, float speed)
    {
        StartCoroutine(TransformPosition(endPosition, speed));
    }

    public void Animate(Vector3 endPosition, float speed, bool isNeedToDestroyAtEnd)
    {
        if (!isNeedToDestroyAtEnd) StartCoroutine(TransformPosition(endPosition, speed));
        else StartCoroutine(TransformPositionAndDestroy(endPosition, speed));
    }

    public void AnimateWithOffset(Vector3 offset, float speed, bool isNeedToDestroyAtEnd)
    {
        Vector3 endPosition = transform.position + offset;
        Animate(endPosition, speed, isNeedToDestroyAtEnd);
    }

    public IEnumerator TransformPosition(Vector3 endPosition, float speed)
    {
        Vector3 startPosition = transform.position;
        float distance = Vector3.Distance(startPosition, endPosition);
        float duration = distance / speed;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            float easedT = t * t;
            transform.position = Vector3.Lerp(startPosition, endPosition, easedT);
            yield return null;
        }

        transform.position = endPosition;
    }

    public IEnumerator TransformPositionAndDestroy(Vector3 endPosition, float speed)
    {
        yield return TransformPosition(endPosition, speed);
        Destroy(gameObject);
    }

    public void AnimateSpriteSize(Vector2 targetSize, float duration, Action onComplete = null)
    {
        StartCoroutine(AnimateSpriteSizeCoroutine(_spriteRenderer, targetSize, duration, onComplete));
    }

    public void AnimateSpriteSize(Vector2 startSize, Vector2 targetSize, float duration, Action onComplete = null)
    {
        _spriteRenderer.size = startSize;
        StartCoroutine(AnimateSpriteSizeCoroutine(_spriteRenderer, targetSize, duration, onComplete));
    }

    private IEnumerator AnimateSpriteSizeCoroutine(SpriteRenderer sr, Vector2 targetSize, float duration, Action onComplete)
    {
        Vector2 startSize = sr.size;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            float easedT = 1f - (1f - t) * (1f - t);
            sr.size = Vector2.Lerp(startSize, targetSize, easedT);
            yield return null;
        }

        sr.size = targetSize;
        onComplete?.Invoke();
    }

    public void AnimateSpriteSizeWithOvershoot(Vector2 targetSize, float duration, float overshootFactor, Action onComplete = null)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        StartCoroutine(AnimateSpriteSizeWithOvershootCoroutine(sr, targetSize, duration, overshootFactor, 0f, onComplete));
    }

    public void AnimateSpriteSizeWithOvershoot(Vector2 startSize, Vector2 targetSize, float duration, float overshootFactor, Action onComplete = null)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        sr.size = startSize;
        StartCoroutine(AnimateSpriteSizeWithOvershootCoroutine(sr, targetSize, duration, overshootFactor, 0f, onComplete));
    }

    public void AnimateSpriteSizeWithOvershoot(Vector2 startSize, Vector2 targetSize, float duration, float overshootFactor, float timeBeforeAnimation, Action onComplete = null)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        sr.size = startSize;
        StartCoroutine(AnimateSpriteSizeWithOvershootCoroutine(sr, targetSize, duration, overshootFactor, timeBeforeAnimation, onComplete));
    }

    private IEnumerator AnimateSpriteSizeWithOvershootCoroutine(SpriteRenderer sr, Vector2 targetSize, float duration, float overshootFactor, float timeBeforeAnimation, Action onComplete)
    {
        Vector2 startSize = sr.size;
        float elapsedTime = 0f;

        if(timeBeforeAnimation > 0) yield return new WaitForSeconds(timeBeforeAnimation);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            float easedT = EaseOutBack(t, overshootFactor);
            sr.size = Vector2.LerpUnclamped(startSize, targetSize, easedT);
            yield return null;
        }

        sr.size = targetSize;
        onComplete?.Invoke();
    }

    private float EaseOutBack(float t, float overshoot)
    {
        float c1 = overshoot;
        float c3 = c1 + 1f;
        return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
    }

    public void AnimateScale(Vector3 targetScale, float duration, Action onComplete = null)
    {
        StartCoroutine(AnimateScaleCoroutine(targetScale, duration, onComplete));
    }

    public void AnimateScale(Vector3 startScale, Vector3 targetScale, float duration, Action onComplete = null)
    {
        transform.localScale = startScale;
        StartCoroutine(AnimateScaleCoroutine(targetScale, duration, onComplete));
    }

    private IEnumerator AnimateScaleCoroutine(Vector3 targetScale, float duration, Action onComplete)
    {
        Vector3 startScale = transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            float easedT = 1f - (1f - t) * (1f - t);
            transform.localScale = Vector3.Lerp(startScale, targetScale, easedT);
            yield return null;
        }

        transform.localScale = targetScale;
        onComplete?.Invoke();
    }

    public void SetPosition(Vector2 position)
    {
        transform.position = position;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void InterpolateColor(Color endColor, float time)
    {
        StartCoroutine(ColorInterpolationAnimation(endColor, time));
    }

    private IEnumerator ColorInterpolationAnimation(Color toColor, float duration)
    {
        Color startColor = _spriteRenderer.color;
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            float t = Mathf.Clamp01(timeElapsed / duration);

            _spriteRenderer.color = Color.Lerp(startColor, toColor, t);

            yield return null;

            timeElapsed += Time.deltaTime;
        }

        _spriteRenderer.color = toColor;
    }
}