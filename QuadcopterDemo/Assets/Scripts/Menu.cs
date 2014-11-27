using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour 
{
    Texture logo;
    Font font;

    void Start()
    {
        logo = (Texture)Resources.Load("GUI/Logo", typeof(Texture));
        font = Resources.Load<Font>("Fonts/Corbel Regular");
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
        style.normal.textColor = new Color(1, 1, 1);
        style.hover.textColor = style.normal.textColor * 0.8f;
        style.active.textColor = style.normal.textColor * 0.6f;
        style.font = font;
        style.fontStyle = FontStyle.Bold;

        GUI.backgroundColor = Color.clear;

        GUI.DrawTexture(new Rect(Screen.width / 2 - 200 / 2, Screen.height / 2 - 400 / 2, 200, 200), logo);

        if (GUI.Button(new Rect(Screen.width / 2 - width / 2, Screen.height / 2 + 20, width, height), "Play", style))
		{
			GameObject.FindWithTag(Tags.gameController).GetComponent<GameLogic>().StartGame();
            gameObject.SetActive(false);
		}
        if (GUI.Button(new Rect(Screen.width / 2 - width / 2, Screen.height / 2 + 70, width, height), "Exit", style))
            Application.Quit();
    }
}
