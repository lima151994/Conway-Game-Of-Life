using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileGrid : MonoBehaviour
{
    [SerializeField]
    private Vector2Int size;

    [SerializeField]
    private Tile[,] _tiles;

    [SerializeField] private Tile prefab;

    [SerializeField] private float updateInterval;
    private float _lastTimeUpdate;

    [SerializeField] private bool play;
    [SerializeField] private int startActive;

    public void Awake()
    {
        _tiles = new Tile[size.x, size.y];

        Vector3 startPosition = new Vector3(-size.x * prefab.transform.localScale.x*1.1f / 2f + prefab.transform.localScale.x*1.1f/2f,
            -size.y * prefab.transform.localScale.y *1.1f/ 2f+ prefab.transform.localScale.y*1.1f/2f, 0);

        for (int i = 0; i < _tiles.GetLength(0); i++)
        {
            for (int j = 0; j < _tiles.GetLength(1); j++)
            {
                _tiles[i, j] = Instantiate(prefab);
                _tiles[i, j].name = "tile " + i + " " + j;
                _tiles[i, j].transform.position = startPosition + new Vector3(i * (prefab.transform.localScale.x * 1.1f),
                    j * (prefab.transform.localScale.y * 1.1f), 0);
            }
        }
        
        for (int i = 0; i < _tiles.GetLength(0); i++)
        {
            for (int j = 0; j < _tiles.GetLength(1); j++)
            {
                GenerateNeighbour(i,j);
            }
        }


        if (startActive > _tiles.Length)
        {
            return;
        }
        
        int counter = 0;

        while (counter<startActive)
        {
            int randomX;
            int randomY;
            Tile selectedTile;

            do
            {
                randomX = Random.Range(0, _tiles.GetLength(0));
                randomY = Random.Range(0, _tiles.GetLength(1));
                selectedTile = _tiles[randomX, randomY];
            } while (selectedTile.IsLife);
            
            selectedTile.Enable();
            counter++;
        }
    }

    private void Update()
    {
        if (!play)
        {
            return;
        }

        if (!(Time.time >= _lastTimeUpdate + updateInterval)) return;
        UpdateTile();
        _lastTimeUpdate = Time.time;
    }

    [ContextMenu("update")]
    public void UpdateTile()
    {
        UpdateTileNeighbour();
        UpdateTileLife();
    }

    private void UpdateTileLife()
    {
        for (int i = 0; i < _tiles.GetLength(0); i++)
        {
            for (int j = 0; j < _tiles.GetLength(1); j++)
            {
                _tiles[i, j].UpdateLife();
            }
        }
    }

    private void UpdateTileNeighbour()
    {
        for (int i = 0; i < _tiles.GetLength(0); i++)
        {
            for (int j = 0; j < _tiles.GetLength(1); j++)
            {
                _tiles[i, j].UpdateNeighbour();
            }
        }
    }

    private void GenerateNeighbour(int i, int j)
    {
        for (int k = 0; k < 3; k++)
        {
            int newI = (i - 1 + k);

            if (newI < 0)
            {
                newI += size.x;
            }else if (newI >= size.x)
            {
                newI -= size.x;
            }

            for (int l = 0; l < 3; l++)
            {
                int newJ = (j - 1 + l);
                
                if (newJ < 0)
                {
                    newJ += size.y;
                }else if (newJ >= size.y)
                {
                    newJ -= size.y;
                }

                if (newI == i && newJ == j)
                {
                    continue;
                }
                
                _tiles[i,j].RegisterNeighbour(_tiles[newI, newJ]);
            }
        }
    }
}