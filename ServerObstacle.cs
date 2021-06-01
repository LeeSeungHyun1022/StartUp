using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerObstacle : MonoBehaviour
{
    public Server server;

    public GameObject brigd1;
    public GameObject brigd2;
    public GameObject brigd3;
    public GameObject brigd4;
    public GameObject brigd5;
    public GameObject brigd6;

    int[,] floors;

    bool[] p1Clear = { true, true, true, true, true };
    bool[] p2Clear = { false, false, false, false, false };

    bool isDone;

    public void ServerStart()
    {
        StartFallingFloor();
        StartSpinBrigde();
    }

    private void StartSpinBrigde()
    {
        StartCoroutine(RandSpin());
    }

    IEnumerator RandSpin()
    {
        for (; ; )
        {
            float fran = Random.Range(1f, 4f);
            yield return new WaitForSeconds(fran);

            int iran = Random.Range(0, 6); //1~6까지 브릿지
            List<int> a = new List<int>();
            for(int i=0; i < iran; i++)
            {
                if (a.Contains(i))
                {
                    continue;
                }
                a.Add(Random.Range(0, 6));
            }
            int dran = Random.Range(0, 2); //0 왼쪽, 1오른쪽 회전

            if (dran == 0)
                server.SpinB(a, true);
            else
                server.SpinB(a, false);

        }

    }

    public List<Vector3> GetBrigdeRot()
    {
        List<Vector3> a = new List<Vector3>();

        a.Add(brigd1.transform.eulerAngles);
        a.Add(brigd2.transform.eulerAngles);
        a.Add(brigd3.transform.eulerAngles);
        a.Add(brigd4.transform.eulerAngles);
        a.Add(brigd5.transform.eulerAngles);
        a.Add(brigd6.transform.eulerAngles);

        //Debug.Log(a);

        return a;
    }

    public string ReRand()
    {
        isDone = false;
        StartFallingFloor();
        for (; ; )
        {
            if(isDone == true) 
                return GetFloor();
        }
    }

    private void StartFallingFloor()
    {
        floors = new int[10, 5] {
            {0,0,0,0,0 },
            {0,0,0,0,0 },
            {0,0,0,0,0 },
            {0,0,0,0,0 },
            {0,0,0,0,0 },
            {0,0,0,0,0 },
            {0,0,0,0,0 },
            {0,0,0,0,0 },
            {0,0,0,0,0 },
            {0,0,0,0,0 },
        };

        int x = 0;
        int y = Random.Range(0, 5);

        floors[x, y] = 1;


        for (; ; )
        {
            if (x == 9)
                break;

            int ran = Random.Range(0, 3); //0==왼쪽 1==오른쪽 2==위쪽 3==아래쪽       아래쪽으로 할경우 갇히는 경우가 생겨서 무한루프가돔



            if (ran == 0)
            {
                if (y == 0 || floors[x, y - 1] == 1)
                    continue;
                y -= 1;
                floors[x, y] = 1;
            }
            else if (ran == 1)
            {
                if (y == 4 || floors[x, y + 1] == 1)
                    continue;
                y += 1;
                floors[x, y] = 1;
            }
            else if (ran == 2)
            {
                if (x == 9 || floors[x + 1, y] == 1)
                    continue;
                x += 1;
                floors[x, y] = 1;
            }
            /*else if (ran == 3)
            {
                if (x == 0 || floors[x - 1, y] == 1)
                    continue;
                x -= 1;
                floors[x, y] = 1;
            }*/
        }
        isDone = true;
    }
    public string GetFloor()
    {
        string a = "";

        foreach (int floor in floors)
        {
            a += $";{floor}";
        }

        return a;
    }

    public void ClearStage(string clientName, string name)
    {
        if (clientName.Equals("0"))
        {
            p1Clear[int.Parse(name)-1] = true;
        }
        else
        {
            p2Clear[int.Parse(name)-1] = true;
        }
    }

    public bool AllClear(string clientName)
    {
        if (clientName.Equals("0"))
        {
            foreach(bool a in p1Clear)
            {
                if (a != true)
                    return false;
            }
        }
        else
        {
            foreach (bool a in p2Clear)
            {
                if (a != true)
                    return false;
            }
        }
        return true;
    }

    public void ClearReset()
    {
        for (int i = 0; i < 5; i++)
        {
            p1Clear[i] = false;
            p2Clear[i] = false;
        }
    }
}
