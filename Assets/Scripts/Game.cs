using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    //public TMP_Text win;
    //public TMP_Text lose;

    bool gameHasEnded = false;

    //public GameObject winUI;
    public GameObject gameOverUI;

    public void Awake()
    {
        //total = FindObjectsOfType<Enemy>().Length;
    }

    public void Update()
    {
        //scoreTxt.SetText($"{score} / {total}");
    }

    public void GameOver()
    {
        if (!gameHasEnded)
        {
            gameHasEnded = true;
            Debug.Log("Game Over");
            gameOverUI.SetActive(true);
            //Restart
            //Invoke("Restart", timer);
        }
    }

    //public void GameWin()
    //{
    //    if (!gameHasEnded)
    //    {
    //        gameHasEnded = true;
    //        Debug.Log("Game Win");
    //        winUI.SetActive(true);
    //        //Restart
    //        //Invoke("Restart", timer);
    //    }
    //}

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
