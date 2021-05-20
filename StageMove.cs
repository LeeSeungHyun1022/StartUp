using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMove : MonoBehaviour
{
    public Transform pos;

    BoxCollider trigger;

    float limit;

    private void Start()
    {
        trigger = gameObject.GetComponent<BoxCollider>();

        limit = gameObject.transform.position.y + 10f;
    }

    public void StageEnd()
    {
        StartCoroutine(StageFloorMove());
    }

    IEnumerator StageFloorMove()
    {
        Debug.Log(gameObject.transform.position.y);
        trigger.isTrigger = false;
        for (; ; )
        {
            if (gameObject.transform.position.y >= limit)
            {
                break;
            }
            gameObject.transform.position += Vector3.up * Time.deltaTime;
            Debug.Log(gameObject.transform.position.y);
            yield return new WaitForSeconds(0.02f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (trigger.isTrigger)
        {
            if (other.gameObject.tag.Equals("Player"))
            {
                other.transform.rotation = pos.rotation;
                other.transform.position = pos.position;
            }
        }
    }
}
