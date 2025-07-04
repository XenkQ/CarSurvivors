﻿using UnityEngine;
using Assets.Scripts.Player;
using Assets.Scripts.FlowFieldSystem;

namespace Assets.Scripts.GridSystem
{
    public sealed class GridManager : MonoBehaviour
    {
        [SerializeField] private GridConfiguration _worldGridConfiguration;
        [SerializeField] private float _delayBetweenWorldGridUpdate = 0.2f;
        public Grid WorldGrid { get; private set; }

        [SerializeField] private GridConfiguration _playerGridConfiguration;
        [SerializeField] private float _delayBetweenPlayerChunkGridUpdate = 0.32f;
        public Grid GridPlayerChunk { get; private set; }

#if DEBUG
        [SerializeField] private bool _debugGrid;
        [SerializeField] private bool _debugFlowField;

        [SerializeField][ColorUsage(false)] private Color _worldCellBorderColor = Color.blue;
        [SerializeField][ColorUsage(false)] private Color _playerChunkCellBorderColor = Color.green;
        [SerializeField][ColorUsage(false)] private Color _blockedCellBorderDrawColor = Color.red;

        [SerializeField] private FlowFieldDebugConfiguration _flowFieldDebugConfiguration;

        private float _playerChunkDrawYOffset = 0.2f;
#endif
        private FlowField _flowField;

        public static GridManager Instance { get; private set; }

        private GridManager()
        { }

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            WorldGrid = new Grid(_worldGridConfiguration);
            _flowField = new FlowField();
            _flowFieldDebugConfiguration.Grid = WorldGrid;
            UpdateFlowFieldBasedOnPlayerInWorldGridPos(WorldGrid);
            UpdatePlayerChunkGrid();

            //Calling debug world gridPerformingUpdate to prevent lag caused by creating a lot of tmpro objects
#if DEBUG
            DebugWorldGrid();
#endif
        }

        public void Start()
        {
            InvokeRepeating(nameof(UpdatePlayerChunkGrid), _delayBetweenWorldGridUpdate, _delayBetweenPlayerChunkGridUpdate);
#if DEBUG
            if (_debugGrid)
            {
                float drawTimeOffset = 0.02f;
                InvokeRepeating(nameof(DebugWorldGrid), _delayBetweenWorldGridUpdate, _delayBetweenWorldGridUpdate + drawTimeOffset);
                InvokeRepeating(nameof(DebugPlayerChunkGrid), _delayBetweenPlayerChunkGridUpdate, _delayBetweenPlayerChunkGridUpdate + drawTimeOffset);
            }
#endif
        }

        private void UpdatePlayerChunkGrid()
        {
            GridPlayerChunk = CreatePlayerChunkBasedOnPlayerPositionInWorldGrid();
            UpdateFlowFieldBasedOnPlayerInWorldGridPos(GridPlayerChunk);
        }

        private void DebugPlayerChunkGrid()
        {
            GridDebug.DisplayGrid(GridPlayerChunk,
                                  _playerChunkCellBorderColor,
                                  _blockedCellBorderDrawColor,
                                  _playerChunkDrawYOffset,
                                  _delayBetweenPlayerChunkGridUpdate);
        }

        private void DebugWorldGrid()
        {
            if (_debugGrid)
            {
                GridDebug.DisplayGrid(WorldGrid, _worldCellBorderColor, _blockedCellBorderDrawColor, 0, _delayBetweenWorldGridUpdate);
            }
            if (_debugFlowField)
            {
                FlowFieldDebug.DisplayFlowFieldDebugTextOnGrid(_flowFieldDebugConfiguration);
            }
        }

        private Grid CreatePlayerChunkBasedOnPlayerPositionInWorldGrid()
        {
            int chunkWidth = _playerGridConfiguration.Width;
            int chunkHeight = _playerGridConfiguration.Height;
            Cell[,] chunkCells = new Cell[chunkWidth, chunkHeight];
            Cell cellClosestToPlayer = WorldPosToCellConverter.GetCellFromGridByWorldPos(WorldGrid,
                (PlayerManager.Instance.transform.position));

            int halfWidth = chunkWidth >> 1;
            int maxGridX = cellClosestToPlayer.WorldGridPos.x + halfWidth;
            int minGridX = cellClosestToPlayer.WorldGridPos.x - halfWidth;

            int halfHeight = chunkHeight >> 1;
            int maxGridY = cellClosestToPlayer.WorldGridPos.y + halfHeight;
            int minGridY = cellClosestToPlayer.WorldGridPos.y - halfHeight;

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
                            chunkCells[chunkX, chunkY].ChunkGridPos = new Vector2Int(chunkX, chunkY);
                            chunkY++;
                        }
                        y++;
                    }

                    chunkX++;
                }
                x++;
            }

            return new Grid(_playerGridConfiguration, chunkCells);
        }

        private void UpdateFlowFieldBasedOnPlayerInWorldGridPos(Grid gridPerformingUpdate)
        {
            _flowField.CreateCostField(gridPerformingUpdate);
            Cell cellClosestToPlayer = WorldPosToCellConverter.GetCellFromGridByWorldPos(WorldGrid, PlayerManager.Instance.transform.position);
            _flowField.CreateIntegrationField(gridPerformingUpdate, cellClosestToPlayer);
            _flowField.CreateFlowField(gridPerformingUpdate);
        }
    }
}
