using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Grid
{
    private int _width;
    private int _height;
    private float _cellSize;
    private Cell[,] _grid;

    public Grid(int width, int height, float cellSize = 0.5f)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;

        Cell[,] grid = new Cell[width, height];
        Vector3 currentTilePos = Vector3.zero;
        GameObject gridHolder = new(nameof(Grid));
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var gridPos = new Vector2Int(i, j);
                grid[i, j] = new Cell(currentTilePos, gridPos);

#if DEBUG
                GameObject visualizer = new(gridPos.ToString());
                visualizer.transform.parent = gridHolder.transform;
                visualizer.transform.SetPositionAndRotation(new Vector3(currentTilePos.x, 0, currentTilePos.z),
                                                            Quaternion.Euler(new Vector3(90, 0, 0)));
                var rectTransform = visualizer.AddComponent<RectTransform>();
                rectTransform.rect.Set(0, 0, _cellSize, _cellSize);
                visualizer.AddComponent<MeshRenderer>();
                var textComponent = visualizer.AddComponent<TextMeshPro>();
                textComponent.text = gridPos.ToString();
                textComponent.fontSize = cellSize;
                textComponent.alignment = TextAlignmentOptions.Center;
                Debug.DrawLine(new Vector3(currentTilePos.x - _cellSize, 0, currentTilePos.z),
                               new Vector3(currentTilePos.x - _cellSize, 0, currentTilePos.z + _cellSize), Color.blue);
                Debug.DrawLine(new Vector3(currentTilePos.x - _cellSize, 0, currentTilePos.z - _cellSize),
                               new Vector3(currentTilePos.x + _cellSize, 0, currentTilePos.z - _cellSize), Color.blue);
#endif
                currentTilePos.z += _cellSize;
            }
            currentTilePos = new Vector3(currentTilePos.x + _cellSize, 0, 0);
        }
    }
}