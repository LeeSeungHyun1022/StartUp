using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Client : MonoBehaviour
{
	public GameManager manager;

	public string clientName;

	public bool socketReady;
	TcpClient socket;
	NetworkStream stream;
	StreamWriter writer;
	StreamReader reader;

	public Vector3 pos;
	public Vector3 rot;

	int[,] floors;

	List<Vector3> birgdes = new List<Vector3>();

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
			Debug.Log($"소켓에러 : {e.Message} 연결실패");
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
			clientName = data.Split(';')[1];
			//Debug.Log(data.Split(';')[1]);
			Send("%NAME;"+clientName);
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
			manager.isCreateDone = true;
			if (clientName != data.Split(';')[1])
            {
				Debug.Log("생성해");
				manager.CreateOther();
			}
			return;
		}

        if (data.Split(';')[0].Equals("%Start"))
        {
			manager.GameStart();
			return;
		}

		if (data.Contains("%Disconnect"))
        {
            if (clientName.Equals("0"))
            {
				manager.DeleteOther();
            }
			return;
		}

        if (data.Contains("%Close"))
        {
			if (!clientName.Equals("0"))
			{
				Debug.Log("호스트가 나갔습니다");
				SceneManager.LoadScene("Create");
			}
			return;
		}

		if (data.Contains("%Goal"))
		{
			//Debug.Log(data.Split(';')[1] + data.Split(';')[2]);

			manager.StageEnd(data.Split(';')[1], int.Parse(data.Split(';')[2]));
			return;
		}
		//Debug.Log(data);

		void Floor()
        {
			string[] a = data.Split(';');

			floors = floors = new int[10, 5] {
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

			int cnti = 0; //2차원 배열 만들기 위해 사용
			int cntj = 0;

			for (int i = 1; i < a.Length; i++)
			{
				floors[cnti, cntj] = int.Parse(a[i]);

				cntj++;

				if (i % 5 == 0)
				{
					cnti++;
					cntj = 0;
				}
			}

			manager.CreateFallingFloor(floors);
			return;
		}

        if (data.Split(';')[0].Equals("%ReFloor"))
        {
			manager.ReStart();
			Floor();
			return;
		}

        if (data.Split(';')[0].Equals("%Floor"))
        {
			Floor();
			return;
		}

        if (data.Contains("%Falling"))
        {
			manager.Falling(int.Parse(data.Split(';')[1]));
			return;
		}

        if (data.Contains("%Brigde"))
        {
			string[] a = data.Split(';');
			birgdes.Add(new Vector3(float.Parse(a[1]), float.Parse(a[2]), float.Parse(a[3])));
			if (birgdes.Count == 6)
			{
				manager.CreateSpineBrigde(birgdes);
			}
			return;
		}

        if (data.Contains("%Spin"))
        {
			string[] a = data.Split(';');
			//Debug.Log($"{int.Parse(a[1])}{bool.Parse(a[2])}");
			manager.SpinDir(int.Parse(a[1]),bool.Parse(a[2]));
			return;
		}

        if (data.Split(';')[0].Equals("%Go"))
		{ 
			manager.Respawn();
			manager.CountTime();
			return;
		}

        if (data.Split(';')[0].Equals("%ReStart"))
        {
			manager.ReStart();
			return;
		}

        if (data.Split(';')[0].Equals("%Time"))
        {
			data.Remove(0);
			manager.setTime(data.Split(';'));
			return;
		}

        if (data.Split(';')[0].Equals("%Clear"))
        {
			string[] a = data.Split(';');
			manager.StageEnd(a[1], int.Parse(a[2]));
			manager.Clear();
        }
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
