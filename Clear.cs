using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clear : MonoBehaviour
{
    public Transform pos;

    public Client client;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            client.Send($"%Goal;" + client.clientName + ";6");

            other.transform.rotation = pos.rotation;
            other.transform.position = pos.position;
        }
    }
}
