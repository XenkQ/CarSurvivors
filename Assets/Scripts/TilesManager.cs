using System;
using System.Collections.Generic;
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

    [SerializeField] private Vector2 _tilesGenerationSpaceSize;
    [SerializeField] private int _maxTileChainLength = 3;

    //Maybe new tile class instead of struct?
    [SerializeField] private Vector2 _tileSize = new Vector2(10f, 10f);

    [SerializeField] private Transform _tilesParent;
    [SerializeField] private Tile[] _tilesThatCanBeSpawned;

    private void Awake()
    {
        GenerateTiles();
    }

    public void GenerateTiles()
    {
        ClearTilesParent();

        HashSet<Vector2> usedPositions = new();
        Stack<Transform> tilesToExpand = new();
        Transform startTile = Instantiate(_tilesThatCanBeSpawned[0].tilePrefab, Vector3.zero, Quaternion.identity, _tilesParent).transform;
        tilesToExpand.Push(startTile);
        usedPositions.Add(startTile.position);
        while (tilesToExpand.Count > 0)
        {
            Transform tile = tilesToExpand.Pop();
            int tileChainLength = UnityEngine.Random.Range(1, _maxTileChainLength + 1);
            Vector2 tileDirection = GetRandomDirection();
            Transform lastTileInChain = null;
            for (int i = 0; i < tileChainLength; i++)
            {
                float x = tile.position.x;
                float y = tile.position.y;

                Vector2 spawningPos = tileDirection switch
                {
                    _ when tileDirection == Vector2.up => new Vector2(x, y + _tileSize.y * 2),
                    _ when tileDirection == Vector2.down => new Vector2(x, y - _tileSize.y * 2),
                    _ when tileDirection == Vector2.left => new Vector2(x + _tileSize.x * 2, y),
                    _ when tileDirection == Vector2.right => new Vector2(x - _tileSize.x * 2, y),
                    _ => throw new ArgumentOutOfRangeException(),
                };

                //TODO: Next attempts instead of break;
                if (spawningPos.magnitude > _tilesGenerationSpaceSize.magnitude
                    || usedPositions.Contains(spawningPos))
                {
                    break;
                }
                else
                {
                    GameObject tilePrefab = GetRandomTileBySpawnChance().tilePrefab;
                    Quaternion tileRotation = GetTileRotationRelatedToSpawningDirection();
                    Transform newTile = Instantiate(tilePrefab, new Vector3(spawningPos.x, 0, spawningPos.y), tileRotation, _tilesParent).transform;
                    usedPositions.Add(newTile.position);
                    lastTileInChain = newTile;
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

    private Vector2 GetRandomDirection()
    {
        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        return directions[UnityEngine.Random.Range(0, directions.Length)];
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