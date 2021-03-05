using System;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _highScoreButton;
    [SerializeField] private Button _creditsButton;

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
        throw new NotImplementedException();
    }

    private void OnCreditsButtonClicked()
    {
        throw new NotImplementedException();
    }
}
