using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changeLvl : MonoBehaviour
{
    public string levelName;
    //private static sideChecker sideChecker;

        void OnCollisionEnter2D(Collision2D collision)        
        {
            if(collision.gameObject.tag == "Player")
            {
                SceneManager.LoadScene(levelName);
                //sideChecker.leftChecker = true;
            }
        }

        /*void Start()
        {
            sideChecker = FindObjectOfType<sideChecker>();
        }*/
}
