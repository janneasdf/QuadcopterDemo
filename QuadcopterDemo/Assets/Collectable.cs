using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour {

	public float rotationSpeed;

	private GameLogic gameLogic;

	// Use this for initialization
	void Start () {
		gameLogic = GameObject.FindWithTag(Tags.gameController).GetComponent<GameLogic>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 euler = transform.rotation.eulerAngles + new Vector3(0, rotationSpeed * Time.deltaTime, 0);
		transform.rotation = Quaternion.Euler(euler);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			AudioSource.PlayClipAtPoint(audio.clip, transform.position);
			gameLogic.CollectItem();
			Destroy(gameObject);
		}
	}
}
