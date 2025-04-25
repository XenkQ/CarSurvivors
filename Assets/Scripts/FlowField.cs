using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlowField
{
    private readonly Grid _grid;
    private Cell _destinationCell;

    private const int IMPASSABLE_COST = 255;
    private const int ROUGH_TERRAIN_COST = 3;
    private const int DEFAULT_FIELD_COST = 1;

    private readonly LayerMask _impassableTerrain = LayerMask.GetMask("Impassable");
    private readonly LayerMask _roughTerrain = LayerMask.GetMask("RoughTerrain");
    private readonly LayerMask _ground = LayerMask.GetMask("Ground");

    public FlowField(Grid grid)
    {
        _grid = grid;
    }

    public void CreateCostField()
    {
        for (int i = 0; i < _grid.Cells.GetLength(0); i++)
        {
            for (int j = 0; j < _grid.Cells.GetLength(1); j++)
            {
                Cell cell = _grid.Cells[i, j];
                Vector3 halfExtents = Vector3.one * (_grid.CellSize / 2);
                Collider[] obstacles = Physics.OverlapBox(cell.WorldPos,
                                                          halfExtents,
                                                          Quaternion.identity,
                                                          _impassableTerrain | _roughTerrain | _ground);
                int maxCost = DEFAULT_FIELD_COST;
                if (obstacles.Length > 0)
                {
                    foreach (Collider obstacle in obstacles)
                    {
                        int layerValue = 1 << obstacle.gameObject.layer;
                        if (maxCost < IMPASSABLE_COST && (layerValue & _impassableTerrain.value) == _impassableTerrain.value)
                        {
                            maxCost = IMPASSABLE_COST;
                        }
                        else if (maxCost < ROUGH_TERRAIN_COST && (layerValue & _roughTerrain.value) == _roughTerrain.value)
                        {
                            maxCost = ROUGH_TERRAIN_COST;
                        }
                    }

                    if (maxCost > DEFAULT_FIELD_COST)
                    {
                        cell.IncreaseCost(maxCost);
                    }
                }
                else
                {
                    cell.IncreaseCost(IMPASSABLE_COST);
                }
            }
        }
    }

    public void CreateIntegrationField(Cell destinationCell)
    {
        _destinationCell = destinationCell;

        _destinationCell.Cost = 0;
        _destinationCell.BestCost = 0;

        Queue<Cell> cellsToCheck = new Queue<Cell>();
        cellsToCheck.Enqueue(destinationCell);
        while (cellsToCheck.Count > 0)
        {
            Cell currentCell = cellsToCheck.Dequeue();
            List<Cell> currentNeighbours = GetNeighbourCells(currentCell, GridDirection.CardinalDirections);
            foreach (Cell currentNeighbour in currentNeighbours)
            {
                if (currentNeighbour.Cost == byte.MaxValue)
                {
                    continue;
                }
                else if (currentNeighbour.Cost + currentCell.BestCost < currentNeighbour.BestCost)
                {
                    currentNeighbour.BestCost = (ushort)(currentNeighbour.Cost + currentCell.BestCost);
                    cellsToCheck.Enqueue(currentNeighbour);
                }
            }
        }
    }

    private List<Cell> GetNeighbourCells(Cell currentCell, IEnumerable<GridDirection> gridDirections)
    {
        List<Cell> cells = new List<Cell>();
        Vector2Int gridPos = currentCell.GridPos;
        foreach (GridDirection gridDirection in gridDirections)
        {
            Vector2Int positionToCheck = gridPos + gridDirection.Vector;
            bool isCellOnPositionExistingInGrid = positionToCheck.x >= 0 && positionToCheck.y >= 0 &&
                                                  positionToCheck.x < _grid.Cells.GetLength(0) && positionToCheck.y < _grid.Cells.GetLength(1);
            if (isCellOnPositionExistingInGrid)
            {
                cells.Add(_grid.Cells[positionToCheck.x, positionToCheck.y]);
            }
        }
        return cells;
    }
}