using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewScreen : MonoBehaviour
{

    [SerializeField] private int _sceneIndex;

    //need the scene number of the game
    void StartGame()
    {
        SceneManager.LoadScene(_sceneIndex);
    }
}
