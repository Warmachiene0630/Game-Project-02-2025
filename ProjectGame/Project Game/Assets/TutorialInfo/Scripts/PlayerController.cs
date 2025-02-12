using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] int speed = 5;
    [SerializeField] int jumpSpeed = 5;
    [SerializeField] int gravity = 10;
    [SerializeField] Camera playerCamera;
   
   

    Vector3 moveDir;
    Vector3 playerVelocity;
    bool isGrounded;

    void Update()
    {
        Move();
        Jump();       
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        moveDir = (transform.right * moveX + transform.forward * moveZ) * speed;
        controller.Move(moveDir * Time.deltaTime);

        playerVelocity.y -= gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            playerVelocity.y = jumpSpeed;
        }
    }

}