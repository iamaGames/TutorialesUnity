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

public class PlayerMovement : MonoBehaviour
{
    //Components or load on start
    GamepadControls controls;
    Rigidbody rb;
    public Transform camera;

    public float speed = 10f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    //Jump settings
    public float jumpForce = 200f;
    public bool jump;

    public Transform groundCheck;
    public float groundDistance = 0.4f; //Radius sphere
    public LayerMask groundMask;
    bool isGrounded;

    //Animations
    public Animator myAnimator;
    private bool isMoving;

    Vector2 move;
    Vector2 rotate;
    private void Awake()
    {
        controls = new GamepadControls(); //Creates a new GamepadControls object
        rb = GetComponent<Rigidbody>();
        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>(); //Get joystic direction
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero; //Reset joystick direction

        controls.Gameplay.Rotate.performed += ctx => rotate = ctx.ReadValue<Vector2>(); //Get right joystick direction
        controls.Gameplay.Rotate.canceled += ctx => rotate = Vector2.zero;

        controls.Gameplay.Jump.performed += ctx => SetJump(true);
    }

    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        Vector3 direction = new Vector3(move.x, 0f, move.y).normalized;
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            transform.position += moveDir.normalized * speed * Time.deltaTime;
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        myAnimator.SetBool("isMoving", isMoving); //Tell the animator we are moving
        Jump();
    }

    private void Jump()
    {
        if(isGrounded && jump)
        {
            SetJump(false);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    private void SetJump(bool jumpValue)
    {
        jump = jumpValue;
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
