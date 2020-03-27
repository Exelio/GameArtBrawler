﻿using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Range(1, 2)] [SerializeField] private int _playerAmount = 2;
    public int PlayerAmount { get => _playerAmount; set { _playerAmount = value; } }

    private int _characterAmount;
    public int CharacterAmount { get => _characterAmount; set { _characterAmount = value; } }

    private Dictionary<int, GameObject> _playerCharacter;
    public Dictionary<int, GameObject> PlayerCharacter { get => _playerCharacter; set { _playerCharacter = value; } }

    private bool _isGamePlaying = true;
    public bool IsGamePlaying { get => _isGamePlaying; set { _isGamePlaying = value; } }

    private void Awake()
    {
        CheckDontDestroyOnLoad();

        Setup();
    }

    private void CheckDontDestroyOnLoad()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("GameController");
        if (gos.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Setup()
    {
        _playerCharacter = new Dictionary<int, GameObject>();

        for (int i = 1; i <= _playerAmount; i++)
        {
            _playerCharacter.Add(i, null);
        }
    }    
}
