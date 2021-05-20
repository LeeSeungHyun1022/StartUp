using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalseFloorEvent : MonoBehaviour
{
    [SerializeField] private Client client;

    public int floorName;

    private void Start()
    {
        client = GameObject.Find("Client").GetComponent<Client>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player") || other.gameObject.tag.Equals("Other"))
        {
            client.Send($"%Falling;{floorName}");
        }
    }

    public void FallingFloor()
    {
        StartCoroutine(Falling());
    }

    IEnumerator Falling()
    {
        for (int i=0 ; i < 60 ; i++)
        {
            gameObject.transform.position -= Vector3.up * Time.deltaTime;
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(this.gameObject);
    }
}
