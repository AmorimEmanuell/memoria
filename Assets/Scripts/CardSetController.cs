using System;
using System.Collections;
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

    public void SetupGame(int rows, int cols)
    {
        var gridPositions = _gridBuilder.CreateGrid(rows, cols);
        _currentSet = _setCreator.GetNewSet(rows, cols);

        for (var i = 0; i < _currentSet.Count; i++)
        {
            _currentSet[i].transform.position = gridPositions[i];
            _currentSet[i].OnFaceRevealed += Card_OnRevealFace;
        }
    }

    private void Card_OnRevealFace(CardController currentRevealedCard)
    {
        if (_lastRevealedCard == null)
        {
            _lastRevealedCard = currentRevealedCard;
            return;
        }

        if (_lastRevealedCard.CardId == currentRevealedCard.CardId)
        {
            _lastRevealedCard.OnFaceRevealed -= Card_OnRevealFace;
            _lastRevealedCard.Shrink();
            _currentSet.Remove(_lastRevealedCard);

            currentRevealedCard.OnFaceRevealed -= Card_OnRevealFace;
            currentRevealedCard.Shrink();
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

    public void SetCardsInteractable(bool IsInteractable)
    {
        for (var i = 0; i < _currentSet.Count; i++)
        {
            _currentSet[i].IsInteractable = IsInteractable;
        }
    }
}
