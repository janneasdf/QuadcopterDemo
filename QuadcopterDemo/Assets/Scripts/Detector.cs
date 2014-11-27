using UnityEngine;
using System.Collections;

public class Detector : MonoBehaviour 
{
    private GameLogic gameLogic;
    private GameObject player;
    private float detectDistance = 24;
    private float fov = 84;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        gameLogic = GameObject.FindWithTag("GameController").GetComponent<GameLogic>();
    }

    void Update()
    {

        int hits = 0;
        Bounds bounds = player.GetComponent<CharacterController>().bounds;
        RaycastHit hit;
        Vector3 size = bounds.max - bounds.min;
        const float shrink = 0.6f;
        for (int x = 0; x < 2; ++x)
        {
            for (int y = 0; y < 2; ++y)
            {
                for (int z = 0; z < 2; ++z)
                {
                    Vector3 p = bounds.min + (1.0f - shrink) / 2.0f * size + new Vector3(
                                            x == 0 ? size.x : 0, 
                                            y == 0 ? size.y : 0, 
                                            z == 0 ? size.z : 0) * shrink;
                    Vector3 rayDirection = (p - transform.position).normalized;
                    float rayLength = (p - transform.position).magnitude;
                    if (Vector3.Angle(transform.forward, rayDirection) <= fov / 2.0f && rayLength <= detectDistance)
                    {
                        Ray ray = new Ray(transform.position, rayDirection);
                        Debug.DrawRay(transform.position, rayDirection * (p - transform.position).magnitude);

                        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag == "Player")
                        {
                            hits++;
                        }
                    }
                }
            }
        }
        if (hits > 4)
        {
            gameLogic.SoundTheAlarm();
        }
    }
}
