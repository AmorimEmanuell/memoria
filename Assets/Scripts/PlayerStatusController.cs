using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatusController : MonoBehaviour
{
    [SerializeField] private Button _potionBtn;
    [SerializeField] private TextMeshProUGUI _potionCountText;
    [SerializeField] private Slider _playerHealthSlider;

    private const int PlayerMaxHealth = 3, PotionMaxCount = 3;

    private int _playerHealth, _potionCount;

    private int PlayerHealth
    {
        get { return _playerHealth; }
        set
        {
            _playerHealth = value;
            _playerHealthSlider.value = _playerHealth;

            if (_playerHealth == PlayerMaxHealth)
            {
                _potionBtn.interactable = false;
            }
        }
    }

    private int PotionCount
    {
        get { return _potionCount; }
        set
        {
            _potionCount = value;
            _potionCountText.text = _potionCount.ToString();

            if (_potionCount == 0)
            {
                _potionBtn.interactable = false;
            }
        }
    }

    private void Awake()
    {
        _potionBtn.onClick.AddListener(OnPotionBtnClicked);
    }

    private void Start()
    {
        _playerHealthSlider.maxValue = PlayerMaxHealth;

        PlayerHealth = PlayerMaxHealth;
        PotionCount = PotionMaxCount;
    }

    private void OnDestroy()
    {
        _potionBtn.onClick.RemoveAllListeners();
    }

    private void OnPotionBtnClicked()
    {
        PotionCount--;
        PlayerHealth++;
    }
}
