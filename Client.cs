using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;

public class Client : MonoBehaviour
{
	GameManager manager;

	public string clientName;

	bool socketReady;
	TcpClient socket;
	NetworkStream stream;
	StreamWriter writer;
	StreamReader reader;

	bool isCreate;

	public void ConnectToServer()
	{
		// 이미 연결되었다면 함수 무시
		if (socketReady) return;

		// 소켓 생성
		try
		{
			socket = new TcpClient(TCP.ip, TCP.port);
			stream = socket.GetStream();
			writer = new StreamWriter(stream);
			reader = new StreamReader(stream);
			socketReady = true;
		}
		catch (Exception e)
		{
			Debug.Log($"소켓에러 : {e.Message}");
		}
	}

	void Update()
	{
		if (socketReady && stream.DataAvailable)
		{
			string data = reader.ReadLine();
			if (data != null)
				OnIncomingData(data);
		}
	}

	void OnIncomingData(string data)
	{
		if (data.Contains("%NAME"))
		{
			clientName = data.Split('l')[1];
			//Debug.Log(data.Split('l')[1]);
			Send("%NAMEl"+clientName);
			return;
		} 
        if (data.Contains("%Position"))
        {
			if(clientName != data.Split(';')[1])
            {
				//if (isCreate)
				{
					string[] temp = data.Split(';');
					Debug.Log("다른 물체가 움직이고 있습니다");
					//manager.other.transform.position = new Vector3(float.Parse(temp[2]), float.Parse(temp[3]), float.Parse(temp[4]));
					//manager.other.transform.eulerAngles = new Vector3(float.Parse(temp[5]), float.Parse(temp[6]), float.Parse(temp[7]));
					//이부분이 문제인데 받아온 값을 나누고 그 값을 vectoer로 만들어야하는데
					
					Debug.Log(temp[2] +" "+ temp[3] + " " + temp[4] + " " + temp[5] + " " + temp[6] + " " + temp[7]  );
					//Debug.Log(manager.other.transform.position);
					return;
				}
			}
        }
		
        if (data.Contains("%Create"))
        {
			if(clientName != data.Split(';')[1])
            {
				StopCoroutine(Creating());
				StartCoroutine(Creating());
				return;
			}
        }
		//Debug.Log(data);
		
	}

	IEnumerator Creating()
    {
		manager.CreateOther();
        while (true)
        {
			yield return new WaitForSeconds(0.1f);
			if (manager.other != null)
				break;
		}
		isCreate = true;
    }

	public void Send(string data)
	{
		if (!socketReady) return;

		writer.WriteLine(data);
		writer.Flush();
	}
	/*
	public void OnSendButton(InputField SendInput)
	{
#if (UNITY_EDITOR || UNITY_STANDALONE)
		if (!Input.GetButtonDown("Submit")) return;
		SendInput.ActivateInputField();
#endif
		if (SendInput.text.Trim() == "") return;

		string message = SendInput.text;
		SendInput.text = "";
		Send(message);
	}
	*/

	void OnApplicationQuit()
	{
		CloseSocket();
	}

	void CloseSocket()
	{
		if (!socketReady) return;

		writer.Close();
		reader.Close();
		socket.Close();
		socketReady = false;
	}
}
