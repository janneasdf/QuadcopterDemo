using UnityEngine;
using System.Collections;

public enum GameState
{
	IN_MENU = 0,
	PLAYING
}

public class GameLogic : MonoBehaviour {

	public GameState gameState = GameState.IN_MENU;

	public float gameEndDuration;	// ending duration in seconds

	private Transform goal;
	private Transform item;
	private bool itemCollected = false;
	private bool gameEnding = false;
	private bool gameWon = false;
	private float sinceEnded = 0.0f;
	private GameObject player;
	private DirectionArrow directionArrow;

	public void StartGame()
	{
		gameState = GameState.PLAYING;
		directionArrow.target = item;

	}

	public void EndGame(bool won)
	{
		if (gameEnding)
			return;
		Debug.Log("Game ending");
		sinceEnded = 0.0f;
		gameEnding = true;
		gameWon = won;
	}

	public void CollectItem()
	{
		itemCollected = true;
		directionArrow.target = goal;
	}

	public void AlertGuards()
	{
		Debug.Log("Guards have been alerted!");
		EndGame(false);
	}

	// Use this for initialization
	void Start () {
		goal = GameObject.FindWithTag(Tags.goal).transform;
		player = GameObject.FindWithTag(Tags.player);
		directionArrow = GameObject.FindWithTag(Tags.directionArrow).GetComponent<DirectionArrow>();
		item = GameObject.FindWithTag(Tags.collectable).transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (gameState == GameState.PLAYING)
		{
			if (gameEnding)
			{
				sinceEnded += Time.deltaTime;
				if (sinceEnded > gameEndDuration)
				{
					ReturnToMenu();
				}
			}
			else
			{
				CheckPlayerWin(); 
			}
		}
	}

	void CheckPlayerWin()
	{
		if (!itemCollected)
			return;
		Vector3 delta = (goal.position - player.transform.position);
		delta.y = 0;
		if (delta.magnitude < 2.0f)
		{
			EndGame(true);
		}
	}

	void ReturnToMenu()
	{
		gameEnding = false;
		gameState = GameState.IN_MENU;
		itemCollected = false;
	}

	void OnGUI()
	{
		if (gameEnding)
		{
			float t = sinceEnded / gameEndDuration;

			const float width = 400;
			const float height = 80;
			const float margin = 10;
			
			GUIStyle style = GUI.skin.button;
			style.fontSize = 40;
			style.normal.textColor = new Color(1, 1, 1);
			style.hover.textColor = style.normal.textColor * 0.8f;
			style.active.textColor = style.normal.textColor * 0.6f;

			Rect rect = new Rect(Screen.width / 2 - width / 2, Screen.height / 2 - height / 2,
			                     width, height);
			if (gameWon)
				GUI.Label(rect, "You win!", style);
			else
				GUI.Label(rect, "You were caught!", style);
		}
	}

}
