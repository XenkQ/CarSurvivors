using UnityEngine;

namespace Grid
{
    public class Grid
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public float CellSize { get; private set; }
        public Cell[,] Cells { get; private set; }

        public Grid(int width, int height, float cellSize = 0.5f)
        {
            Width = width;
            Height = height;
            CellSize = cellSize;
            Cells = CreateCells(width, height);
        }

        public Grid(int width, int height, Cell[,] cells, float cellSize = 0.5f)
        {
            Width = width;
            Height = height;
            CellSize = cellSize;
            Cells = cells;
        }

        public Cell GetCellFromWorldPos(Vector3 worldPos)
        {
            float percentX = worldPos.x / (Width * CellSize);
            float percentY = worldPos.z / (Height * CellSize);

            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            int x = Mathf.Clamp(Mathf.FloorToInt((Width) * percentX), 0, Width - 1);
            int y = Mathf.Clamp(Mathf.FloorToInt((Height) * percentY), 0, Height - 1);
            return Cells[x, y];
        }

        private Cell[,] CreateCells(int width, int height)
        {
            Cell[,] cells = new Cell[width, height];
            Vector3 currentTilePos = new Vector3(CellSize / 2, 0, CellSize / 2);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var gridPos = new Vector2Int(i, j);
                    cells[i, j] = new Cell(currentTilePos, gridPos);
                    currentTilePos.z += CellSize;
                }
                currentTilePos = new Vector3(currentTilePos.x + CellSize, 0, CellSize / 2);
            }

            return cells;
        }
    }
}