using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinBrigde : MonoBehaviour
{
    public int spinSpeed;

    public bool dir;

    private void Update()
    {
        if (dir == false)
        {
            gameObject.transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime);
        }
        else
        {
            gameObject.transform.Rotate(Vector3.up * -spinSpeed * Time.deltaTime);
        }
    }
}
