using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour {

    public Transform target;
    public float distanceUp;
    public float distanceAway;
    public float smooth;
    public float rotationSpeed;
    public float mouseSensitivity;
    public float minOffsetDistance = 0.3f;

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
        CollisionHandling(characterOffset, ref targetPosition);

        SmoothPosition(this.transform.position, targetPosition);


        transform.LookAt(target);
	}

    private void SmoothPosition(Vector3 fromPos, Vector3 toPos)
    {
        this.transform.position = Vector3.SmoothDamp(fromPos, toPos, ref velocityCamSmooth, camSmoothDampTime);
    }

    private void CollisionHandling(Vector3 fromObject, ref Vector3 toTarget)
    {
        // First try to rotate the camera
        Vector3 minErrorTarget = toTarget;
        float maxHitFraction = 0.0f;
        for (int angle = 0; angle <= 45; angle += 1)
        {
            Vector3 newToTarget1 = RotatePointAroundPivot(toTarget, fromObject, angle);
            Vector3 newToTarget2 = RotatePointAroundPivot(toTarget, fromObject, -angle);
            float hitFraction1 = CheckCollision(fromObject, ref newToTarget1);
            if (hitFraction1 >= 1.0f)
            {
                transform.RotateAround(target.position, Vector3.up, angle);
                return;
            }
            else if (hitFraction1 > maxHitFraction)
            {
                maxHitFraction = hitFraction1;
                minErrorTarget = newToTarget1;
            }
            float hitFraction2 = CheckCollision(fromObject, ref newToTarget2);
            if (hitFraction2 >= 1.0f)
            {
                transform.RotateAround(target.position, Vector3.up, -angle);
                return;
            }
            else if (hitFraction2 > maxHitFraction)
            {
                maxHitFraction = hitFraction2;
                minErrorTarget = newToTarget2;
            }
        }
        toTarget = minErrorTarget;
    }

    private float CheckCollision(Vector3 fromObject, ref Vector3 toTarget)
    {
        float minHitFraction = 1.0f;
        float offsetDistance = (fromObject - toTarget).magnitude;
        float raycastLength = offsetDistance - minOffsetDistance;

        if (raycastLength < 0.0f)
            return minHitFraction;

        Vector3 cameraOut = (fromObject - toTarget).normalized;
        Vector3 nearestCameraPosition = fromObject - cameraOut * minOffsetDistance;

        Vector3[] corners = GetNearPlaneCorners(toTarget);
        for (int i = 0; i < 4; i++)
        {
            Vector3 cornerOffset = corners[i] - toTarget;
            Vector3 rayStart = nearestCameraPosition + cornerOffset;
            Vector3 rayEnd = corners[i];

            RaycastHit hit = new RaycastHit();
            Debug.DrawRay(rayStart, rayEnd - rayStart);
            if (Physics.Linecast(rayStart, rayEnd, out hit))
                minHitFraction = hit.distance / raycastLength;
        }
        toTarget = nearestCameraPosition - cameraOut * Mathf.Min(raycastLength * minHitFraction * 0.99f, 0);
        return minHitFraction;
    }

    private Vector3[] GetNearPlaneCorners(Vector3 toTarget)
    {
        Vector3[] corners = new Vector3[4];
        float offsetX = camera.nearClipPlane * Mathf.Tan(camera.fieldOfView / 2);
        float offsetY = camera.nearClipPlane * Mathf.Tan(camera.fieldOfView / 2);
        corners[0] = toTarget + transform.rotation * new Vector3(-offsetX, offsetY, camera.nearClipPlane);
        corners[1] = toTarget + transform.rotation * new Vector3(-offsetX, -offsetY, camera.nearClipPlane);
        corners[2] = toTarget + transform.rotation * new Vector3(offsetX, -offsetY, camera.nearClipPlane);
        corners[3] = toTarget + transform.rotation * new Vector3(offsetX, offsetY, camera.nearClipPlane);
        return corners;
    }

    private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, float angle) {
       Vector3 direction = point - pivot;
       direction = Quaternion.Euler(new Vector3(0, angle, 0)) * direction; 
       point = direction + pivot;
       return point;
    }
}
