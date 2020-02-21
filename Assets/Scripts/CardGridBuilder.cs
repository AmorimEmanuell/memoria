using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardGridBuilder : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup _grid = default;
    [SerializeField] private RectTransform _gridRectTransform = default;

    private const int MaxRows = 6, MaxColumns = 6;

    private List<Transform> _cells = new List<Transform>();

    public void CreateGrid(int rows, int columns)
    {
        Reset();

        _grid.constraintCount = columns;

        var numberOfCells = rows * columns;

        for (var i = 0; i < numberOfCells; i++)
        {
            var cell = ObjectPooler.Instance.Retrieve(PoolItemType.Cell);
            cell.transform.SetParent(_gridRectTransform);
            cell.transform.localPosition = Vector3.zero;
            cell.SetActive(true);
            _cells.Add(cell.transform);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(_gridRectTransform);
    }

    private void Reset()
    {
        if (_cells.Count == 0)
            return;

        for (var i = 0; i < _cells.Count; i++)
            ObjectPooler.Instance.Return(_cells[i].gameObject);

        _cells.Clear();
    }

    public Vector3[] GetPositions()
    {
        var positions = new Vector3[_cells.Count];

        for (var i = 0; i < positions.Length; i++)
            positions[i] = _cells[i].position;

        return positions;
    }
}
