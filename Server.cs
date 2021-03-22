using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class Server : MonoBehaviour
{

    List<ServerClient> clients;
    List<ServerClient> disconnectList;

    TcpListener server;
    bool serverStarted;

    int cntClinet = 0;

    public void ServerCreate()
    {
        clients = new List<ServerClient>();
        disconnectList = new List<ServerClient>();

        try
        {
            server = new TcpListener(IPAddress.Any, TCP.port);
            server.Start();

            StartListening();
            serverStarted = true;
            Debug.Log($"서버가 {TCP.port}에서 시작되었습니다.");
        }
        catch (Exception e)
        {
            Debug.Log($"소켓 에러: {e.Message}");
        }
    }

    void Update()
    {
        if (!serverStarted) return;

        foreach (ServerClient c in clients)
        {
            // 클라이언트가 여전히 연결되있나?
            if (!IsConnected(c.tcp))
            {
                c.tcp.Close();
                disconnectList.Add(c);
                continue;
            }
            // 클라이언트로부터 체크 메시지를 받는다
            else
            {
                NetworkStream s = c.tcp.GetStream();
                if (s.DataAvailable)
                {
                    string data = new StreamReader(s, true).ReadLine();
                    if (data != null)
                        OnIncomingData(c, data);
                }
            }
        }

        for (int i = 0; i < disconnectList.Count - 1; i++)
        {
            Broadcast($"{disconnectList[i].clientName} 연결이 끊어졌습니다", clients);

            clients.Remove(disconnectList[i]);
            disconnectList.RemoveAt(i);
        }
    }



    bool IsConnected(TcpClient c)
    {
        try
        {
            if (c != null && c.Client != null && c.Client.Connected)
            {
                if (c.Client.Poll(0, SelectMode.SelectRead))
                    return !(c.Client.Receive(new byte[1], SocketFlags.Peek) == 0);

                return true;
            }
            else
                return false;
        }
        catch
        {
            return false;
        }
    }

    void StartListening()
    {
        server.BeginAcceptTcpClient(AcceptTcpClient, server);
    }

    void AcceptTcpClient(IAsyncResult ar)
    {
        TcpListener listener = (TcpListener)ar.AsyncState;
        clients.Add(new ServerClient(listener.EndAcceptTcpClient(ar)));
        StartListening();

        // 메시지를 연결된 모두에게 보냄
        Broadcast("%NAMEl"+(cntClinet++).ToString(), new List<ServerClient>() { clients[clients.Count - 1] });
    }


    void OnIncomingData(ServerClient c, string data)
    {
        if (data.Contains("%NAME"))
        {
            //Debug.Log(data);
            c.clientName = data.Split('l')[1];
            Broadcast($"%Create;{c.clientName}", clients);
            return;
        }

        if (data.Contains("%Position"))
        {
            string[] datas = data.Split(';');
            //Debug.Log(data.Split(';')[1] + " " + data.Split(';')[2]+" " + data.Split(';')[3]);
            Broadcast($"%Position;{datas[1]};{datas[2]};{datas[3]};{datas[4]};{datas[5]};{datas[6]};{datas[7]}", clients);
            //  클라이언트 번호 1                     포지션 xyz 234                         방향 xyz 567
            //Debug.Log($"%Position;{datas[1]};{datas[2]};{datas[3]};{datas[4]};{datas[5]};{datas[6]};{datas[7]}");
            return;
        }

        Broadcast($"{c.clientName}:{data}", clients);
    }
    // "
    void Broadcast(string data, List<ServerClient> cl)
    {
        foreach (var c in cl)
        {
            try
            {
                StreamWriter writer = new StreamWriter(c.tcp.GetStream());
                writer.WriteLine(data);
                writer.Flush();
            }
            catch (Exception e)
            {
                Debug.Log($"쓰기 에러 : {e.Message}를 클라이언트에게 {c.clientName}");
            }
        }
    }
}


public class ServerClient
{
    public TcpClient tcp;
    public string clientName;

    public ServerClient(TcpClient clientSocket)
    {
        clientName = "Guest";
        tcp = clientSocket;
    }
}
