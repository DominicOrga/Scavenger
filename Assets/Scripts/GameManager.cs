using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public float levelStartDelay = 2f;
    public float turnDelay = 0.1f;
    public BoardManager boardManager;
    public int playerFoodPoints = 100;
    [HideInInspector] public bool playersTurn = true;

    private Text levelText;
    private GameObject levelImage;
    private int level = 0;
    private List<Enemy> enemies;
    private bool enemiesMoving;
    private bool doingSetup;

    public void AddEnemiesToList(Enemy script)
    {
        enemies.Add(script);
    }

    public void GameOver()
    {
        levelText.text = "After " + level + " days, you starved.";
        levelImage.SetActive(true);
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

        enemies = new List<Enemy>();
        boardManager = GetComponent <BoardManager>();
    }

    void InitGame ()
    {
        Debug.Log("Game initialized");

        doingSetup = true;

        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day " + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", 2f);

        enemies.Clear();
        boardManager.SetupScene(level);
    }

    void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);

        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(turnDelay);
        }

        playersTurn = true;
        enemiesMoving = false;
    }

    void Update()
    {
        if (playersTurn || enemiesMoving || doingSetup)
        {
            return;
        }

        StartCoroutine(MoveEnemies());
    }

    void OnFinishedLoadingLevel(Scene scene, LoadSceneMode mode)
    {
        level++;
        InitGame();     // Reinitialize level.
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnFinishedLoadingLevel;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnFinishedLoadingLevel;
    }
}
