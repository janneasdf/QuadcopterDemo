using UnityEngine;
using System.Collections;

public class QuadcopterAI : MonoBehaviour {

    public float speed;
    public Transform[] waypoints;

    int targetWaypoint = 0;

    bool patrolling = true;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (patrolling)
            Patrol();
        Detect();   // try to detect player
	}

    void Patrol()
    {
        Vector3 target = waypoints[targetWaypoint].position;
        float d = (target - transform.position).magnitude;
        if (d < 0.1)
        {
            targetWaypoint++;
            targetWaypoint %= waypoints.Length;
        }
        target = waypoints[targetWaypoint].position;
        Vector3 direction = (target - transform.position).normalized;
        transform.position += speed * direction * Time.deltaTime;
    }

    void Detect()
    {

    }

}
