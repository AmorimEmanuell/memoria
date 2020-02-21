using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSetController : MonoBehaviour
{
    [SerializeField] private CardDataCollection _cardCollection = default;
    [SerializeField] private CardGridBuilder _gridBuilder = default;

    public Action OnPairFound, OnPairMiss;
    public int RemainingPairs => _remainingCards.Count / 2;

    private CardController _lastRevealedCard;
    private List<CardController> _remainingCards = new List<CardController>();

    public void CreateNewSet(int rows, int columns)
    {
        ClearRemainingCards();

        _gridBuilder.CreateGrid(rows, columns);

        var positions = _gridBuilder.GetPositions();
        var numOfCardPairs = (rows * columns) / 2;
        var selectedCardsIds = SelectCardsFromCollection(_cardCollection.GetIds(), numOfCardPairs);
        selectedCardsIds.AddRange(new List<int>(selectedCardsIds));
        selectedCardsIds.Shuffle();

        for (var i = 0; i < selectedCardsIds.Count; i++)
        {
            var card = ObjectPooler.Instance.Retrieve(PoolItemType.Card).GetComponent<CardController>();
            card.SetData(_cardCollection.GetItem(selectedCardsIds[i]));
            card.transform.position = positions[i];
            card.OnRevealFace += Card_OnRevealFace;
            card.gameObject.SetActive(true);
            card.IsInteractable = true;

            _remainingCards.Add(card);
        }
    }

    private void ClearRemainingCards()
    {
        for (var i = _remainingCards.Count - 1; i >= 0; i--)
        {
            _remainingCards[i].ReturnToPool();
            _remainingCards.RemoveAt(i);
        }
    }

    private List<int> SelectCardsFromCollection(List<int> idCollection, int numberOfPairs)
    {
        var selectedCardsIds = new List<int>();

        for (var i = 0; i < numberOfPairs; i++)
        {
            var next = UnityEngine.Random.Range(0, idCollection.Count);
            selectedCardsIds.Add(idCollection[next]);
            idCollection.RemoveAt(next);
        }

        return selectedCardsIds;
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
            _lastRevealedCard.OnRevealFace -= Card_OnRevealFace;
            _lastRevealedCard.Shrink();
            _remainingCards.Remove(_lastRevealedCard);

            currentRevealedCard.OnRevealFace -= Card_OnRevealFace;
            currentRevealedCard.Shrink();
            _remainingCards.Remove(currentRevealedCard);

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
        for (var i = 0; i < _remainingCards.Count; i++)
        {
            _remainingCards[i].IsInteractable = IsInteractable;
        }
    }
}
