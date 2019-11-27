using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardGridBuilder : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup _grid = default;
    [SerializeField] private int _rows = 3;
    [SerializeField] private int _columns = 4;

    private int _numberOfCells;
    private List<Transform> _positions = new List<Transform>();

    private void Awake()
    {
        CreateGrid(_rows, _columns);
    }

    public void CreateGrid(int rows, int columns)
    {
        _grid.constraintCount = columns;

        _numberOfCells = rows * columns;
        var gridTransform = _grid.transform;

        for (var i = 0; i < _numberOfCells; i++)
        {
            var pos = new GameObject("pos" + i, typeof(RectTransform));
            pos.transform.SetParent(gridTransform);
            _positions.Add(pos.transform);
        }
    }
}
