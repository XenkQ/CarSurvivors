using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GridController : MonoBehaviour
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private int _cellSize;
    [SerializeField] private float _delayBetweenFlowFieldRecreaction = 0.2f;
    [HideInInspector] public UnityEvent OnGridUpdate;

    public Grid Grid { get; private set; }

    private FlowField _flowField;

    public static GridController Instance { get; private set; }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
            Instance = this;
        }

        Grid = new Grid(_width, _height, _cellSize);
        _flowField = new FlowField(Grid);
        _flowField.CreateCostField();
    }

    public void Start()
    {
        StartCoroutine(RecreateIntegrationFieldReapeater());
    }

    private IEnumerator RecreateIntegrationFieldReapeater()
    {
        while (true)
        {
            RecreateIntegrationFieldBasedOnPlayerTilePos();
            OnGridUpdate.Invoke();
            yield return new WaitForSeconds(_delayBetweenFlowFieldRecreaction);
        }
    }

    private void RecreateIntegrationFieldBasedOnPlayerTilePos()
    {
        _flowField.CreateCostField();
        Cell cellClosestToPlayer = Grid.GetCellFromWorldPos(Player.Instance.transform.position);
        _flowField.CreateIntegrationField(cellClosestToPlayer);
        _flowField.CreateFlowField();
    }
}