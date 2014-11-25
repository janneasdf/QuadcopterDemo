using UnityEngine;
using System.Collections;

public class Minimap : MonoBehaviour {
	
	public Transform Target;

	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (Target.position.x, transform.position.y, Target.position.z);
	}
}
