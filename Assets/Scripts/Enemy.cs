using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject
{
    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;

        if (Mathf.Abs(transform.position.x - target.position.x) < float.Epsilon)
        {
            yDir = (transform.position.y < target.position.y) ? 1 : -1;
        }
        else
        {
            xDir = (transform.position.x < target.position.x) ? 1 : -1;
        }

        AttemptMove<Player>(xDir, yDir);
    }

    protected override void Start()
    {
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        if (skipMove)
        {
            skipMove = false;
            return;
        }

        base.AttemptMove<T>(xDir, yDir);

        skipMove = true;
    }

    protected override void OnCantMove<T>(T component)
    {
        Player player = component as Player;

        player.LoseFood(playerDamage);
    }

    public int playerDamage;

    private Animator animator;
    private Transform target;
    private bool skipMove;
}
