using UnityEngine;
using System.Collections;

public class Detector : MonoBehaviour 
{
    private GameLogic gameLogic;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        gameLogic = GameObject.FindWithTag("GameController").GetComponent<GameLogic>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Vector3 rayDirection = (player.transform.position - transform.position).normalized;
            Ray ray = new Ray(transform.position + rayDirection,
                              rayDirection);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            if (hit.collider.gameObject.tag == "Player")
            {
                gameLogic.SoundTheAlarm();
            }
        }
    }
}
