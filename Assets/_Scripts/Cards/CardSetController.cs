using System;
using System.Collections.Generic;
using UnityEngine;

public class CardSetController : MonoBehaviour
{
    [SerializeField] private CardSetCreator _setCreator = default;
    [SerializeField] private CardGridBuilder _gridBuilder = default;

    public Action OnPairFound, OnPairMiss;
    public int RemainingPairs => _currentSet.Count / 2;

    private CardController _lastRevealedCard;
    private List<CardController> _currentSet = new List<CardController>();

    public void SetupNewGame(int rows, int cols)
    {
        ClearRemainingCards();

        var gridPositions = _gridBuilder.CreateGrid(rows, cols);
        _currentSet = _setCreator.GetNewSet(rows, cols);

        for (var i = 0; i < _currentSet.Count; i++)
        {
            _currentSet[i].transform.position = gridPositions[i];
            _currentSet[i].OnFaceRevealed += Card_OnFaceRevealed;
        }

        SetCardsInteractable(true);
    }

    public void ClearRemainingCards()
    {
        _lastRevealedCard = null;

        for (var i = 0; i < _currentSet.Count; i++)
        {
            _currentSet[i].OnFaceRevealed -= Card_OnFaceRevealed;
            ResetCard(_currentSet[i]);
        }

        _currentSet.Clear();
    }

    private void Card_OnFaceRevealed(CardController currentRevealedCard)
    {
        if (_lastRevealedCard == null)
        {
            _lastRevealedCard = currentRevealedCard;
            return;
        }

        if (_lastRevealedCard.CardId == currentRevealedCard.CardId)
        {
            _lastRevealedCard.OnFaceRevealed -= Card_OnFaceRevealed;
            _lastRevealedCard.Shrink(ResetCard);
            _currentSet.Remove(_lastRevealedCard);

            currentRevealedCard.OnFaceRevealed -= Card_OnFaceRevealed;
            currentRevealedCard.Shrink(ResetCard);
            _currentSet.Remove(currentRevealedCard);

            OnPairFound?.Invoke();
        }
        else
        {
            _lastRevealedCard.HideFace();
            currentRevealedCard.HideFace();

            OnPairMiss?.Invoke();
        }

        _lastRevealedCard = null;
    }

    private void ResetCard(CardController card)
    {
        card.ResetState();
        ObjectPooler.Instance.Return(card.gameObject);
    }

    public void SetCardsInteractable(bool IsInteractable)
    {
        for (var i = 0; i < _currentSet.Count; i++)
        {
            _currentSet[i].ForceIsInteractable(IsInteractable);
        }
    }
}
