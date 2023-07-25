﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreWindow : WindowPanel
{
    [SerializeField] private TextMeshProUGUI _highscore;
    [SerializeField] private Button _button;

    private void Awake()
    {
        _button.onClick.AddListener(() => Close());
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }

    public override void Open()
    {
        base.Open();
        _highscore.SetText("{0}", HighscoreDataManager.GetHighscore());
    }
}