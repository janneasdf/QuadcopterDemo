using UnityEngine;
using System.Collections;

public class DirectionArrow : MonoBehaviour {

	public Transform target;
    private float distance;

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

        float distance = (target.position - transform.position).magnitude;
        if (renderer.enabled)
        {
            GUI.Label(new Rect(Screen.width / 2 - width / 2, Screen.height * 0.09f, width, height), distance.ToString("#.0") + " m");
        }
    }
}
