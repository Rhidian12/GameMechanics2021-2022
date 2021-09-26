using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputActionAsset playerControls;

    [SerializeField] private float m_JumpStrength;
    [SerializeField] private float m_Speed;

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

    private void OnMovement(InputAction.CallbackContext context)
    {
        if (context.control.name.Equals("w"))
        {
            Debug.Log("Forward!");
            Vector3 direction = new Vector3(0f, 0f, m_Speed);
            m_RigidBody.AddForce(direction, ForceMode.VelocityChange);
        }
        else if (context.control.name.Equals("d"))
        {
            Debug.Log("Right!");
            Vector3 direction = new Vector3(m_Speed, 0f, 0f);
            m_RigidBody.AddForce(direction, ForceMode.VelocityChange);
        }
        else if (context.control.name.Equals("s"))
        {
            Debug.Log("Backwards!");
            Vector3 direction = new Vector3(0f, 0f, -m_Speed);
            m_RigidBody.AddForce(direction, ForceMode.VelocityChange);
        }
        else if (context.control.name.Equals("a"))
        {
            Debug.Log("Left!");
            Vector3 direction = new Vector3(-m_Speed, 0f, 0f);
            m_RigidBody.AddForce(direction, ForceMode.VelocityChange);
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump!");
        Vector3 direction = new Vector3(0f, m_JumpStrength, 0f);
        m_RigidBody.AddForce(direction, ForceMode.VelocityChange);
    }
}
