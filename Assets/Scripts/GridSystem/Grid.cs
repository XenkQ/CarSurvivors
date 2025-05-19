using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Assets.Scripts.GridSystem
{
    [Serializable]
    public class GridConfiguration
    {
        public int Width;
        public int Height;
        public float CellSize;

        public GridConfiguration(int width, int height, float cellSize)
        {
            Width = width;
            Height = height;
            CellSize = cellSize;
        }
    }

    public class Grid
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public float CellSize { get; private set; }
        public Cell[,] Cells { get; private set; }

        public Grid(GridConfiguration gridConfiguration)
        {
            Width = gridConfiguration.Width;
            Height = gridConfiguration.Height;
            CellSize = gridConfiguration.CellSize;
            Cells = CreateCells(Width, Height);
        }

        public Grid(GridConfiguration gridConfiguration, Cell[,] existingCells)
        {
            Width = gridConfiguration.Width;
            Height = gridConfiguration.Height;
            CellSize = gridConfiguration.CellSize;
            Cells = existingCells;
        }

        public Cell GetCellFromWorldPos(Vector3 worldPos)
        {
            float percentX = worldPos.x / (Width * CellSize);
            float percentY = worldPos.z / (Height * CellSize);

            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            int x = Mathf.Clamp(Mathf.FloorToInt(Width * percentX), 0, Width - 1);
            int y = Mathf.Clamp(Mathf.FloorToInt(Height * percentY), 0, Height - 1);

            return Cells[x, y];
        }

        public Cell GetRandomWalkableEdgeCell()
        {
            List<Cell> edgeCells = GridManager.Instance.GridPlayerChunk.GetWalkableEdgeCells();
            int randomIndex = UnityEngine.Random.Range(0, edgeCells.Count);

            return edgeCells[randomIndex];
        }

        public Cell GetRandomWalkableNotOccupiedByCollectibleCell()
        {
            Cell[] notOccupiedCells = Cells.Cast<Cell>().Where(c => !c.IsOccupiedByCollectible && IsCellWalkable(c)).ToArray();
            int randomIndex = UnityEngine.Random.Range(0, notOccupiedCells.Length);

            return notOccupiedCells[randomIndex];
        }

        public List<Cell> GetWalkableEdgeCells()
        {
            List<Cell> edgeCells = new List<Cell>();

            int horizontalCellsCount = Cells.GetLength(0);
            int verticalCellsCount = Cells.GetLength(1);
            for (int x = 0; x < horizontalCellsCount; x++)
            {
                if (IsCellWalkable(Cells[x, 0])) { edgeCells.Add(Cells[x, 0]); }
                if (IsCellWalkable(Cells[x, verticalCellsCount - 1])) { edgeCells.Add(Cells[x, verticalCellsCount - 1]); }
            }

            for (int y = 0; y < verticalCellsCount; y++)
            {
                if (IsCellWalkable(Cells[0, y])) { edgeCells.Add(Cells[0, y]); }
                if (IsCellWalkable(Cells[horizontalCellsCount - 1, y])) { edgeCells.Add(Cells[horizontalCellsCount - 1, y]); }
            }

            return edgeCells;
        }

        private bool IsCellWalkable(Cell cell) =>
            cell.Cost < byte.MaxValue;

        private Cell[,] CreateCells(int width, int height)
        {
            Cell[,] cells = new Cell[width, height];
            Vector3 currentTilePos = new Vector3(CellSize * 0.5f, 0, CellSize * 0.5f);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var gridPos = new Vector2Int(i, j);
                    cells[i, j] = new Cell(currentTilePos, gridPos, gridPos);
                    currentTilePos.z += CellSize;
                }
                currentTilePos = new Vector3(currentTilePos.x + CellSize, 0, CellSize * 0.5f);
            }

            return cells;
        }
    }
}
