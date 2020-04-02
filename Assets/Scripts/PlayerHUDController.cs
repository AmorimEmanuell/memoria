using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUDController : MonoBehaviour
{
    [SerializeField] private Button potionBtn = default;
    [SerializeField] private TextMeshProUGUI potionCount = default;
    [SerializeField] private Slider healthSlider = default;
    [SerializeField] private Image healthFill = default;
    [SerializeField] private Gradient healthColorGradient = default;
    [SerializeField] private TextMeshProUGUI displayedScore = default;

    private const float AnimationTime = 0.5f;

    private RectTransform displayedScoreRect;
    private Coroutine updateScoreTextAnimation;

    public Action OnPotionButtonClicked;

    private void Awake()
    {
        potionBtn.onClick.AddListener(() => OnPotionButtonClicked?.Invoke());
        displayedScoreRect = displayedScore.rectTransform;
    }

    private void OnDestroy()
    {
        potionBtn.onClick.RemoveAllListeners();
    }

    public void SetInitialValues(PlayerSaveData playerData)
    {
        healthSlider.maxValue = playerData.MaxHealth;
        healthSlider.value = playerData.MaxHealth;

        healthFill.color = healthColorGradient.Evaluate(1);

        potionCount.text = playerData.MaxPotions.ToString();
        potionBtn.interactable = false;

        displayedScore.text = "0";
    }

    public void UpdateHealth(int currentHealth, float healthPercentage)
    {
        healthSlider.DOValue(currentHealth, AnimationTime);

        var healthColor = healthColorGradient.Evaluate(healthPercentage);
        healthFill.DOColor(healthColor, AnimationTime);
    }

    public void UpdatePotions(int currentPotions)
    {
        potionCount.text = currentPotions.ToString();
    }

    public void ActivatePotionButton(bool active)
    {
        potionBtn.interactable = active;
    }

    public void UpdateScore(int currentScore)
    {
        displayedScoreRect.DOScale(1.5f, AnimationTime).OnComplete(() =>
        {
            displayedScoreRect.DOScale(1f, AnimationTime);
        });

        if (updateScoreTextAnimation != null)
        {
            StopCoroutine(updateScoreTextAnimation);
        }

        updateScoreTextAnimation = StartCoroutine(UpdateScoreAnimation(currentScore));
    }

    private IEnumerator UpdateScoreAnimation(int nextDisplayedScore)
    {
        var progress = 0f;
        var elapsedTime = 0f;
        var currentDisplayedScore = int.Parse(displayedScore.text);

        while (progress < 1)
        {
            elapsedTime += Time.deltaTime;
            progress = Mathf.Clamp01(elapsedTime / AnimationTime);

            var displayScore = (int)Mathf.Lerp(currentDisplayedScore, nextDisplayedScore, progress);
            displayedScore.text = displayScore.ToString();

            yield return null;
        }
    }
}
