using UnityEngine;
using System.Collections;

public enum HelpState
{
    NONE = 0,
    MOVE,
    JUMP,
    MAP,
    CANDY
}

public class MainCameraGUI : MonoBehaviour
{
    public GameObject tupla1;
    public GameObject tupla2;

    public float fadeTime;
    
    private GameLogic logic;
    private AudioSource[] audioSources;
    public HelpState helpState;
    private HelpState drawnHelpState;

    private Texture moveHelp;
    private Texture jumpHelp;
    private Texture mapHelp;
    private Texture nextTexture;

    private Vector3 prevPosition;
    private float distanceDelta;
    private float distanceMoved;
    private float fadeTimer;
    private float candyTimer;

    private Font font;

    void Start()
    {
        helpState = HelpState.MOVE;
        drawnHelpState = HelpState.MOVE;
        logic = GameObject.FindWithTag(Tags.gameController).GetComponent<GameLogic>();
        tupla1.renderer.enabled = false;
        tupla2.renderer.enabled = false;
        audioSources = GetComponents<AudioSource>();

        moveHelp = Resources.Load<Texture>("GUI/WASD");
        jumpHelp = Resources.Load<Texture>("GUI/Space");
        mapHelp = Resources.Load<Texture>("GUI/Tab");

        font = Resources.Load<Font>("Fonts/Corbel Regular");
	}

    public void OnAlarm()
    {
        audioSources[0].Play();
        audioSources[1].Play();
        audioSources[2].Stop();
    }

    public void OnGameOver()
    {
        audioSources[4].Play();
    }

    public void OnVictory()
    {
        audioSources[3].Play();
    }

    void Update()
    {
        if (logic.gameState != GameState.PLAYING)
            return;

        float distanceDelta = (transform.position - prevPosition).magnitude;
        prevPosition = transform.position;
        distanceMoved += distanceDelta;

        if (helpState == HelpState.CANDY)
            candyTimer += Time.deltaTime;

        if (helpState == HelpState.MOVE && drawnHelpState == helpState && distanceMoved > 15 && fadeTimer == 0)
            helpState = HelpState.JUMP;
        else if (helpState == HelpState.JUMP && drawnHelpState == helpState && Input.GetAxis("Jump") > 0 && fadeTimer == 0)
            helpState = HelpState.MAP;
        else if (helpState == HelpState.MAP && drawnHelpState == helpState && Input.GetAxis("Map") > 0 && fadeTimer == 0)
            helpState = HelpState.CANDY;
        else if (helpState == HelpState.CANDY && drawnHelpState == helpState && candyTimer > 10 && fadeTimer == 0)
            helpState = HelpState.NONE;

        if (drawnHelpState != helpState)
            fadeTimer += Time.deltaTime;
        else
            fadeTimer = Mathf.Max(0, fadeTimer - Time.deltaTime);
        if (fadeTimer > fadeTime)
            drawnHelpState = helpState;
    }

    void OnGUI()
    {
        const float width = 200;
        const float height = 80;
        const float margin = 10;

        GUIStyle style = GUI.skin.button;
        style.fontSize = 52;
        style.normal.textColor = new Color(1, 1, 1);
        style.hover.textColor = style.normal.textColor * 0.8f;
        style.active.textColor = style.normal.textColor * 0.6f;
        style.font = font;
        style.fontStyle = FontStyle.Bold;

        GUI.backgroundColor = Color.clear;

        Rect rect = new Rect(Screen.width / 2 - width / 2, Screen.height / 2 - height - 60,
                                width, height);

        if (logic.gameState == GameState.VICTORY)
        {
            tupla1.renderer.enabled = true;
            tupla1.transform.Rotate(new Vector3(1, 1, 1) * 15 * Time.deltaTime);
            tupla2.renderer.enabled = true;
            tupla2.transform.Rotate(new Vector3(1, 1, 1) * 15 * Time.deltaTime);
            GUI.Label(rect, "Victory!", style);
        }
        else if (logic.gameState == GameState.BUSTED)
            GUI.Label(rect, "Busted!", style);

        if (logic.gameState == GameState.BUSTED || logic.gameState == GameState.VICTORY)
        {
            style.fontSize = 32;
            if (GUI.Button(new Rect(Screen.width / 2 - width / 2, Screen.height / 2 - margin, width, 50), "Main menu", style))
            {
                Application.LoadLevel("Scene1");
            }
            if (GUI.Button(new Rect(Screen.width / 2 - width / 2, Screen.height / 2 + height / 2 + margin, width, 50), "Exit", style))
                Application.Quit();
        }

        if (logic.gameState == GameState.PLAYING)
        {
            style.fontSize = 32;
            style.wordWrap = true;
            float helpHeight = Screen.height / 10.0f;
            GUI.color = new Color(1.0f, 1.0f, 1.0f, (fadeTime - fadeTimer) / fadeTime);
            if (drawnHelpState == HelpState.MOVE)
                GUI.DrawTexture(new Rect(20, 20, helpHeight / moveHelp.height * moveHelp.width, helpHeight), moveHelp);
            else if (drawnHelpState == HelpState.JUMP)
                GUI.DrawTexture(new Rect(20, 20, helpHeight / jumpHelp.height * jumpHelp.width, helpHeight), jumpHelp);
            else if (drawnHelpState == HelpState.MAP)
                GUI.DrawTexture(new Rect(20, 20, helpHeight / mapHelp.height * mapHelp.width, helpHeight), mapHelp);
            else if (drawnHelpState == HelpState.CANDY)
                GUI.Label(new Rect(20, 20, 400, 140), "Follow the red arrow. Find the candy and return to where you started from.", style);
            GUI.color = new Color(1.0f, 1.0f, 1.0f);
        }
    }
}
