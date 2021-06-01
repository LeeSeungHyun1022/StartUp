using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingCheck : MonoBehaviour
{
    Ping ping;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StringPing(TCP.ip));
    }

    IEnumerator StringPing(string address)
    {
        while (true)
        {
            ping = new Ping(address);
            while (ping.isDone == false)
            {
                yield return new WaitForSeconds(0.5f);
            }
            //Debug.Log(ping.time);
            yield return new WaitForEndOfFrame();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
