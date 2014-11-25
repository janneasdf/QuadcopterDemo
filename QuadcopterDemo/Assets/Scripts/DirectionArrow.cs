using UnityEngine;
using System.Collections;

public class DirectionArrow : MonoBehaviour {

	public Transform target;
	
	void Update () {
        if (!target)
        {
            renderer.enabled = false;
        }
        else
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(direction);
            renderer.enabled = true;
        }
		
	}
}
