using Grid.FlowField;
using UnityEngine;
using UnityEngine.Events;

namespace Grid
{
    public sealed class GridController : MonoBehaviour
    {
        [Header("WorldGrid")]
        [SerializeField] private int _gridWidth;
        [SerializeField] private int _gridHeight;
        [SerializeField] private int _gridCellSize;
        [SerializeField] private float _delayBetweenWorldGridUpdate = 0.2f;
        public Grid WorldGrid { get; private set; }

        [Header("PlayerChunkGrid")]
        [SerializeField] private int _playerChunkWidth;
        [SerializeField] private int _playerChunkHeight;
        [SerializeField] private float _delayBetweenPlayerChunkGridUpdate = 0.32f;
        public Grid PlayerChunkGrid { get; private set; }

#if DEBUG

        [Header("Debug colors")]
        [SerializeField][ColorUsage(false)] private Color _worldCellBorderColor = Color.blue;
        [SerializeField][ColorUsage(false)] private Color _playerChunkCellBorderColor = Color.green;
        [SerializeField][ColorUsage(false)] private Color _blockedCellBorderDrawColor = Color.red;

        [Header("FlowField Debug")]
        [SerializeField] private FlowFieldDebugConfiguration _flowFieldDebugConfiguration;

        private float _playerChunkDrawYOffset = 0.2f;
#endif

        [HideInInspector] public UnityEvent OnWorldGridUpdate;

        private FlowField.FlowField _flowField;

        public static GridController Instance { get; private set; }

        private GridController()
        { }

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

            WorldGrid = new Grid(_gridWidth, _gridHeight, _gridCellSize);
            _flowField = new FlowField.FlowField(WorldGrid);
            _flowFieldDebugConfiguration.grid = WorldGrid;
            UpdateWorldGrid();
        }

        public void Start()
        {
            InvokeRepeating(nameof(UpdateWorldGrid), _delayBetweenWorldGridUpdate, _delayBetweenWorldGridUpdate);
            InvokeRepeating(nameof(UpdatePlayerChunkGrid), _delayBetweenWorldGridUpdate, _delayBetweenPlayerChunkGridUpdate);
        }

        private void UpdatePlayerChunkGrid()
        {
            UpdateGridPlayerChunk();
#if DEBUG
            GridDebug.DisplayGrid(PlayerChunkGrid,
                                  _playerChunkCellBorderColor,
                                  _blockedCellBorderDrawColor,
                                  _playerChunkDrawYOffset,
                                  _delayBetweenPlayerChunkGridUpdate);
#endif
        }

        private void UpdateWorldGrid()
        {
            UpdateGridPlayerChunk();
            UpdateFlowFieldBasedOnPlayerGridPos();

#if DEBUG
            GridDebug.DisplayGrid(WorldGrid, _worldCellBorderColor, _blockedCellBorderDrawColor, 0, _delayBetweenWorldGridUpdate);
            FlowFieldDebug.DisplayFlowFieldDebugTextOnGrid(_flowFieldDebugConfiguration);
#endif

            OnWorldGridUpdate.Invoke();
        }

        private void UpdateGridPlayerChunk()
        {
            Cell[,] chunkCells = new Cell[_playerChunkWidth, _playerChunkHeight];
            Cell cellClosestToPlayer = WorldGrid.GetCellFromWorldPos(Player.Instance.transform.position);
            int maxGridX = cellClosestToPlayer.GridPos.x + _playerChunkWidth / 2;
            int minGridX = cellClosestToPlayer.GridPos.x - _playerChunkWidth / 2;
            int maxGridY = cellClosestToPlayer.GridPos.y + _playerChunkHeight / 2;
            int minGridY = cellClosestToPlayer.GridPos.y - _playerChunkHeight / 2;

            int x = minGridX;
            int chunkX = 0;
            while (x <= maxGridX && x < WorldGrid.Cells.GetLength(0))
            {
                if (x >= 0 && chunkX < chunkCells.GetLength(0))
                {
                    int y = minGridY;
                    int chunkY = 0;
                    while (y <= maxGridY && y < WorldGrid.Cells.GetLength(1))
                    {
                        if (y >= 0 && chunkY < chunkCells.GetLength(1))
                        {
                            chunkCells[chunkX, chunkY] = WorldGrid.Cells[x, y];
                            chunkY++;
                        }
                        y++;
                    }

                    chunkX++;
                }
                x++;
            }

            PlayerChunkGrid = new Grid(_playerChunkWidth, _playerChunkHeight, chunkCells, _gridCellSize);
        }

        private void UpdateFlowFieldBasedOnPlayerGridPos()
        {
            _flowField.CreateCostField();
            Cell cellClosestToPlayer = WorldGrid.GetCellFromWorldPos(Player.Instance.transform.position);
            _flowField.CreateIntegrationField(cellClosestToPlayer);
            _flowField.CreateFlowField();
        }
    }
}
