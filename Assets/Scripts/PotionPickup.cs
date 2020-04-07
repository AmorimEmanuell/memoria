using System;
using System.Collections;
using UnityEngine;

public class PotionPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private SpriteRenderer spriteRenderer = default;

    private const float
        PepareToFadeTime = 5f,
        FadeTime = 5f,
        FadeAcceleration = 1.2f,
        SineOffset = 1f,
        SineNormalizer = 2f,
        Radians360 = Mathf.PI * 2,
        SineStartAngle = Mathf.PI / 2f,
        FadeVelocityMin = 1f,
        FadeVelocityMax = 1.7f;

    private Coroutine fadeRoutine;

    public Action<PotionPickup> OnPickup, OnFadeRoutineComplete;

    public void OnClick()
    {
    }

    public void OnTouchDown()
    {
        OnPickup?.Invoke(this);
    }

    public void OnTouchUp()
    {
    }

    public void PrepareToFade()
    {
#if D
        Invoke(nameof(StartFadeRoutine), 0f);
#else
        Invoke(nameof(StartFadeRoutine), PepareToFadeTime);
#endif
    }

    private void StartFadeRoutine()
    {
        fadeRoutine = StartCoroutine(FadeRoutine());
    }

    private IEnumerator FadeRoutine()
    {
        var elapsedTime = 0f;

        while (elapsedTime < FadeTime)
        {
            elapsedTime += Time.deltaTime;
            elapsedTime = Mathf.Clamp(elapsedTime, 0f, FadeTime);

            var progress = elapsedTime / FadeTime;

            //This is used to accelerate the fadein/fadeout of the potion as time passes
            var fadeVelocity = Mathf.Lerp(FadeVelocityMin, FadeVelocityMax, progress);
            var fadeProgress = elapsedTime * fadeVelocity * Radians360;
            var currentSine = Mathf.Sin(fadeProgress + SineStartAngle);

            //The sine returns a value between -1 and 1. Here we normalize it to 0 and 1.
            var alpha = (currentSine + SineOffset) / SineNormalizer;

            SetSpriteRendererColorAlpha(alpha);

            yield return null;
        }

        OnFadeRoutineComplete?.Invoke(this);
    }

    public void ResetState()
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        SetSpriteRendererColorAlpha(1);
    }

    private void SetSpriteRendererColorAlpha(float alpha)
    {
        var color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }
}
