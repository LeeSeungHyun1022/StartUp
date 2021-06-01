using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearTime : MonoBehaviour
{
    public TextMesh[] p1S;

    public TextMesh[] p2S;

    float totaltime;
    float time;

    public void Reset()
    {
        foreach (TextMesh p1 in p1S)
        {
            p1.text = "00.0";
        }
        foreach (TextMesh p2 in p2S)
        {
            p2.text = "00.0";
        }
    }

    public void ReCount()
    {
        totaltime += time;
        time = 0;
    }

    public float getTotalTime()
    {
        return totaltime;
    }

    public void StageStart()
    {
        StopAllCoroutines();
        totaltime = 0;
        time = 0;
        StartCoroutine(CountSec());
    }

    IEnumerator CountSec()
    {
        for(; ; )
        {
            time += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void Player1(int stage)
    {
        if (stage == 5)
        {
            p1S[stage].text = string.Format("{0:N1}", totaltime);
        }
        else
        {
            p1S[stage].text = string.Format("{0:N1}", time);
        }
        ReCount();
    }
    public void Player2(int stage)
    {
        if (stage == 5)
        {
            p2S[stage].text = string.Format("{0:N1}", totaltime);
        }
        else
        {
            p2S[stage].text = string.Format("{0:N1}", time);
        }
        ReCount();
    }

    public string UpdateTime()
    {
        string a="", b="";
        foreach(TextMesh p1 in p1S)
        {
            a += ";" + p1.text;
        }
        foreach(TextMesh p2 in p2S)
        {
            b += ";" + p2.text;
        }

        return a + b;
    }
}
