using UnityEngine;
using System.Collections;

public class MainCameraGUI : MonoBehaviour
{
    public GameObject tupla1;
    public GameObject tupla2;
    
    private GameLogic logic;

    void Start()
    {
        logic = GameObject.FindWithTag(Tags.gameController).GetComponent<GameLogic>();
        tupla1.renderer.enabled = false;
        tupla2.renderer.enabled = false;
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
    }
}
