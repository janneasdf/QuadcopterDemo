using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour {

	private GameLogic gameLogic;

	// Use this for initialization
	void Start () {
		gameLogic = GameObject.FindWithTag("GameController").GetComponent<GameLogic>();
	}
	
	// Update is called once per frame
	void Update () {
	
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
