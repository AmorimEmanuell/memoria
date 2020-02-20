using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSetController : MonoBehaviour
{
    [SerializeField] private CardDataCollection _cardCollection = default;
    [SerializeField] private CardGridBuilder _gridBuilder = default;

    private CardController _lastRevealedCard;
    private int _remainingPairs;

    public Action<CardController, CardController> OnPairFound;
    public Action OnPairMiss;

    public int RemainingPairs => _remainingPairs;

    public void CreateNewSet(int rows, int columns)
    {
        _gridBuilder.CreateGrid(rows, columns);
        var positions = _gridBuilder.GetPositions();

        _remainingPairs = (rows * columns) / 2;

        var selectedCardsIds = SelectCardsFromCollection(_cardCollection.GetIds(), _remainingPairs);
        selectedCardsIds.AddRange(new List<int>(selectedCardsIds));
        selectedCardsIds.Shuffle();

        for (var i = 0; i < selectedCardsIds.Count; i++)
        {
            var card = ObjectPooler.Instance.Retrieve(PoolItemType.Card).GetComponent<CardController>();
            card.SetData(_cardCollection.GetItem(selectedCardsIds[i]));
            card.transform.position = positions[i];
            card.OnRevealFace += Card_OnRevealFace;
            card.gameObject.SetActive(true);
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

        Events.instance.Raise(new BlockCardSelectionEvent(true));

        if (_lastRevealedCard.CardId == currentRevealedCard.CardId)
        {
            _lastRevealedCard.OnRevealFace -= Card_OnRevealFace;
            currentRevealedCard.OnRevealFace -= Card_OnRevealFace;

            _remainingPairs--;

            Events.instance.Raise(new BlockCardSelectionEvent(false));

            OnPairFound?.Invoke(_lastRevealedCard, currentRevealedCard);
        }
        else
        {
            _lastRevealedCard.HideFace();
            currentRevealedCard.HideFace(UnblockCardSelection);

            OnPairMiss?.Invoke();
        }

        _lastRevealedCard = null;
    }

    private void UnblockCardSelection()
    {
        Events.instance.Raise(new BlockCardSelectionEvent(false));
    }
}
