using UnityEngine;
using System.Collections;

public class Minimap : MonoBehaviour 
{
	public Transform target;
    public Transform largeTarget;
    public float height;
	
	private float minimapWidth = 0.14f;
	private float minimapHeight = 0.14f;
	private float offset = 0.02f;
	private float speed = 4.0f;

	// Minimap rect variables
	private Vector4 RectSmall = new Vector4();
	private Vector4 RectBig = new Vector4(0, 0, 1, 1);
	private float orthSize;
	private float orthSizeMin = 20;
	private float orthSizeMax = 70; // This should change according to map size

	void Start() 
    {
		float aspectRatio = Screen.width / (float)Screen.height;

		camera.rect = new Rect(1 - minimapWidth - offset, 1-aspectRatio * minimapHeight - offset*aspectRatio, minimapWidth, aspectRatio*minimapHeight);
		RectSmall = new Vector4(camera.rect.x, camera.rect.y, camera.rect.width, camera.rect.height);
		orthSize = orthSizeMin;
	}
		
	void LateUpdate() 
    {
        Transform t = Input.GetAxis("Map") > 0 ? largeTarget : target;
        transform.position = Vector3.Lerp(transform.position, new Vector3(t.position.x, height, t.position.z), Time.deltaTime * speed);
		MapEnlargementStuff();
	}

	void MapEnlargementStuff()
	{
		Vector4 currentRect = new Vector4 (camera.rect.x, camera.rect.y, camera.rect.width, camera.rect.height);
		float targetOrthSize = 0.0f;

		Vector4 targetRect = new Vector4 ();
		if (Input.GetAxis("Map") > 0) 
        {
			targetRect = RectBig;
			targetOrthSize = orthSizeMax;	
		} 
        else 
        {
			targetRect = RectSmall;
			targetOrthSize = orthSizeMin;
		}

		Vector4 result = Vector4.Lerp (currentRect, targetRect, Time.deltaTime*speed*2.0f);
		orthSize = Mathf.Lerp(orthSize, targetOrthSize, Time.deltaTime*speed);

		camera.rect = new Rect (result.x, result.y, result.z, result.w);
		camera.orthographicSize = orthSize;
	}

    void OnGUI()
    {
        //GUI.DrawTexture();
    }
}
