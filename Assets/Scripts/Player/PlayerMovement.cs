using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public float speed = 100f;
    private float singleSpeed;

    private Vector3 move;

    public float mouseSensitivity = 5f;
    public float minY = -90f;
    public float maxY = 90f;
    private float rotationX = 0f;
    private float rotationY = 0f;

    Quaternion originalRotation;

    void Start()
    {
        singleSpeed = speed;
        Cursor.lockState = CursorLockMode.Locked;
        originalRotation = transform.rotation;
    }

    void Update()
    {

        // if(Input.GetKeyDown(KeyCode.P))
        // {
        //     if(GameIsPaused)
        //     {
        //         Resume();
        //     }
        //     else
        //     {
        //         Pause();
        //     }
        // }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        rotationX += Input.GetAxis("Mouse X") * mouseSensitivity;
        rotationY += Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationY = Mathf.Clamp(rotationY, minY, maxY);

        Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, Vector3.left);
        transform.rotation = originalRotation * xQuaternion * yQuaternion;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            float doubleSpeed = speed * 4;
            singleSpeed = doubleSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            float semiSpeed = speed/ 4;
            singleSpeed = semiSpeed;
        }
        else
        {
            singleSpeed = speed;
        }
        move = transform.forward * z;
        move += transform.right * x;
        transform.position += move * singleSpeed * Time.deltaTime;
    }


    void Pause()
    {
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    void Resume()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

}
