using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHUDController : MonoBehaviour
{
    [SerializeField] private Slider _healthSlider = default;
    [SerializeField] private TextMeshProUGUI _healthCounter = default;
    [SerializeField] private Image _healthFill = default;
    [SerializeField] private Gradient _colorGradient = default;

    private const float AnimationTime = 0.5f;

    public void SetInitialValues(int maxHealth)
    {
        _healthSlider.maxValue = maxHealth;
        _healthSlider.value = maxHealth;
        _healthCounter.text = maxHealth.ToString();
        _healthFill.color = _colorGradient.Evaluate(1);
    }

    public void UpdateHealth(int currentHealth, float healthPercentage)
    {
        _healthSlider.DOValue(currentHealth, AnimationTime);
        _healthFill.DOColor(_colorGradient.Evaluate(healthPercentage), AnimationTime);
        _healthCounter.text = currentHealth.ToString();
    }
}
