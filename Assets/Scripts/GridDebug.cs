using TMPro;
using UnityEngine;

[RequireComponent(typeof(GridController))]
public class GridDebug : MonoBehaviour
{
    [SerializeField]
    [ColorUsage(false)] private Color _cellBorderColor = Color.blue;

    private GridController _gridController;
    private Grid _grid;

    private void Awake()
    {
        _gridController = GetComponent<GridController>();
    }

    private void Start()
    {
        _grid = _gridController.Grid;
    }

    private void FixedUpdate()
    {
        DisplayCellBorders();
    }

    private void DisplayCellBorders()
    {
        Cell[,] cells = _grid.Cells;
        float cellSize = _grid.CellSize;
        for (int i = 0; i < cells.GetLength(0); i++)
        {
            Vector3 position = cells[i, 0].WorldPos;
            for (int j = 0; j < cells.GetLength(1); j++)
            {
                position = cells[i, j].WorldPos;
                Debug.DrawLine(new Vector3(position.x - cellSize / 2, 0, position.z - cellSize / 2),
                               new Vector3(position.x - cellSize / 2, 0, position.z + cellSize / 2),
                               _cellBorderColor);
                Debug.DrawLine(new Vector3(position.x - cellSize / 2, 0, position.z - cellSize / 2),
                               new Vector3(position.x + cellSize / 2, 0, position.z - cellSize / 2),
                               _cellBorderColor);
            }
        }

        float horizontalCellsSize = _grid.Width * cellSize;
        float verticalCellSize = _grid.Height * cellSize;
        Debug.DrawLine(new Vector3(0, 0, verticalCellSize),
                       new Vector3(horizontalCellsSize, 0, verticalCellSize),
                       _cellBorderColor);
        Debug.DrawLine(new Vector3(horizontalCellsSize, 0, 0),
                       new Vector3(horizontalCellsSize, 0, verticalCellSize),
                       _cellBorderColor);
    }
}