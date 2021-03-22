using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject otherPlayerPrefab;

    GameObject player;
    public GameObject other = null;

    public Transform playerCreatePos;
    public Transform otherPos;

    public Server server;
    public Client client;

    bool isConnect;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(TCP.ip +" "+ TCP.port);

        if (TCP.isHost)
        {
            //Debug.Log("서버 생성");
            server.ServerCreate();
            client.ConnectToServer();
        }
        else
        {
            //Debug.Log("서버 참여");
            client.ConnectToServer();
        }

        StartCoroutine(Connect());

        //연결 됐으면
        //플레이어 생성
        
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
            for (int i = 0; int.Parse(client.clientName) > i; i++)
            {
                other = Instantiate(otherPlayerPrefab, otherPos.position, Quaternion.identity);
            }

            player = Instantiate(playerPrefab, playerCreatePos.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("에러 연결 안됨");
        }
    }
    //host 0 다음부터 +1
    //나머지는 다른 플레이어를 움직이도록 설정

    // Update is called once per frame
    void Update()
    {
        if (isConnect)
        {
            client.Send($"%Position;{client.clientName};{player.transform.position.x};{player.transform.position.y};{player.transform.position.z};" +
                $"{player.transform.eulerAngles.x};{player.transform.eulerAngles.y};{player.transform.eulerAngles.z}");
        }
    }

    public void CreateOther()
    {
        other = Instantiate(otherPlayerPrefab, otherPos.position, Quaternion.identity);
    }
}
