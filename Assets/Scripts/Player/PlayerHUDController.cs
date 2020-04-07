using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUDController : MonoBehaviour
{
    [SerializeField] private Button potionBtn = default;
    [SerializeField] private RectTransform potionCountBg = default;
    [SerializeField] private TextMeshProUGUI potionCount = default;
    [SerializeField] private Slider healthSlider = default;
    [SerializeField] private Image healthFill = default;
    [SerializeField] private Gradient healthColorGradient = default;
    [SerializeField] private TextMeshProUGUI displayedScore = default;

    private const float
        AnimationTime = 0.5f,
        PotionCountAnimationScale = 1.3f,
        ScoreAnimationScale = 1.5f;

    private RectTransform displayedScoreRect;
    private Coroutine updateScoreTextRoutine;

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

    public void UpdatePotionCount(int currentPotions)
    {
        potionCount.text = currentPotions.ToString();

        potionCountBg.DOScale(PotionCountAnimationScale, AnimationTime / 2).OnComplete(() =>
        {
            potionCountBg.DOScale(1f, AnimationTime / 2);
        });
    }

    public void ActivatePotionButton(bool active)
    {
        potionBtn.interactable = active;
    }

    public void UpdateScore(int currentScore)
    {
        displayedScoreRect.DOScale(ScoreAnimationScale, AnimationTime).OnComplete(() =>
        {
            displayedScoreRect.DOScale(1f, AnimationTime);
        });

        if (updateScoreTextRoutine != null)
        {
            StopCoroutine(updateScoreTextRoutine);
        }

        updateScoreTextRoutine = StartCoroutine(UpdateScoreTextRoutine(currentScore));
    }

    private IEnumerator UpdateScoreTextRoutine(int nextDisplayedScore)
    {
        var elapsedTime = 0f;
        var currentDisplayedScore = int.Parse(displayedScore.text);

        while (elapsedTime < AnimationTime)
        {
            elapsedTime += Time.deltaTime;
            var progress = Mathf.Clamp01(elapsedTime / AnimationTime);

            var displayScore = Mathf.Lerp(currentDisplayedScore, nextDisplayedScore, progress);
            displayScore = Mathf.CeilToInt(displayScore);
            displayedScore.text = displayScore.ToString();

            yield return null;
        }
    }
}
