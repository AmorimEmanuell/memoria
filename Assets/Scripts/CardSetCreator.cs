using System.Collections.Generic;
using UnityEngine;

public class CardSetCreator : MonoBehaviour
{
    [SerializeField] private CardDataCollection _cardCollection = default;

    public List<CardController> GetNewSet(int rows, int columns)
    {
        var pairs = (rows * columns) / 2;
        var selectedCards = ChooseCardsFromCollection(_cardCollection.GetIds(), pairs);
        selectedCards.AddRange(new List<int>(selectedCards));
        selectedCards.Shuffle();

        var cardSet = new List<CardController>();

        for (var i = 0; i < selectedCards.Count; i++)
        {
            var card = ObjectPooler.Instance.Retrieve(PoolItemType.Card).GetComponent<CardController>();
            card.SetData(_cardCollection.GetItem(selectedCards[i]));
            card.IsInteractable = true;
            card.gameObject.SetActive(true);
            cardSet.Add(card);
        }

        return cardSet;
    }

    private List<int> ChooseCardsFromCollection(List<int> idCollection, int numberOfPairs)
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
}
