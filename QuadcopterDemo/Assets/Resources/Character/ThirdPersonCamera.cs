using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour {

    public Transform target;
    public float distanceUp;
    public float distanceAway;
    public float smooth;

    private Vector3 targetPosition;
    private Vector3 lookDirection;
    private Vector3 velocityCamSmooth = Vector3.zero;
    private float camSmoothDampTime = 0.1f;

	void Start () {
	
	}
	
	void LateUpdate () {
        Vector3 characterOffset = target.position + new Vector3(0, distanceUp, 0);
        
        lookDirection = characterOffset - this.transform.position;
        lookDirection.y = 0;
        lookDirection.Normalize();

        targetPosition = characterOffset + target.up * distanceUp - lookDirection * distanceAway;

        CheckCollision(characterOffset, ref targetPosition);
        SmoothPosition(this.transform.position, targetPosition);


        transform.LookAt(target);
	}

    private void SmoothPosition(Vector3 fromPos, Vector3 toPos)
    {
        this.transform.position = Vector3.SmoothDamp(fromPos, toPos, ref velocityCamSmooth, camSmoothDampTime);
    }

    private void CheckCollision(Vector3 fromObject, ref Vector3 toTarget)
    {
        RaycastHit hit = new RaycastHit();
        if (Physics.Linecast(fromObject, toTarget, out hit))
        {
            toTarget = new Vector3(hit.point.x, toTarget.y, hit.point.z);
        }
    }
}
