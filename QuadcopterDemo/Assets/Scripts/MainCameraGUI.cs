using UnityEngine;
using System.Collections;

public enum HelpState
{
    NONE = 0,
    MOVE,
    JUMP,
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
    private Texture lookHelp;
    private Texture jumpHelp;
    private Texture mapHelp;
    private Texture nextTexture;

    private Vector3 prevPosition;
    private float distanceDelta;
    private float distanceMoved;
    private float distanceLooked;
    private float fadeTimer;

    private Font font;
    int activeButton = 0;
    bool controller = false;
    bool mapped = false;
    bool jumped = false;
    private float candyTimer = 0;
    void Start()
    {
        helpState = HelpState.MOVE;
        drawnHelpState = HelpState.MOVE;
        logic = GameObject.FindWithTag(Tags.gameController).GetComponent<GameLogic>();
        tupla1.renderer.enabled = false;
        tupla2.renderer.enabled = false;
        audioSources = GetComponents<AudioSource>();

        moveHelp = Resources.Load<Texture>("GUI/WASD");
        lookHelp = Resources.Load<Texture>("GUI/Mouse");
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
        distanceLooked += Mathf.Abs(Input.GetAxis("Horizontal2")) + (Input.GetKey(KeyCode.Mouse1) ? Mathf.Abs(Input.GetAxis("Mouse X")) : 0);

        if (helpState == HelpState.JUMP && Input.GetAxis("Jump") > 0)
            jumped = true;
        if (helpState == HelpState.JUMP && Input.GetAxis("Map") > 0)
            mapped = true;

        if (helpState == HelpState.CANDY)
            candyTimer += Time.deltaTime;

        if (helpState == HelpState.MOVE && drawnHelpState == helpState && (distanceMoved > 15 || distanceLooked > 30) && fadeTimer == 0)
            helpState = HelpState.JUMP;
        else if (helpState == HelpState.JUMP && drawnHelpState == helpState && fadeTimer == 0 && jumped && mapped)
            helpState = HelpState.CANDY;
        else if (helpState == HelpState.CANDY && candyTimer > 10)
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
        const float width = 400;
        const float height = 80;

        GUIStyle style = GUI.skin.button;
        style.fontSize = 52;
        style.normal.textColor = new Color(1, 1, 1) * 0.8f;
        style.hover.textColor = style.normal.textColor / 0.8f;
        style.active.textColor = style.normal.textColor;
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
            if (Input.GetAxis("Vertical") > 0.1f)
                activeButton = 0;
            else if (Input.GetAxis("Vertical") < -0.1f)
                activeButton = 1;

            if (Input.GetAxis("Accept") != 0 && controller)
            {
                if (activeButton == 0)
                    Application.LoadLevel("Scene1");
                else
                    Application.Quit();
            }

            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Accept") != 0)
                controller = true;
            else if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0 || Input.GetKey(KeyCode.Mouse0))
                controller = false;

            if (activeButton == 0 && controller)
            {
                style.normal.textColor = style.hover.textColor;
                style.fontSize = 42;
            }
            else
            {
                style.normal.textColor = new Color(1, 1, 1) * 0.8f;
                style.fontSize = 32;
            }
            if (GUI.Button(new Rect(Screen.width / 2 - width / 2, Screen.height / 2 + 20, width, height), "Main menu", style))
                Application.LoadLevel("Scene1");

            if (activeButton == 1 && controller)
            {
                style.normal.textColor = style.hover.textColor;
                style.fontSize = 42;
            }
            else
            {
                style.normal.textColor = new Color(1, 1, 1) * 0.8f;
                style.fontSize = 32;
            }
            if (GUI.Button(new Rect(Screen.width / 2 - width / 2, Screen.height / 2 + 70, width, height), "Exit", style))
                Application.Quit();
        }

        float helpHeight = Screen.height / 16.0f;
        if (logic.gameState == GameState.PLAYING)
        {
            style.fontSize = 32;
            style.wordWrap = true;
            GUI.color = new Color(1.0f, 1.0f, 1.0f, (fadeTime - fadeTimer) / fadeTime);
            if (drawnHelpState == HelpState.MOVE)
            {
                GUI.DrawTexture(new Rect(80, 20, helpHeight / moveHelp.height * moveHelp.width, helpHeight), moveHelp);
                GUI.DrawTexture(new Rect(70, 20 + helpHeight, helpHeight / lookHelp.height * lookHelp.width, helpHeight), lookHelp);
            }
            else if (drawnHelpState == HelpState.JUMP)
            {
                GUI.DrawTexture(new Rect(70, 20, helpHeight / jumpHelp.height * jumpHelp.width, helpHeight), jumpHelp);
                GUI.DrawTexture(new Rect(70, 20 + helpHeight, helpHeight / mapHelp.height * mapHelp.width, helpHeight), mapHelp);
            }
            if (drawnHelpState != HelpState.NONE)
            {
                GUI.color = new Color(1.0f, 1.0f, 1.0f);
            }
            style.normal.textColor = GUI.color;
            if (drawnHelpState != HelpState.NONE)
                GUI.Label(new Rect(20, 20 + 2 * helpHeight, 400, 140), "Follow the red arrow. Find the candy and return to where you started from.", style);
        }
    }
}
