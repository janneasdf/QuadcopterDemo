using UnityEngine;
using System.Collections;

public class DirectionArrow : MonoBehaviour {
    public Transform[] waypoints;
    public Transform actualTarget;

    private Transform target;
    private float distance;
    private int currentWaypoint = 0;
    private int increment = -1;

    public void SetNewTarget(Transform newTarget)
    {
        increment *= -1;
        currentWaypoint = increment < 0 ? waypoints.Length - 1 : 0;
        actualTarget = newTarget;
        target = waypoints[currentWaypoint];
        Debug.Log("___________________________________________");
        Debug.Log(target);
    }

    public void Clear()
    {
        target = null;
    }

	void Update () {
        if (!target)
        {
            renderer.enabled = false;
        }
        else
        {
            if ((target.position - transform.position).magnitude < 10.0f)
            {
                currentWaypoint += increment;
                if (currentWaypoint >= waypoints.Length || currentWaypoint < 0)
                    target = actualTarget;
                else
                    target = waypoints[currentWaypoint];
            }
                
            transform.LookAt(target.position);
            transform.Rotate(Vector3.right, 20, Space.Self);
            renderer.enabled = true;
        }
		
	}

    void OnGUI()
    {
        if (!target)
            return;
        const float width = 100;
        const float height = 20;

        GUIStyle style = GUI.skin.label;
        style.fontSize = 16;
        style.normal.textColor = new Color(1, 1, 1);
        style.hover.textColor = style.normal.textColor * 0.8f;
        style.active.textColor = style.normal.textColor * 0.6f;
        style.fontStyle = FontStyle.Bold;
        style.alignment = TextAnchor.MiddleCenter;

        GUI.backgroundColor = Color.clear;

        float distance = Mathf.Max(0, (target.position - transform.position).magnitude - 10.0f);
        if (renderer.enabled)
        {
            GUI.Label(new Rect(Screen.width / 2 - width / 2, Screen.height * 0.15f, width, height), distance.ToString("#.0") + " m");
        }
    }
}
