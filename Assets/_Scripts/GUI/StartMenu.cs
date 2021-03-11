using System;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _highScoreButton;
    [SerializeField] private Button _creditsButton;

    [Header("Windows")]
    [SerializeField] private WindowPanel _highscoreWindow;

    private void Awake()
    {
        _startButton.onClick.AddListener(OnStartButtonClicked);
        _highScoreButton.onClick.AddListener(OnHighScoreButtonClicked);
        _creditsButton.onClick.AddListener(OnCreditsButtonClicked);
    }

    private void OnStartButtonClicked()
    {
        SceneLoader.Instance.LoadScene("BattleScene");
    }

    private void OnHighScoreButtonClicked()
    {
        _highscoreWindow.Open();
    }

    private void OnCreditsButtonClicked()
    {
        throw new NotImplementedException();
    }
}
