using UnityEngine;

namespace Grid
{
    public class Cell
    {
        private const int DEFAULT_FIELD_COST = 1;
        public Vector3 WorldPos { get; private set; }
        public Vector2Int GridPos { get; private set; }
        public byte Cost { get; set; }
        public ushort BestCost { get; set; }
        public GridDirection BestDirection { get; set; }

        public Cell(Vector3 worldPos, Vector2Int gridPos)
        {
            WorldPos = worldPos;
            GridPos = gridPos;
            Cost = DEFAULT_FIELD_COST;
            BestCost = ushort.MaxValue;
        }

        public void IncreaseCost(int amount)
        {
            if (amount <= 0)
            {
                return;
            }
            if (Cost + amount < byte.MaxValue)
            {
                Cost += (byte)amount;
            }
            else if (Cost + amount >= byte.MaxValue)
            {
                Cost = byte.MaxValue;
            }
        }

        public void ResetCosts()
        {
            Cost = 1;
            BestCost = ushort.MaxValue;
        }
    }
}
