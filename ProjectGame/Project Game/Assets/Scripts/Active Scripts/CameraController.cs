using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] int sens;
    [SerializeField] int lockVertMin;
    [SerializeField] int lockVertMax;
    [SerializeField] bool invertY;

    float rotX;

    public int getSens()
    {
        return sens;
    }

    public void setSens(int newSens)
    {
        sens = newSens;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // get input
        float mouseX = Input.GetAxis("Mouse X") * sens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sens * Time.deltaTime;

        // tie the mouseY to rotate the camera's X - look up and down
        if (invertY)
            rotX += mouseY;
        else
            rotX -= mouseY;

        // clamp the camera on the x-axis
        rotX = Mathf.Clamp(rotX, lockVertMin, lockVertMax);

        // rotate the camera on the x-axis
        transform.localRotation = Quaternion.Euler(rotX, 0, 0);

        // rotate the player on the y-axis - look left and right
        transform.parent.Rotate(Vector3.up * mouseX);

    }
}