using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MovingObject
{
    public int wallDmg = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;

    private Animator animator;
    private int food;

    public void LoseFood (int loss)
    {
        animator.SetTrigger("playerHit");
        food -= loss;
        CheckIfGameOver();
    }
   
    // Use this for initialization
    protected override void Start ()
    {
        animator = GetComponent <Animator>();
       
        food = GameManager.instance.playerFoodPoints;

        base.Start ();
	}

    private void Update()
    {
        if (!GameManager.instance.playersTurn)
        {
            return;
        }

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int) Input.GetAxis("Horizontal");
        vertical = (int) Input.GetAxis("Vertical");

        // Strictly move as either horizontal or vertical, but not diagonal.
        if (horizontal != 0)
        {
            vertical = 0;
        }

        if (horizontal != 0 || vertical != 0)
        {
            AttemptMove<Wall>(horizontal, vertical);
        }
    }

    protected override void AttemptMove <T>(int xDir, int yDir)
    {
        base.AttemptMove <T>(xDir, yDir);
        GameManager.instance.playersTurn = false;

        food--;
        CheckIfGameOver();
    }

    protected override void OnCantMove<T>(T component)
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDmg);
        animator.SetTrigger("playerChop");
    }

    // Call when player reaches exit in order to move to the next level.
    private void Restart ()
    {
        // Application.LoadLevel(Application.loadedLevel); deprecated
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnDisable ()
    {
        GameManager.instance.playerFoodPoints = food;
    } 

    private void CheckIfGameOver ()
    {
        if (food <= 0)
        {
            GameManager.instance.gameOver ();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (collision.tag == "Food")
        {
            food += pointsPerFood;
            collision.gameObject.SetActive(false);
        }
        else if (collision.tag == "Soda")
        {
            food += pointsPerSoda;
            collision.gameObject.SetActive(false);
        }
    }
}
