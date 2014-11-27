using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour {

    public Transform target;
    public float distanceUp;
    public float distanceAway;
    public float smooth;
    public float rotationSpeed;
    public float mouseSensitivity;

    private Vector3 targetPosition;
    private Vector3 lookDirection;
    private Vector3 velocityCamSmooth = Vector3.zero;
    private float camSmoothDampTime = 0.1f;
    private GameLogic gameLogic;

    void Start()
    {
        gameLogic = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<GameLogic>();
    }

    void LateUpdate()
    {
        if (gameLogic.gameState == GameState.IN_MENU)
            return;
        float mouseInput = Input.GetKey(KeyCode.Mouse1) ? mouseSensitivity * Input.GetAxis("Mouse X") : 0;
        float rotationInput = Input.GetAxis("Horizontal2") != 0 ? Input.GetAxis("Horizontal2") : mouseInput;
        transform.RotateAround(target.position, Vector3.up, Time.deltaTime * rotationSpeed * rotationInput);

        Vector3 characterOffset = target.position + new Vector3(0, distanceUp, 0);
        
        lookDirection = characterOffset - this.transform.position;
        lookDirection.y = 0;
        lookDirection.Normalize();

        targetPosition = characterOffset - lookDirection * distanceAway;
        targetPosition = CollisionRotate(characterOffset, targetPosition);

        SmoothPosition(this.transform.position, targetPosition);


        transform.LookAt(target);
	}

    private void SmoothPosition(Vector3 fromPos, Vector3 toPos)
    {
        this.transform.position = Vector3.SmoothDamp(fromPos, toPos, ref velocityCamSmooth, camSmoothDampTime);
    }

    private Vector3 CollisionRotate(Vector3 fromObject, Vector3 toTarget)
    {
        for (int angle = 0; angle <= 180; angle += 10)
        {
            Vector3 newToTarget1 = RotatePointAroundPivot(toTarget, fromObject, angle);
            Vector3 newToTarget2 = RotatePointAroundPivot(toTarget, fromObject, -angle);
            if (!CheckCollision(fromObject, newToTarget1))
                return newToTarget1;
            if (!CheckCollision(fromObject, newToTarget2))
                return newToTarget2;
        }
        return toTarget;
    }

    private bool CheckCollision(Vector3 fromObject, Vector3 toTarget)
    {
        Vector3[] corners = GetNearPlaneCorners(toTarget);
        for (int i = 0; i < 4; i++)
        {
            Vector3 cornerOffset = corners[i] - toTarget;
            Vector3 rayStart = fromObject + cornerOffset;
            Vector3 rayEnd = corners[i];

            RaycastHit hit = new RaycastHit();
            if (Physics.Linecast(rayStart, rayEnd, out hit))
                return true;
        }
        return false;
    }

    private Vector3[] GetNearPlaneCorners(Vector3 toTarget)
    {
        Vector3[] corners = new Vector3[4];
        float offsetX = camera.nearClipPlane * Mathf.Tan(camera.fieldOfView / 2);
        float offsetY = camera.nearClipPlane * Mathf.Tan(camera.fieldOfView / 2);
        corners[0] = toTarget + new Vector3(-offsetX, offsetY, camera.nearClipPlane);
        corners[1] = toTarget + new Vector3(-offsetX, -offsetY, camera.nearClipPlane);
        corners[2] = toTarget + new Vector3(offsetX, -offsetY, camera.nearClipPlane);
        corners[3] = toTarget + new Vector3(offsetX, offsetY, camera.nearClipPlane);
        return corners;
    }

    private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, float angle) {
       Vector3 direction = point - pivot;
       direction = Quaternion.Euler(new Vector3(0, angle, 0)) * direction; 
       point = direction + pivot;
       return point;
    }
}
