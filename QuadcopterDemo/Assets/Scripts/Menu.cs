using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour 
{
    Texture logo;
    Font font;
    int activeButton = 0;
    bool controller = false;

    void Start()
    {
        logo = (Texture)Resources.Load("GUI/Logo", typeof(Texture));
        font = Resources.Load<Font>("Fonts/Corbel Regular");
    }

    void Update()
    {
        if (Input.GetAxis("Vertical") > 0.1f)
            activeButton = 0;
        else if (Input.GetAxis("Vertical") < -0.1f)
            activeButton = 1;

        if (Input.GetAxis("Accept") != 0 && controller)
        {
            if (activeButton == 0)
                StartGame();
            else 
                Application.Quit();
        }

        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Accept") != 0)
            controller = true;
        else if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0 || Input.GetKey(KeyCode.Mouse0))
            controller = false;
    }

    void OnGUI()
    {
        DrawMenu();
	}

    void DrawMenu()
    {
        const float width = 200;
        const float height = 50;

        GUIStyle style = GUI.skin.button;
        style.fontSize = 32;
        style.normal.textColor = new Color(1, 1, 1) * 0.8f;
        style.hover.textColor = style.normal.textColor / 0.8f;
        style.active.textColor = style.normal.textColor;
        style.font = font;
        style.fontStyle = FontStyle.Bold;

        GUI.backgroundColor = Color.clear;

        float logoSize = Screen.height * 0.5f;
        GUI.DrawTexture(new Rect(Screen.width / 2 - logoSize / 2, Screen.height / 2 - logoSize * 0.7f, logoSize, logoSize), logo);

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

        if (GUI.Button(new Rect(Screen.width / 2 - width / 2, Screen.height / 2 + logoSize * 0.3f + 10, width, height), "Play", style))
            StartGame();

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

        if (GUI.Button(new Rect(Screen.width / 2 - width / 2, Screen.height / 2 + logoSize * 0.3f + 60, width, height), "Exit", style))
            Application.Quit();
    }

    void StartGame()
    {
        GameObject.FindWithTag(Tags.gameController).GetComponent<GameLogic>().StartGame();
        gameObject.SetActive(false);
    }
}
