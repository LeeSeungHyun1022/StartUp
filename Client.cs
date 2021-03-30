using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Client : MonoBehaviour
{
	public string clientName;

	public bool socketReady;
	TcpClient socket;
	NetworkStream stream;
	StreamWriter writer;
	StreamReader reader;

	public bool isCreate;
	public bool isDelete;

	public Vector3 pos;
	public Vector3 rot;
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
					//Debug.Log($"다른 물체가 움직이고 있습니다 x={temp[2]} y={temp[3]} z={temp[4]}");

					pos = new Vector3(float.Parse(temp[2]), float.Parse(temp[3]), float.Parse(temp[4]));
					rot = new Vector3(float.Parse(temp[5]), float.Parse(temp[6]), float.Parse(temp[7]));

					//manager.other.transform.position = new Vector3(float.Parse(temp[2]), float.Parse(temp[3]), float.Parse(temp[4]));
					//manager.other.transform.eulerAngles = new Vector3(float.Parse(temp[5]), float.Parse(temp[6]), float.Parse(temp[7]));
					//이부분이 문제인데 받아온 값을 나누고 그 값을 vectoer로 만들어야하는

					//Debug.Log(temp[2] +" "+ temp[3] + " " + temp[4] + " " + temp[5] + " " + temp[6] + " " + temp[7]  );
					//Debug.Log(manager.other.transform.position);
					return;
				}
			}
        }
		
        if (data.Contains("%Create"))
        {
			if(clientName != data.Split(';')[1])
            {
				isCreate = true;
			}
        }

		if (data.Contains("%Disconnect"))
        {
            if (clientName.Equals("0"))
            {
				isDelete = true;
            }
        }

        if (data.Contains("%Close"))
        {
			if (!clientName.Equals("0"))
			{
				Debug.Log("호스트가 나갔습니다");
				SceneManager.LoadScene("Create");
			}

		}
		//Debug.Log(data);
		
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

	public void CloseSocket()
	{
		if (!socketReady) return;

		writer.Close();
		reader.Close();
		socket.Close();
		socketReady = false;
	}
}
