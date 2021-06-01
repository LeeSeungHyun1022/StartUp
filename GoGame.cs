using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoGame : MonoBehaviour
{
    public InputField IpInput;
    public InputField PortInput;
    public InputField CreatePortInput;

    public void Create()
    {
        TCP.ip = IpInput.text == "" ? "127.0.0.1" : IpInput.text;
        TCP.port = CreatePortInput.text == "" ? 7777 : int.Parse(CreatePortInput.text);
        SceneManager.LoadScene("InGame");
        TCP.isHost = true;
    }

    public void Join()
    {
        TCP.ip = IpInput.text == "" ? "127.0.0.1" : IpInput.text;
        TCP.port = PortInput.text == "" ? 7777 : int.Parse(PortInput.text);
        SceneManager.LoadScene("InGame");
        TCP.isHost = false;
    }
}
