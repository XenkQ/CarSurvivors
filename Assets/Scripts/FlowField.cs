using UnityEngine;

public class FlowField : MonoBehaviour
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private int _cellSize;

    public void Awake()
    {
        var grid = new Grid(_width, _height, _cellSize);
    }
}