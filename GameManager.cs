using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{ 
    public GameObject playerPrefab;
    public GameObject otherPlayerPrefab;

    GameObject player;
    public GameObject other;

    public Transform player1LobbyPos;
    public Transform player2LobbyPos;
    public Transform player1CreatePos;
    public Transform player2CreatePos;

    public StageMove[] player1Stage;
    public StageMove[] player2Stage;

    public GameObject trueFloor;
    public GameObject falseFloor;
    public Transform[] floorsPos;


    public Transform[] brigdesRot;
    public SpinBrigde[] brigdes;

    public Server server;
    public Client client;

    public ServerObstacle obstacle;

    public Text countDownTxt;

    bool isConnect;
    bool isOtherCreate;
    public bool isCreateDone;

    Dictionary<int, FalseFloorEvent> dicFalse;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(TCP.ip +" "+ TCP.port);


        Screen.SetResolution(800, 600, false, 60);

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        if (TCP.isHost)
        {
            server.ServerCreate();
            obstacle.ServerStart();
        }
        client.ConnectToServer();
        StartCoroutine(CreateMap());
    }


    IEnumerator CreateMap()
    {
        for (; ; )              //서버에서 create를 받아오면
        {
            if (isCreateDone || TCP.isHost)
            {
                isCreateDone = false;
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        //Debug.Log("로딩끝");
        client.Send("%Floor");
        for (; ; )
        {
            if (isCreateDone)
            {
                isCreateDone = false;
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        client.Send("%Brigde");
        for (; ; )
        {
            if (isCreateDone)
            {
                isCreateDone = false;
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        StartCoroutine(Connect());
    }

    IEnumerator Connect()
    {
        for (; ; )

        {
            if (client.clientName != "-1")
            {
                isConnect = true;
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        if (isConnect)
        {
            if (client.clientName.Equals("0"))
            {
                player = Instantiate(playerPrefab, player1LobbyPos.position, Quaternion.identity);
            } else
            {
                player = Instantiate(playerPrefab, player2LobbyPos.position, Quaternion.identity);
                other = Instantiate(otherPlayerPrefab, player1LobbyPos.position, Quaternion.identity);
                isOtherCreate = true;
                StartCoroutine(Creating());
            }

        }
        else
        {
            Debug.Log("에러 연결 안됨");
        }
    }
    //host 0 다음부터 +1
    //나머지는 다른 플레이어를 움직이도록 설정

    // Update is called once per frame
    bool frame;

    void Update()
    {
        if (isConnect && frame)
        {
            client.Send($"%Position;{client.clientName};{player.transform.position.x};{player.transform.position.y};{player.transform.position.z};" +
                $"{player.transform.eulerAngles.x};{player.transform.eulerAngles.y};{player.transform.eulerAngles.z}");
        }

        if (isOtherCreate && frame)
        {
            other.transform.position = client.pos;
            other.transform.eulerAngles = client.rot;
        }
        frame = !frame;

        if (Input.GetKey("escape"))
        {
            client.CloseSocket();
        }

        if (Input.GetKey("f5"))
        {
            Respawn();
        }


        if (!client.socketReady)
        {
            SceneManager.LoadScene("Create");
        }
    }

    public void CreateOther()
    {
        other = Instantiate(otherPlayerPrefab, player2LobbyPos.position, Quaternion.identity);
        StartCoroutine(Creating());
    }

    public void DeleteOther()
    {
        Destroy(other);
    }


    public void Respawn()
    {
        if (client.clientName.Equals("0"))
        {
            player.transform.position = player1CreatePos.position;
            player.transform.rotation = Quaternion.identity;
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        else
        {
            player.transform.position = player2CreatePos.position;
            player.transform.rotation = Quaternion.identity;
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }

    IEnumerator Creating()
    {
        for(; ; )
        {
            if (other != null)
            {
                isOtherCreate = true;
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        isCreateDone = true;
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(0.3f);
        other.GetComponent<BoxCollider>().enabled = true;
    }

    public void GameStart()
    {
        Debug.Log("게임시작");//게임 시작
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        countDownTxt.enabled = true ;
        for (int i = 3; i >= 0; i--)
        {
            countDownTxt.text = i.ToString();
            Debug.Log(countDownTxt.text);
            yield return new WaitForSeconds(1f);
        }
        countDownTxt.text = null;

        client.Send("%Start");
    }

    public void CreateFallingFloor(int[,] floors)
    {
        int cnt = 0;

        dicFalse = new Dictionary<int, FalseFloorEvent>();

        foreach (int floor in floors)
        {
            if (floor == 0)
            {
                FalseFloorEvent a = Instantiate(falseFloor, floorsPos[cnt].position, floorsPos[cnt].rotation).GetComponent<FalseFloorEvent>();
                a.floorName = cnt;
                dicFalse.Add(cnt, a);
            }

            if (floor == 1)
            {
                Instantiate(trueFloor, floorsPos[cnt].position, floorsPos[cnt].rotation);
            }

            cnt++;
        }

        isCreateDone = true;
    }



    public void CreateSpineBrigde(List<Vector3> brigdes)
    {
        for(int i=0; i<6; i++)
        {
            brigdesRot[i].eulerAngles = brigdes[i];
        }

        isCreateDone = true;
    }

    public void SpinDir(int id, bool dir)
    {
        brigdes[id].dir = dir;
    }

    public void Falling(int name)
    {
        dicFalse[name].FallingFloor();
    }

    public void StageEnd(string name, int stage) 
    {
        if (name.Equals("0"))
        {
            player1Stage[stage-1].StageEnd();
        }
        else
        {
            player2Stage[stage-1].StageEnd();
        }

        if (client.clientName.Equals(name))
        {
            Respawn();
        }
    }
}
