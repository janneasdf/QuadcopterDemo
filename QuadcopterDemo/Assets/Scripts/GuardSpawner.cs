using UnityEngine;
using System.Collections;

public class GuardSpawner : MonoBehaviour {

	public float spawnDistanceStart;
	public float spawnDistanceEnd;
	public GameObject guardPrefab;

	private Transform player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag(Tags.player).transform;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 delta = player.position - transform.position;
		delta.y = 0;
		float playerDistance = delta.magnitude;
		if (playerDistance > spawnDistanceStart && playerDistance < spawnDistanceEnd)
		{
			GameObject.Instantiate(guardPrefab, 
			                       transform.position + guardPrefab.transform.position,
			                       Quaternion.identity);
			Destroy(this);
		}
	}
}
