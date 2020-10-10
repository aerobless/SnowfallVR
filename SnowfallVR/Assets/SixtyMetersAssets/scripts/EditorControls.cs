using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorControls : MonoBehaviour
{
    public float speed = 5;
    public float gravity = -5;

    float velocityY = 0;

    CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        velocityY += gravity * Time.deltaTime;

        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        input = input.normalized;

        Vector3 temp = Vector3.zero;
        if (input.z == 1)
        {
            temp += transform.forward;
        }
        else if (input.z == -1)
        {
            temp += transform.forward * -1;
        }

        if (input.x == 1)
        {
            temp += transform.right;
        }
        else if (input.x == -1)
        {
            temp += transform.right * -1;
        }

        Vector3 velocity = temp * speed;
        velocity.y = velocityY;

        controller.Move(velocity * Time.deltaTime);

        if (controller.isGrounded)
        {
            velocityY = 0;
        }
    }
}