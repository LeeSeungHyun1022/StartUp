﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject otherPlayerPrefab;

    GameObject player;
    public GameObject other;

    public Transform player1CreatePos;
    public Transform player2CreatePos;

    public Server server;
    public Client client;

    bool isConnect;
    bool isOtherCreate;


    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(TCP.ip +" "+ TCP.port);


        Screen.SetResolution(800, 600, false, 60);

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;

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
            if (client.clientName.Equals("0"))
            {
                player = Instantiate(playerPrefab, player1CreatePos.position, Quaternion.identity);
            }else
            {
                player = Instantiate(playerPrefab, player2CreatePos.position, Quaternion.identity);
                other = Instantiate(otherPlayerPrefab, player1CreatePos.position, Quaternion.identity);
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

        if (client.isCreate)
        {
            client.isCreate = false;
            other = Instantiate(otherPlayerPrefab, player2CreatePos.position, Quaternion.identity);
            StartCoroutine(Creating());
        }

        if (client.isDelete)
        {
            client.isDelete = false;
            isOtherCreate = false;
            Destroy(other); 
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

    public void Respawn()
    {
        player.transform.position = player1CreatePos.position;
        player.transform.rotation = Quaternion.identity;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
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
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(0.3f);
        other.GetComponent<BoxCollider>().enabled = true;
    }
}
