using UnityEngine;

public class Cell
{
    public Vector3 position;
    public Vector2Int gridPos;

    public Cell(Vector3 position, Vector2Int gridPos)
    {
        this.position = position;
        this.gridPos = gridPos;
    }
}