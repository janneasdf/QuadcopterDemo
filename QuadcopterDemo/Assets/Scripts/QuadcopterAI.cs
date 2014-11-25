using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum FlightState 
{
    HOVER = 0,
    PATROLLING,
    IN_PURSUIT
}

public class QuadcopterAI : MonoBehaviour
{
    public Transform[] waypoints;
    public FlightState flightState;
    public float accelerationMagnitude;
    public float maxSpeed;
    public float propellerRotationSpeed;
	public float catchDistance;

    private float hoverOffset;
    private Vector3 position;
    private Vector3 velocity;
    private Vector3 acceleration;
    private Vector3 turningDirection;
    private Quaternion rotation;
    private List<Transform> propellers;
	private bool intruderSighted = false;
    private int targetWaypoint = 0;
	private GameObject player;
	private GameLogic gameLogic;

	// Use this for initialization
	void Start () {
        propellers = new List<Transform>();
        position = transform.position;
        foreach (Transform child in transform) 
        {
            if (child.name.Contains("Propeller"))
                propellers.Add(child);
        }
		player = GameObject.FindWithTag("Player");
		gameLogic = GameObject.FindWithTag("GameController").GetComponent<GameLogic>();
	}
	
	// Update is called once per frame
	void Update () {
        if (gameLogic.gameState == GameState.PLAYING)
            UpdatePlaying();
        else
            SimulateFlight(Hover());
        RotatePropellers();
    }

    void UpdatePlaying()
    {
        if (flightState == FlightState.HOVER)
            SimulateFlight(Hover());
        else if (flightState == FlightState.PATROLLING)
            SimulateFlight(Patrol());
        else if (flightState == FlightState.IN_PURSUIT)
            SimulateFlight(Pursue());
        Detect();   // try to detect player
    }

    FlightState Hover()
    {
        acceleration = accelerationMagnitude * -velocity.normalized;
        velocity += Vector3.Min(acceleration * Time.deltaTime, velocity.magnitude * -velocity.normalized);
        return FlightState.HOVER;
    }

    FlightState Patrol()
    {
        Vector3 target = waypoints[targetWaypoint].position;
        float d = (target - transform.position).magnitude;
        if (d < 1)
        {
            targetWaypoint++;
            targetWaypoint %= waypoints.Length;
        }
        target = waypoints[targetWaypoint].position;
        Vector3 direction = (target - transform.position).normalized;
        acceleration = accelerationMagnitude * direction;
        velocity += acceleration * Time.deltaTime;
        return FlightState.PATROLLING;
    }

    FlightState Pursue()
	{
		Vector3 target = player.transform.position;
		Vector3 direction = target - transform.position;
		direction.y = 0.0f;
		if (direction.magnitude < catchDistance)
		{
			flightState = FlightState.HOVER;
            return FlightState.HOVER;
		}
		direction.Normalize();
		acceleration = accelerationMagnitude * direction;
        velocity += acceleration * Time.deltaTime;
        return FlightState.IN_PURSUIT;
	}

    void RotatePropellers()
    {
        foreach (Transform propeller in propellers)
        {
            propeller.Rotate(new Vector3(0, propellerRotationSpeed * Time.deltaTime, 0));
        }
    }

    void SimulateFlight(FlightState state)
    {
        // Move the quadcopter according to velocity
        if (velocity.magnitude > maxSpeed)
            velocity = maxSpeed * velocity.normalized;
        position += velocity * Time.deltaTime;

        // Apply hover offset
        hoverOffset = Mathf.Sin(Time.timeSinceLevelLoad * 2.0f) / 20.0f;
        transform.position = new Vector3(position.x, position.y + hoverOffset, position.z);

        // Rotate towards acceleration direction
        if (state != FlightState.HOVER && new Vector3(acceleration.x, 0, acceleration.z) != Vector3.zero)
        {
            Quaternion accelerationLook = Quaternion.LookRotation(new Vector3(acceleration.x, 0, acceleration.z), Vector3.up);
            rotation = Quaternion.Lerp(rotation, accelerationLook, 2.0f * Time.deltaTime);
        }
        transform.localRotation = rotation;
        
        // Rotate the quadcopter to give a sense of acceleration
        if (acceleration != Vector3.zero)
        {
            turningDirection = Vector3.Lerp(turningDirection, acceleration.normalized, 0.5f * Time.deltaTime);
            Vector3 axis = transform.worldToLocalMatrix * Vector3.Cross(turningDirection, Vector3.up);
            transform.Rotate(axis, -Mathf.Min(25.0f, turningDirection.magnitude / 0.06f));
        }
    }

    void Detect()
    {
		if (intruderSighted)
		{
			flightState = FlightState.IN_PURSUIT;
			intruderSighted = false;
			gameLogic.AlertGuards();
		}
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
				Debug.Log("spotted!");
				intruderSighted = true;
			}
		}
	}
}
