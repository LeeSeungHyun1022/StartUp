using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    public float speed;
    public float lookSensitivity;

    public float cameraRotationLimit;
    public float currentCameraRotationX;

    Camera camera;
    Rigidbody rigid;

    Vector3 moveVec;

    float hAxis;
    float vAxis;
    float yRota;
    float xRota;
    
    bool jDown;

    bool isJump1;   //점프 후 
    bool isJump2;   //다이빙

    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody>();

        camera = gameObject.GetComponentInChildren<Camera>();
    }


    
    void Update()
    {
        GetInput();
        Move();
        jump();
        Camerarotation();
        CharacterRotation();
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        xRota = Input.GetAxisRaw("Mouse Y"); //위 아래는 x축 회전
        yRota = Input.GetAxisRaw("Mouse X"); //좌 우는 y축 회전
        jDown = Input.GetButtonDown("Jump");
    }

    void Move()
    {
        if (!isJump1)
        {
            Vector3 moveH = transform.right * hAxis;
            Vector3 moveV = transform.forward * vAxis;

            Vector3 velocity = (moveH + moveV).normalized * speed;

            rigid.MovePosition(transform.position + velocity * Time.deltaTime);
        }
    }

    void Camerarotation()
    {
        float cameraRoatationX = xRota * lookSensitivity;

        currentCameraRotationX -= cameraRoatationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
        //clamp 최대 최소를 넘으면 최대 최소로 값을 바꿈
        camera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    void CharacterRotation()
    {
        Vector3 charRotate = new Vector3(0f, yRota, 0f) * lookSensitivity;
        rigid.MoveRotation(rigid.rotation * Quaternion.Euler(charRotate));
    }

    void jump()
    {
        if (jDown && isJump1 && !isJump2)
        {
            rigid.AddForce(transform.forward * 3, ForceMode.Impulse);
            isJump2 = true;
            //다이빙 애니메이션
        }

        if (jDown && !isJump1)
        {
            rigid.AddForce(Vector3.up * 10, ForceMode.Impulse);
            rigid.AddForce(transform.forward * 2, ForceMode.Impulse);
            isJump1 = true;
            //점프 애니메이션
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isJump1 = false;
            isJump2 = false;
            //착지 애니메이션
        }
    }
}
