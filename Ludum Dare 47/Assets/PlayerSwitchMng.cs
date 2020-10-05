using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitchMng : MonoBehaviour
{
    private void OnValidate()
    {
        if (players.Count == 0)
            players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
    }

    void Start()
    {
        SelectPlayer(0);
    }

    [SerializeField] private List<GameObject> players = new List<GameObject>();
    private int _currentPlayerIndex = -1;

    public GameObject GetCurrentAuthority()
    {
        if (_currentPlayerIndex < 0)
            return null;

        return players[_currentPlayerIndex];
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
        return players.Count - 1;
    }
    
    void SelectPlayer(int playerIndex)
    {
        if (_currentPlayerIndex == playerIndex)
        {
            return;
        }

        foreach (var player in players)
        {
            player.GetComponent<Authority>().Enabled = false;
        }

        players[playerIndex].GetComponent<Authority>().Enabled = true;
        _currentPlayerIndex = playerIndex;
    }
}