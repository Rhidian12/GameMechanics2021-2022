using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputActionAsset playerControls;

    [SerializeField] private float m_JumpStrength;
    [SerializeField] private float m_Speed;

    private Vector3 m_Direction;
    private Rigidbody m_RigidBody;

    private InputAction m_Movement;
    private InputAction m_Jump;

    private void Awake()
    {
        m_RigidBody = gameObject.GetComponent<Rigidbody>();

        var playerActionMap = playerControls.FindActionMap("Player");

        m_Movement = playerActionMap.FindAction("Movement");
        m_Movement.started += OnMovement;
        m_Movement.Enable();

        m_Jump = playerActionMap.FindAction("Jump");
        m_Jump.performed += OnJump;
        m_Jump.Enable();
    }

    private void FixedUpdate()
    {
        m_RigidBody.MovePosition(m_Direction);

        m_Direction += Physics.gravity * Time.deltaTime;

        if (m_Direction.y < 0f)
            m_Direction.y = 0f;

        //Debug.Log(m_Direction);
    }

    private void OnMovement(InputAction.CallbackContext context)
    {
        if (context.control.name.Equals("w"))
        {
            Debug.Log("Forward!");
            m_Direction.z += m_Speed * Time.deltaTime;
        }
        else if (context.control.name.Equals("d"))
        {
            Debug.Log("Right!");
            m_Direction.x += m_Speed * Time.deltaTime;
        }
        else if (context.control.name.Equals("s"))
        {
            Debug.Log("Backwards!");
            m_Direction.z -= m_Speed * Time.deltaTime;
        }
        else if (context.control.name.Equals("a"))
        {
            Debug.Log("Left!");
            m_Direction.x -= m_Speed * Time.deltaTime;
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump!");
        m_Direction.y += m_JumpStrength * Time.deltaTime;
    }
}
