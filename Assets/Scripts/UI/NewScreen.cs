using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewScreen : MonoBehaviour
{
    //need the scene number of the game
    void StartGame()
    {
        SceneManager.LoadScene(0);
    }
}
