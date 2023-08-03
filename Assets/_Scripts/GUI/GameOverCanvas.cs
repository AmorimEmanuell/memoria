using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverCanvas : MonoBehaviour
{
    [SerializeField] private Canvas canvas = default;
    [SerializeField] private RectTransform panel = default;
    [SerializeField] private Button restartBtn = default;
    [SerializeField] private TextMeshProUGUI scoreCount = default;

    private const float AnimationTime = 0.5f;

    private void Awake()
    {
        Events.instance.AddListener<PlayerDefeatEvent>(OnPlayerDefeatEvent);
        restartBtn.onClick.AddListener(OnRestartBtnClicked);
    }

    private void OnDestroy()
    {
        Events.instance.RemoveListener<PlayerDefeatEvent>(OnPlayerDefeatEvent);
        restartBtn.onClick.RemoveAllListeners();
    }

    private void OnPlayerDefeatEvent(PlayerDefeatEvent e)
    {
        scoreCount.text = e.FinalScore.ToString();
        panel.localScale = Vector3.zero;
        restartBtn.interactable = false;
        canvas.enabled = true;

        panel.DOScale(1, AnimationTime).SetEase(Ease.OutBack).OnComplete(() =>
        {
            restartBtn.interactable = true;
        });
    }

    private void OnRestartBtnClicked()
    {
        restartBtn.interactable = false;

        panel.DOScale(0, AnimationTime).OnComplete(() =>
        {
            canvas.enabled = false;
            Events.instance.Raise(new RestartEvent());
        });
    }
}
