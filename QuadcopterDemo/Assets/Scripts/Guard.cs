using UnityEngine;
using System.Collections;

public class Guard : MonoBehaviour 
{
    GameObject player;
    GameLogic gameLogic;
    Animator animator;
    AIPath AI;

	void Start () 
    {
        player = GameObject.FindGameObjectWithTag(Tags.player);
        gameLogic = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<GameLogic>();
        animator = GetComponent<Animator>();
        AI = GetComponent<AIPath>();
        AI.target = player.transform;
	}
	
	void Update () 
    {
        if (gameLogic.gameState == GameState.PLAYING)
        {
            AI.enabled = true;
            CheckCatch();
            Move();
        }
        else
        {
            AI.enabled = false;
            animator.SetFloat("Speed", 0);
        }
	}

    void Move()
    {
        animator.SetFloat("Speed", AI.speed);
    }

    void CheckCatch()
    {
        float distance = (player.transform.position - transform.position).magnitude;
        if (gameLogic.gameState == GameState.PLAYING && distance < AI.slowdownDistance)
        {
            gameLogic.Caught();
        }
    }
}
