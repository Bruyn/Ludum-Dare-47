using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitchMng : MonoBehaviour
{

    public static PlayerSwitchMng Instance = null;

    private void Awake()
    {
        Debug.Log("A");
        if (Instance == null)
            return;

        Instance = this;
        Debug.Log("B");
    }

    void Start()
    {
        _players = GameObject.FindGameObjectsWithTag("Player");
        SelectPlayer(0);
    }

    private GameObject[] _players;
    private int _currentPlayerIndex = -1;

    public GameObject GetCurrentAuthority()
    {
        if (_currentPlayerIndex < 0)
            return null;

        return _players[_currentPlayerIndex];
    }
    
    void Update()
    {
        int newPlayerIndex = _currentPlayerIndex;
        
        if (Input.GetButtonDown("SwitchLeft"))
        {
            newPlayerIndex = _currentPlayerIndex - 1;
            if (newPlayerIndex < 0) newPlayerIndex = GetMaxPlayerIndex();
        }

        if (Input.GetButtonDown("SwitchRight"))
        {
            newPlayerIndex = _currentPlayerIndex + 1;
            if (newPlayerIndex > GetMaxPlayerIndex()) newPlayerIndex = 0;
        }
        
        SelectPlayer(newPlayerIndex);
    }

    int GetMaxPlayerIndex()
    {
        return _players.Length - 1;
    }
    
    void SelectPlayer(int playerIndex)
    {
        if (_currentPlayerIndex == playerIndex)
        {
            return;
        }

        foreach (var player in _players)
        {
            player.GetComponent<Authority>().Enabled = false;
        }

        _players[playerIndex].GetComponent<Authority>().Enabled = true;
        _currentPlayerIndex = playerIndex;
    }
}