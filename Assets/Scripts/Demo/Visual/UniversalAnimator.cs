using System.Collections;
using UnityEngine;

public class UniversalAnimator : MonoBehaviour
{
    public void Animate(Vector2 endPosition, float speed)
    {
        StartCoroutine(TransformPosition(endPosition, speed));
    }

    public void Animate(Vector2 endPosition, float speed, bool isNeedToDestroyAtEnd)
    {
        if(!isNeedToDestroyAtEnd) StartCoroutine(TransformPosition(endPosition, speed));
        else StartCoroutine(TransformPositionAndDestroy(endPosition, speed));
    }

    public void AnimateWithOffset(Vector2 offset, float speed, bool isNeedToDestroyAtEnd)
    {
        Vector2 endPosition = (Vector2)transform.position + offset;
        Animate(endPosition, speed);
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
}
