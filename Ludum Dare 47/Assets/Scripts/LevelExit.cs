using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("Load level. Curr - " + currentLevel + " total: " + SceneManager.sceneCountInBuildSettings);
        if (currentLevel + 1 >= SceneManager.sceneCountInBuildSettings)
            return;
        Debug.Log("Loaded!");
        SceneManager.LoadScene(currentLevel + 1);
    }
}
