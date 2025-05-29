using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.GridSystem
{
    public static class RandomGridCellWithConditionFinder
    {
        public static Cell FindRandomWalkableCellWithoutCollectibleOrNull(Grid grid)
        {
            Cell[] notOccupiedCells = grid
                .Cells
                .Cast<Cell>()
                .Where(c => !c.IsOccupiedByCollectible && CellStatusDescriber.IsWalkable(c))
                .ToArray();

            if (notOccupiedCells.Length == 0)
            {
                return null;
            }

            int randomIndex = UnityEngine.Random.Range(0, notOccupiedCells.Length);

            return notOccupiedCells[randomIndex];
        }

        public static Cell GetRandomWalkableEdgeCellOrNull(Grid grid)
        {
            List<Cell> edgeCells = GridEdgeHelper.GetWalkableEdgeCells(grid);

            if (edgeCells.Count == 0)
            {
                return null;
            }

            int randomIndex = UnityEngine.Random.Range(0, edgeCells.Count);

            return edgeCells[randomIndex];
        }
    }
}
