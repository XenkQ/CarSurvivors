using TMPro;
using UnityEngine;

[RequireComponent(typeof(GridController))]
public class FlowFieldDebug : MonoBehaviour
{
    private enum DisplayMode
    {
        CostField,
        IntegrationField,
        FlowField
    }

    [SerializeField] private float _fontSize;
    [SerializeField] private float _yOffset = 0.5f;
    [SerializeField] private DisplayMode _displayMode;
    private Grid _grid;
    private Cell[,] _cells;
    private float _cellSize;
    private Transform _flowFieldInfoHolder;

    private void Awake()
    {
        _flowFieldInfoHolder = new GameObject("FlowFieldInfoHolder").transform;
    }

    private void Start()
    {
        _flowFieldInfoHolder.transform.parent = GridController.Instance.transform;
        _grid = GridController.Instance.Grid;
        _cells = _grid.Cells;
        _cellSize = _grid.CellSize;

        GridController.Instance.OnGridUpdate.AddListener(DisplayCellDebugText);
    }

    private void DisplayCellDebugText()
    {
        bool infoWasCreatedPreviously = _flowFieldInfoHolder.transform.childCount > 0;
        int currentChildIndex = 0;
        for (int i = 0; i < _cells.GetLength(0); i++)
        {
            for (int j = 0; j < _cells.GetLength(1); j++)
            {
                Cell cell = _cells[i, j];
                Vector3 worldPos = cell.WorldPos;
                byte cellCost = cell.Cost;
                if (infoWasCreatedPreviously)
                {
                    var textComponent = _flowFieldInfoHolder.GetChild(currentChildIndex).GetComponent<TextMeshPro>();
                    textComponent.text = GetCellDebugTextOnMode(cell);
                    currentChildIndex++;
                }
                else
                {
                    CreateCellDebugText(cell, worldPos, cellCost.ToString());
                }
            }
        }
    }

    private void CreateCellDebugText(Cell cell, Vector3 worldPos, string name)
    {
        GameObject visualizer = new(name);
        visualizer.transform.parent = _flowFieldInfoHolder;
        visualizer.transform.SetPositionAndRotation(new Vector3(worldPos.x, _yOffset, worldPos.z),
                                                    Quaternion.Euler(new Vector3(90, 0, 0)));
        var rectTransform = visualizer.AddComponent<RectTransform>();
        rectTransform.rect.Set(0, 0, _cellSize, _cellSize);
        visualizer.AddComponent<MeshRenderer>();
        var textComponent = visualizer.AddComponent<TextMeshPro>();
        textComponent.text = GetCellDebugTextOnMode(cell);
        textComponent.fontSize = _fontSize;
        textComponent.alignment = TextAlignmentOptions.Center;
    }

    private string GetCellDebugTextOnMode(Cell cell) => _displayMode switch
    {
        DisplayMode.CostField => cell.Cost.ToString(),
        DisplayMode.IntegrationField => cell.BestCost.ToString(),
        DisplayMode.FlowField => cell.BestDirection.Vector.ToString(),
        _ => ""
    };
}