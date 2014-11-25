﻿using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour 
{
    void OnGUI()
    {
        DrawMenu();
	}

    void DrawMenu()
    {
        const float width = 200;
        const float height = 40;
        const float margin = 10;

        GUIStyle style = GUI.skin.button;
        style.fontSize = 36;
        style.normal.textColor = new Color(1, 1, 1);
        style.hover.textColor = style.normal.textColor * 0.8f;
        style.active.textColor = style.normal.textColor * 0.6f;

        GUI.backgroundColor = Color.clear;

        if (GUI.Button(new Rect(Screen.width / 2 - width / 2, Screen.height / 2 - height - margin, width, height), "Play", style))
		{
			GameObject.FindWithTag("GameController").GetComponent<GameLogic>().StartGame();
            gameObject.SetActive(false);
		}
        if (GUI.Button(new Rect(Screen.width / 2 - width / 2, Screen.height / 2 + margin, width, height), "Exit", style))
            Application.Quit();
    }
}
