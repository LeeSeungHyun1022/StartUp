using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seesaw : MonoBehaviour
{
    Vector3 seesawPos;

    Vector3 playerPos = Vector3.zero;
    Vector3 otherPos = Vector3.zero;

    float a, b;

    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        seesawPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(transform.eulerAngles.z);
        //Debug.Log(gameObject.transform.rotation.z);
        if (playerPos == Vector3.zero && otherPos == Vector3.zero)
        {
            

            if (transform.eulerAngles.z < 100 && transform.eulerAngles.z > 1)
            {
                transform.Rotate(Vector3.forward * -10 * Time.deltaTime);
                //Debug.Log("오른쪽");
            }
            else if (transform.eulerAngles.z < 359 && transform.eulerAngles.z > 260)
            {
                transform.Rotate(Vector3.forward * 10 * Time.deltaTime);
                //Debug.Log("왼쪽");
            
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            return;
        }

        

        if(playerPos != Vector3.zero)
        {
            a = seesawPos.x - playerPos.x;
        }
        if(otherPos != Vector3.zero)
        {
            b = seesawPos.x - otherPos.x;
        }

        if(a + b > 0)
        {
            transform.Rotate(Vector3.forward * 20 * Time.deltaTime);

        }
        else if(a + b < 0)
        {
            transform.Rotate(Vector3.forward * -20 * Time.deltaTime);

        }
    }

    private void OnTriggerStay(Collider other)
    {
        StopAllCoroutines();

        if (other.gameObject.tag.Equals("Player"))
        {
            playerPos = new Vector3(other.transform.position.x, 0, 0);
            
            //Debug.Log("플레이어가 밟음");
        }
        else if (other.gameObject.tag.Equals("Other"))
        {
            otherPos = new Vector3(other.transform.position.x, 0, 0);
            //Debug.Log("플레이어가 밟음");
        }
    }

    IEnumerator Delay(Collider obj)
    {
        yield return new WaitForSeconds(0.3f);
        if (obj.gameObject.tag.Equals("Player"))
        {
            playerPos = Vector3.zero;
        }
        else if (obj.gameObject.tag.Equals("Other"))
        {
            otherPos = Vector3.zero;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        StartCoroutine(Delay(other));
    }
}
