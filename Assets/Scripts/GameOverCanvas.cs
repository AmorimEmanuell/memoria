using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverCanvas : MonoBehaviour
{
    [SerializeField] private Canvas _canvas = default;
    [SerializeField] private Button _restartBtn = default;

    private void Awake()
    {
        Events.instance.AddListener<PlayerDefeatEvent>(OnPlayerDefeatEvent);
        _restartBtn.onClick.AddListener(OnRestartBtnClicked);
    }

    private void OnDestroy()
    {
        Events.instance.RemoveListener<PlayerDefeatEvent>(OnPlayerDefeatEvent);
        _restartBtn.onClick.RemoveAllListeners();
    }

    private void OnPlayerDefeatEvent(PlayerDefeatEvent e)
    {
        _canvas.enabled = true;
    }

    private void OnRestartBtnClicked()
    {
        _canvas.enabled = false;
        Events.instance.Raise(new RestartEvent());
    }
}
