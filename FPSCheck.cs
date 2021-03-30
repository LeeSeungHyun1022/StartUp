using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCheck : MonoBehaviour
{
    [Range(1, 100)]
    public int fFont_Size;

    float deltaTime = 0.0f;

    private void Start()
    {
        fFont_Size = fFont_Size == 0 ? 50 : fFont_Size;
    }

    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        //지난 프레임이 완료되는 데 걸린 시간으로 점점 올라가다가 같으면 더이상 올라가지 않음
    }

    private void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        // 위치 
        style.alignment = TextAnchor.UpperLeft;
        // 정렬
        style.fontSize = h * 2 / fFont_Size;
        // fFont_Size 클수록 폰트가 작아짐
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0}ms ({1:0.}fps)", msec, fps);
        GUI.Label(rect, text, style);
    }
}
