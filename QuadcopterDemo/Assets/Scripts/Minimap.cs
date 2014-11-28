using UnityEngine;
using System.Collections;

public class Minimap : MonoBehaviour 
{
	public Transform target;
    public Transform largeTarget;
    public Transform tupla;
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
    private Texture tuplaIcon;
    private Texture playerIcon;
    private Camera[] cameras;

	void Start() 
    {
		float aspectRatio = Screen.width / (float)Screen.height;

		camera.rect = new Rect(1 - minimapWidth - offset, 1-aspectRatio * minimapHeight - offset*aspectRatio, minimapWidth, aspectRatio*minimapHeight);
		RectSmall = new Vector4(camera.rect.x, camera.rect.y, camera.rect.width, camera.rect.height);
		orthSize = orthSizeMin;
        tuplaIcon = Resources.Load<Texture>("GUI/Tupla");
        playerIcon = Resources.Load<Texture>("GUI/Player");

        cameras = GameObject.FindObjectsOfType<Camera>();
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
        foreach (Camera otherCamera in cameras)
            if (otherCamera.gameObject.activeInHierarchy && otherCamera.depth > camera.depth)
                return;

        GUIStyle style = GUI.skin.label;
        style.alignment = TextAnchor.MiddleCenter;
        float iconWidth = Screen.width * 0.04f;
        float iconHeight = Screen.height * 0.04f;
        float cx = camera.rect.x * Screen.width;
        float cy = (1.0f - camera.rect.y) * Screen.height;
        float cwidth = camera.rect.width * Screen.width;
        float cheight = camera.rect.height * Screen.height;
        float aspectRatio = cwidth / cheight;

        // Draw tupla icon
        if (tupla)
        {
            Vector3 toTupla = tupla.position - transform.position;
            Vector2 toTuplaOrtho = new Vector2((toTupla.x + aspectRatio * orthSize) / (aspectRatio * orthSize * 2.0f), (toTupla.z + orthSize) / (orthSize * 2.0f));
            float x = cx + cwidth * toTuplaOrtho.x;
            float y = cy - cheight * toTuplaOrtho.y;
            if (x > cx && x < cx + cwidth && y > cy - cheight && y < cy)
                GUI.DrawTexture(new Rect(x - iconWidth / 2, y - iconHeight / 2, iconWidth * Screen.height / (float)Screen.width, iconHeight), tuplaIcon);
        }

        // Draw player icon
        if (target)
        {
            Vector3 toTarget = target.position - transform.position;
            Vector2 toTargetOrtho = new Vector2((toTarget.x + aspectRatio * orthSize) / (aspectRatio * orthSize * 2.0f), (toTarget.z + orthSize) / (orthSize * 2.0f));
            float x = cx + cwidth * toTargetOrtho.x;
            float y = cy - cheight * toTargetOrtho.y;
            if (x > cx && x < cx + cwidth && y > cy - cheight && y < cy)
                GUI.DrawTexture(new Rect(x - iconWidth * Screen.height / (float)Screen.width / 2, y - iconHeight / 2, iconWidth * Screen.height / (float)Screen.width, iconHeight), playerIcon);
        }
    }
}
