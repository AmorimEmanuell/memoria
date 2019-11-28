using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSetController : MonoBehaviour
{
    [SerializeField] private CardGridBuilder _gridBuilder = default;
    [SerializeField] private CardDataCollection _collection = default;
    [SerializeField] private int _rows = 2;
    [SerializeField] private int _columns = 2;

    private CardController _lastRevealedCard;
    private int _numberOfPairs;

    private void Start()
    {
        CreateNewSet();
    }

    private void CreateNewSet()
    {
        _gridBuilder.CreateGrid(_rows, _columns);
        var positions = _gridBuilder.GetPositions();

        _numberOfPairs = (_rows * _columns) / 2;

        var selectedCardsIds = SelectCardsFromCollection(_collection.GetIds(), _numberOfPairs);
        selectedCardsIds.AddRange(new List<int>(selectedCardsIds));
        selectedCardsIds.Shuffle();

        for (var i = 0; i < selectedCardsIds.Count; i++)
        {
            var card = ObjectPooler.Instance.Retrieve(PoolItemType.Card).GetComponent<CardController>();
            card.SetData(_collection.GetData(selectedCardsIds[i]));
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

            _lastRevealedCard.Shrink();
            currentRevealedCard.Shrink(() =>
            {
                UnblockCardSelection();
                CheckForEndOfGameSet();
            });
        }
        else
        {
            _lastRevealedCard.HideFace();
            currentRevealedCard.HideFace(UnblockCardSelection);
        }

        _lastRevealedCard = null;
    }

    private void UnblockCardSelection()
    {
        Events.instance.Raise(new BlockCardSelectionEvent(false));
    }

    private void CheckForEndOfGameSet()
    {
        if (--_numberOfPairs == 0)
            CreateNewSet();
    }
}
