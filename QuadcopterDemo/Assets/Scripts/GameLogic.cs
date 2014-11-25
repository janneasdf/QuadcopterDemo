using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour {

	public float gameEndDuration;	// ending duration in seconds

	private bool gameEnding = false;
	private float sinceEnded = 0.0f;
	
	public void EndGame()
	{
		Debug.Log("Player caught, game ending");
		sinceEnded = 0.0f;
		gameEnding = true;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (gameEnding)
		{
			sinceEnded += Time.deltaTime;
			if (sinceEnded > gameEndDuration)
			{
				Restart();
			}
		}
	}

	void Restart()
	{
		gameEnding = false;
		// Go to the menu thing
	}

	void OnGUI()
	{
		if (gameEnding)
		{
			float t = sinceEnded / gameEndDuration;
			Rect textRect = new Rect(Screen.width - 100.0f, Screen.height - 100.0f, 
			                         Screen.width + 100.0f, Screen.height + 100.0f);
			GUI.TextArea(textRect, "asd");
		}
	}

}
