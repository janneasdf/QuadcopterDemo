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

    private float hoverOffset;
    private Vector3 position;
    private Vector3 velocity;
    private Vector3 acceleration;
    private Vector3 turningDirection;
    private Quaternion rotation;
    private List<Transform> propellers;

    int targetWaypoint = 0;

	// Use this for initialization
	void Start () {
        propellers = new List<Transform>();
        position = transform.position;
        foreach (Transform child in transform) 
        {
            if (child.name.Contains("Propeller"))
                propellers.Add(child);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (flightState == FlightState.HOVER)
            Hover();
        else if (flightState == FlightState.PATROLLING)
            Patrol();

        SimulateFlight();
        RotatePropellers();
        Detect();   // try to detect player
    }

    void Hover()
    {
        acceleration = accelerationMagnitude * -velocity.normalized;
        velocity += Vector3.Min(acceleration * Time.deltaTime, velocity.magnitude * -velocity.normalized);
    }

    void Patrol()
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
    }

    void RotatePropellers()
    {
        foreach (Transform propeller in propellers)
        {
            Vector3 rot = propeller.localRotation.eulerAngles + new Vector3(0, propellerRotationSpeed * Time.deltaTime, 0);
            propeller.localRotation = Quaternion.Euler(rot);
        }
    }

    void SimulateFlight()
    {
        // Move the quadcopter according to velocity
        if (velocity.magnitude > maxSpeed)
            velocity = maxSpeed * velocity.normalized;
        position += velocity * Time.deltaTime;

        // Apply hover offset
        hoverOffset = Mathf.Sin(Time.timeSinceLevelLoad * 2.0f) / 20.0f;
        transform.position = new Vector3(position.x, position.y + hoverOffset, position.z);

        // Rotate towards acceleration direction
        if (flightState != FlightState.HOVER)
        {
            Quaternion accelerationLook = Quaternion.LookRotation(new Vector3(acceleration.x, 0, acceleration.z), Vector3.up);
            rotation = Quaternion.Lerp(rotation, accelerationLook, 2.0f * Time.deltaTime);
        }
        transform.localRotation = rotation;
        
        // Rotate the quadcopter to give a sense of acceleration
        turningDirection = Vector3.Lerp(turningDirection, acceleration.normalized, 0.5f * Time.deltaTime);
        transform.localRotation = rotation;
        Vector3 axis = transform.worldToLocalMatrix * Vector3.Cross(turningDirection, Vector3.up);
        transform.Rotate(axis, -Mathf.Min(25.0f, turningDirection.magnitude / 0.06f));
    }

    void Detect()
    {

    }
}
