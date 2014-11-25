using UnityEngine;
using System.Collections;

public class DirectionArrow : MonoBehaviour {

	public Transform target;
	public Vector3 cameraDistance;

	private Transform mainCamera;

	// Use this for initialization
	void Start () {
		mainCamera = GameObject.FindWithTag(Tags.mainCamera).transform;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = mainCamera.position +
			mainCamera.right * cameraDistance.x +
			mainCamera.up * cameraDistance.y +
			mainCamera.forward * cameraDistance.z;
		Vector3 direction = (target.position - transform.position).normalized;
		transform.rotation = Quaternion.LookRotation(direction);
	}
}
