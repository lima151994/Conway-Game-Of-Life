using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    
    private bool _isLife;
    private int _neighbourLifeCounter;

    private List<Tile> _neighbour = new();

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = _isLife ? Color.white : Color.black;
    }

    [ContextMenu("test")]
    public void Test()
    {
        IsLife = !IsLife;
    }

    public void Enable()
    {
        IsLife = true;
    }

    public bool IsLife
    {
        get => _isLife;
        private set
        {
            _isLife = value;
            _spriteRenderer.color = _isLife ? Color.white : Color.black;
        }
    }

    public void UpdateLife()
    {
        if (_neighbourLifeCounter <= 1 || _neighbourLifeCounter >= 4)
        {
            IsLife = false;
        }
        else
        {
            if (IsLife) return;
            if (_neighbourLifeCounter == 3)
            {
                IsLife = true;
            }
        }
    }

    public void UpdateNeighbour()
    {
        _neighbourLifeCounter = _neighbour.Count(tile => tile.IsLife);
    }

    public void RegisterNeighbour(Tile tile)
    {
        _neighbour.Add(tile);
    }
}