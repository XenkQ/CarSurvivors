using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TilesManager))]
public class TilesManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var tilesManager = target as TilesManager;
        if (tilesManager != null)
        {
            if (GUILayout.Button("Generate Tiles") && Application.isPlaying)
            {
                tilesManager.GenerateTiles();
            }
        }
    }
}

public class TilesManager : MonoBehaviour
{
    [Serializable]
    private struct Tile
    {
        public GameObject tilePrefab;
        public float spawnChance;
    }

    private enum Direction
    {
        Up, Down, Left, Right
    }

    [SerializeField] private Vector2 _tilesGenerationSpaceSize;
    [SerializeField] private int _minTileChainLength = 1;
    [SerializeField] private int _maxTileChainLength = 3;

    //Maybe new tile class instead of struct?
    [SerializeField] private Vector2 _tileSize = new(10f, 10f);

    [SerializeField] private Transform _tilesParent;
    [SerializeField] private Tile[] _tilesThatCanBeSpawned;

    private void Awake()
    {
        GenerateTiles();
    }

    public void GenerateTiles()
    {
        ClearTilesParent();

        Stack<Transform> tilesToExpand = new();
        HashSet<Vector3> usedPositions = new();
        int directionsCount = Enum.GetValues(typeof(Direction)).Length;

        Transform startTile = Instantiate(_tilesThatCanBeSpawned[0].tilePrefab, Vector3.zero, Quaternion.identity, _tilesParent).transform;
        tilesToExpand.Push(startTile);
        usedPositions.Add(startTile.position);
        while (tilesToExpand.Count > 0)
        {
            Transform tile = tilesToExpand.Pop();
            int tileChainLength = UnityEngine.Random.Range(_minTileChainLength, _maxTileChainLength + 1);
            Transform lastTileInChain = null;

            HashSet<Direction> excludedDirections = new();
            Direction tileDirection = GetRandomDirection(excludedDirections);

            int i = 0;
            while (i < tileChainLength)
            {
                float x = tile.position.x;
                float z = tile.position.z;
                Vector3 spawningPos = tileDirection switch
                {
                    Direction.Left => new Vector3(x + _tileSize.x * 2, 0, z),
                    Direction.Right => new Vector3(x - _tileSize.x * 2, 0, z),
                    Direction.Up => new Vector3(x, 0, z + _tileSize.y * 2),
                    Direction.Down => new Vector3(x, 0, z - _tileSize.y * 2),
                    _ => throw new ArgumentOutOfRangeException(),
                };

                if (Mathf.Abs(spawningPos.x) > _tilesGenerationSpaceSize.x / 2
                    || Mathf.Abs(spawningPos.z) > _tilesGenerationSpaceSize.y / 2
                    || usedPositions.Contains(spawningPos))
                {
                    excludedDirections.Add(tileDirection);
                    if (excludedDirections.Count >= directionsCount)
                    {
                        break;
                    }
                    else
                    {
                        tileDirection = GetRandomDirection(excludedDirections);
                    }
                }
                else
                {
                    GameObject tilePrefab = GetRandomTileBySpawnChance().tilePrefab;
                    Quaternion tileRotation = GetTileRotationRelatedToSpawningDirection();
                    Transform newTile = Instantiate(tilePrefab, spawningPos, tileRotation, _tilesParent).transform;
                    usedPositions.Add(newTile.position);
                    lastTileInChain = newTile;
                    excludedDirections.Clear();
                    i++;
                }
            }

            if (lastTileInChain != null)
            {
                tilesToExpand.Push(lastTileInChain);
            }
        }
    }

    private void ClearTilesParent()
    {
        if (_tilesParent.childCount > 0)
        {
            foreach (Transform tile in _tilesParent)
            {
                Destroy(tile.gameObject);
            }
        }
    }

    private Direction GetRandomDirection(IEnumerable<Direction> excludedDirections)
    {
        var directions = Enum.GetValues(typeof(Direction))
            .Cast<Direction>()
            .Where(direction => !excludedDirections.Contains(direction))
            .ToList();
        return directions[UnityEngine.Random.Range(0, directions.Count)];
    }

    private Tile GetRandomTileBySpawnChance()
    {
        return _tilesThatCanBeSpawned[0];
    }

    private Quaternion GetTileRotationRelatedToSpawningDirection()
    {
        return Quaternion.identity;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Vector2.zero, new Vector3(_tilesGenerationSpaceSize.x, 0, _tilesGenerationSpaceSize.y));
    }
}