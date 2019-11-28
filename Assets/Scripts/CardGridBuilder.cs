using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardGridBuilder : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup _grid = default;
    [SerializeField] private RectTransform _gridRectTransform = default;

    private List<Transform> _cells = new List<Transform>();

    public void CreateGrid(int rows, int columns)
    {
        _grid.constraintCount = columns;

        var numberOfCells = rows * columns;

        for (var i = 0; i < numberOfCells; i++)
        {
            var cell = new GameObject("cell" + i, typeof(RectTransform));
            cell.transform.SetParent(_gridRectTransform);
            cell.transform.localPosition = Vector3.zero;
            _cells.Add(cell.transform);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(_gridRectTransform);
    }

    public Vector3[] GetPositions()
    {
        var positions = new Vector3[_cells.Count];

        for (var i = 0; i < positions.Length; i++)
            positions[i] = _cells[i].position;

        return positions;
    }
}
