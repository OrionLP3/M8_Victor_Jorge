using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;
    public Joystick joystick;
    public Button jumpButton; 
    public float runSpeed = 40f;

    float horizontalMove = 0f;
    bool jump = false;

    void Start()
    {
        // Check if the jump button is not null before adding a listener
        if (jumpButton != null)
        {
            jumpButton.onClick.AddListener(Jump);
        }
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = joystick.Horizontal * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
    }

    void Jump()
    {
        jump = true;
        animator.SetBool("IsJumping", true);
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    void FixedUpdate()
    {
        // Move our character
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
    }
}
