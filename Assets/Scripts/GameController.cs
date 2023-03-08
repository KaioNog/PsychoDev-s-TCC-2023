using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  //biblioteca canvas
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public GameObject gameOver;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ShowGameOver()
    {
        gameOver.SetActive(true);
    }
}
