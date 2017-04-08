using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public BoardManager boardManager;
    public int playerFoodPoints = 100;
    [HideInInspector] public bool playersTurn = true;

    private int level = 3;

    public void gameOver()
    {
        enabled = false;
    }

    void Awake ()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        boardManager = GetComponent <BoardManager>();
        InitGame ();
    }

    void InitGame ()
    {
        boardManager.SetupScene (level);
    }

   
}
