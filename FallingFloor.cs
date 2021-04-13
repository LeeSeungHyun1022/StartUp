using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingFloor : MonoBehaviour
{
    public GameObject trueFloor;
    public GameObject falseFloor;

    public Transform[] floorsPos;

    int[,] floors;

    // 0은 비어있는거 1은 밟을 수 잇는거

    private void Start()
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

        //[0][5], [1][5] ~~~~ [10][5]
        // rand 0~9 사이로 x값에 넣고
        // x + 1 or x - 1 or x + 10 or x - 10 하는데 있을경우 다시 돌리기
        // 

        int x = 0;
        int y = Random.Range(0, 5);

        floors[x, y] = 1;

        Debug.Log(x + " " + y);

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

        int cnt = 0;

        foreach (int floor in floors)
        {
            if(floor == 0)
            {
                Instantiate(falseFloor, floorsPos[cnt].position, floorsPos[cnt].rotation);
            }

            if(floor == 1)
            {
                Instantiate(trueFloor, floorsPos[cnt].position, floorsPos[cnt].rotation);
            }

            cnt++;
        }
    }
}
