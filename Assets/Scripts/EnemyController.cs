using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Slider _healthSlider = default;
    [SerializeField] private TextMeshProUGUI _healthText = default;
    [SerializeField] private Image _healthFill = default;
    [SerializeField] private Gradient _colorGradient = default;

    private int _currentHealth, _maxHealth;

    private void Awake()
    {
        Events.instance.AddListener<PlayerAttackEvent>(OnPlayerAttack);
    }

    private void OnDestroy()
    {
        Events.instance.AddListener<PlayerAttackEvent>(OnPlayerAttack);
    }

    public void SetData(EnemyData enemyData)
    {
        _maxHealth = enemyData.Health;
        _healthSlider.maxValue = _maxHealth;
        _healthSlider.value = _maxHealth;

        SetCurrentHealth(enemyData.Health);
    }

    private void SetCurrentHealth(int health)
    {
        _currentHealth = health;
        _healthText.text = _currentHealth.ToString();
    }

    private void OnPlayerAttack(PlayerAttackEvent e)
    {
        //UpdateHealth
    }
}
