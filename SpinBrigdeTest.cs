using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinBrigdeTest : MonoBehaviour
{
    public GameObject[] brigdes;

    bool[] dirs;

    Coroutine[] coroutines;

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            coroutines[i] = StartCoroutine(SpinB(brigdes[i], dirs[i]));
            StartCoroutine(RandSpin());
        }

        //StartCoroutine()
    }

    IEnumerator SpinB(GameObject brigde, bool dir)
    {
        for (; ; )
        {
            if (dir == false)
            {
                brigde.transform.Rotate(Vector3.right * 10 * Time.deltaTime);
            }
            else
            {
                brigde.transform.Rotate(Vector3.right * -10 * Time.deltaTime);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator RandSpin()
    {
        for (; ; )
        {
            float fran = Random.Range(1f, 3f);
            yield return new WaitForSeconds(fran);

            int iran = Random.Range(0, 2);

            dirs[iran] = true;



            for (int i = 0; i < 3; i++)
            {
                StopCoroutine(coroutines[i]);
            }
            for (int i = 0; i < 3; i++)
            {
                coroutines[i] = StartCoroutine(SpinB(brigdes[i], dirs[i]));
            }


        }

    }
}
