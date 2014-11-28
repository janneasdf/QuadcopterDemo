using UnityEngine;
using System.Collections;

public class Guard : MonoBehaviour 
{
    GameObject player;
    GameLogic gameLogic;
    Animator animator;
    AIPath AI;

    private float nextHeyStopTime;

	void Start () 
    {
        player = GameObject.FindGameObjectWithTag(Tags.player);
        gameLogic = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<GameLogic>();
        animator = GetComponent<Animator>();
        AI = GetComponent<AIPath>();
        AI.target = player.transform;
        nextHeyStopTime = 10.0f + Random.value * 5.0f; 
	}
	
	void Update () 
    {
        nextHeyStopTime -= Time.deltaTime;
        if (nextHeyStopTime < 0 && gameLogic.gameState == GameState.PLAYING)
        {
            nextHeyStopTime = 10.0f + Random.value * 5.0f;
            GetComponent<AudioSource>().Play();
        }
        if (gameLogic.gameState == GameState.PLAYING)
        {
            AI.enabled = true;
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

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.gameObject.tag == Tags.player && gameLogic.gameState == GameState.PLAYING)
        {
            gameLogic.Caught();
        }
    }
}
