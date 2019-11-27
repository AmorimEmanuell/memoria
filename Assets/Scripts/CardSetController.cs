using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSetController : MonoBehaviour
{
    [SerializeField] private CardController _cardPrefab = default;
    [SerializeField] private CardGridBuilder _gridBuilder = default;
    [SerializeField] private CardDataCollection _collection = default;
    [SerializeField] private int _rows = 2;
    [SerializeField] private int _columns = 2;

    private void Start()
    {
        _gridBuilder.CreateGrid(_rows, _columns);
        var positions = _gridBuilder.GetPositions();

        var numberOfPairs = (_rows * _columns) / 2;

        var selectedCardsIds = SelectCardsFromCollection(_collection.GetIds(), numberOfPairs);
        selectedCardsIds.AddRange(new List<int>(selectedCardsIds));
        selectedCardsIds.Shuffle();

        for (var i = 0; i < selectedCardsIds.Count; i++)
        {
            var card = Instantiate(_cardPrefab, transform);
            card.SetData(_collection.GetData(selectedCardsIds[i]));
            card.transform.position = positions[i];
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
}
