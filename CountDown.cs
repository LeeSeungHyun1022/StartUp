using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDown : MonoBehaviour
{
    public GameManager manager;

    [Range(1, 100)]
    public int fFont_Size;

    [Range(1, 100)]
    public int rection;

    void Start()
    {
        fFont_Size = fFont_Size == 0 ? 50 : fFont_Size;
    }

    // Update is called once per frame
    private void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / rection);
        // 위치 
        style.alignment = TextAnchor.MiddleCenter;
        // 정렬
        style.fontSize = h * 2 / fFont_Size;

        string text;

        /*if (manager.countdown.Equals("0"))
        {
            text = string.Format("Go");
        }
        else if (manager.countdown.Equals("1"))
        {
            text = string.Format("1");
        }
        else if (manager.countdown.Equals("2"))
        {
            text = string.Format("2");
        }
        else if (manager.countdown.Equals("3"))
        {
            text = string.Format("3");
        }
        else
        {
            return;
        }

        GUI.Label(rect, text, style);*/
    }
}
