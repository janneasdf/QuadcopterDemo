using UnityEngine;
using System.Collections;

public enum GameState
{
	IN_MENU = 0,
	PLAYING,
    VICTORY,
    BUSTED
}

public class GameLogic : MonoBehaviour 
{
    public ThirdPersonController character;
	public GameObject guardPrefab;

    private GameState _gameState;
	private Transform goal;
	private Transform item;
	private GameObject player;
    private DirectionArrow directionArrow;
    private bool itemCollected = false;
    private bool alarm = false;

    public GameState gameState
    {
        get { return _gameState; }
        set
        {
            _gameState = value;
            character.isControllable = _gameState == GameState.PLAYING;
        }
    }

	public void StartGame()
	{
		gameState = GameState.PLAYING;
		directionArrow.target = item;
	}

	public void CollectItem()
	{
		itemCollected = true;
		directionArrow.target = goal;
        Transform wrench = player.transform.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck/Bip001 R Clavicle/Bip001 R UpperArm/Bip001 R Forearm/Bip001 R Hand/wrench");
        Transform tupla = player.transform.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck/Bip001 R Clavicle/Bip001 R UpperArm/Bip001 R Forearm/Bip001 R Hand/Tupla");
        wrench.renderer.enabled = false;
        tupla.renderer.enabled = true;
	}

	public void SoundTheAlarm()
	{
        if (alarm)
            return;
        alarm = true;
        GameObject[] quadCopters = GameObject.FindGameObjectsWithTag("Quadcopter");
        foreach (GameObject quadCopter in quadCopters)
        {
            quadCopter.GetComponent<QuadcopterAI>().OnAlarm();
        }
        GameObject.FindGameObjectWithTag(Tags.mainCamera).GetComponent<MainCameraGUI>().OnAlarm();
		// Spawn guards
		GameObject[] guardSpawns = GameObject.FindGameObjectsWithTag(Tags.guardSpawn);
		foreach (GameObject spawnPoint in guardSpawns)
		{
			Vector3 spawnPosition = spawnPoint.transform.position + 
				new Vector3(0, guardPrefab.transform.position.y, 0);
			GameObject.Instantiate(guardPrefab, spawnPosition, Quaternion.identity);
		}
	}

    public void Caught()
    {
        EndGame(false);
    }

    void EndGame(bool won)
    {
        if (gameState != GameState.PLAYING)
            return;
        if (won)
        {
            gameState = GameState.VICTORY;
            GameObject.FindGameObjectWithTag(Tags.mainCamera).GetComponent<MainCameraGUI>().OnVictory();
        }
        else
        {
            gameState = GameState.BUSTED;
            GameObject.FindGameObjectWithTag(Tags.mainCamera).GetComponent<MainCameraGUI>().OnGameOver();
        }
        directionArrow.target = null;
    }

    void Start()
    {
        gameState = GameState.IN_MENU;
		goal = GameObject.FindWithTag(Tags.goal).transform;
		player = GameObject.FindWithTag(Tags.player);
		directionArrow = GameObject.FindWithTag(Tags.directionArrow).GetComponent<DirectionArrow>();
		item = GameObject.FindWithTag(Tags.collectable).transform;
	}
	
	void Update () {
		if (gameState == GameState.PLAYING)
		{
		    CheckPlayerWin(); 
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
}
