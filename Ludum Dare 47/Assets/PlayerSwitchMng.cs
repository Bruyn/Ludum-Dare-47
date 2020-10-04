using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitchMng : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        _players = GameObject.FindGameObjectsWithTag("Player");
        SelectPlayer(0);
    }

    private GameObject[] _players;
    private int _currentPlayerIndex = -1;

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