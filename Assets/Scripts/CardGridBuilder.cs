using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardGridBuilder : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup _grid = default;
    [SerializeField] private RectTransform _gridRectTransform;

    private RectTransform[] _cells;

    public void CreateGrid(int rows, int columns)
    {
        _grid.constraintCount = columns;

        var numberOfCells = rows * columns;
        _cells = new RectTransform[numberOfCells];

        for (var i = 0; i < numberOfCells; i++)
        {
            var cell = new GameObject("cell" + i, typeof(RectTransform));
            cell.transform.SetParent(_gridRectTransform);
            _cells[i] = cell.GetComponent<RectTransform>();
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(_gridRectTransform);
    }

    public Vector3[] GetPositions()
    {
        var positions = new Vector3[_cells.Length];
        for (var i = 0; i < positions.Length; i++)
            positions[i] = _cells[i].position;

        return positions;
    }
}
