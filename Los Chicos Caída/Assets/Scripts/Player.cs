using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Scripting.APIUpdating;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Player : MonoBehaviour
{

    GamepadControls controls;
    public CharacterController controller;
    public Transform camera;

    public float speed = 10f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    //Jump settings
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    private Vector3 velocity;
    private bool jump = false;

    public Transform groundCheck;
    public float groundDistance = 0.4f; //Radius sphere
    public LayerMask groundMask;
    bool isGrounded;

    Vector2 move;
    Vector2 rotate;
    private void Awake()
    {
        controls = new GamepadControls(); //Creates a new GamepadControls object

        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>(); //Get joystic direction
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero; //Reset joystick direction

        controls.Gameplay.Rotate.performed += ctx => rotate = ctx.ReadValue<Vector2>(); //Get right joystick direction
        controls.Gameplay.Rotate.canceled += ctx => rotate = Vector2.zero;

        controls.Gameplay.Jump.performed += ctx => SetJump(true);
        controls.Gameplay.Jump.canceled += ctx => SetJump(false);
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        Vector3 direction = new Vector3(move.x, 0f, move.y).normalized;
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        Jump();
    }

    private void Jump()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        if(jump && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
    private void SetJump(bool jumpValue)
    {
        jump = jumpValue;
    }

    //Check if an object has collided with player
    public bool CollisionWithPlayer()
    {
        return true;
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }
}
