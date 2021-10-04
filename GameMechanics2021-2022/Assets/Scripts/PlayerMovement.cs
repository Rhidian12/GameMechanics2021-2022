using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float m_Speed;

    private Vector3 m_DesiredVelocity;
    private Rigidbody m_RigidBody;

    private string m_HorizontalKeyboardAxis = "KeyboardHorizontal";
    private string m_VerticalKeyboardAxis = "KeyboardVertical";

    private void Awake()
    {
        m_RigidBody = gameObject.GetComponentInParent<Rigidbody>();
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        Vector3 newVelocity = Vector3.zero;
        newVelocity += m_DesiredVelocity;
        newVelocity *= m_Speed * Time.fixedDeltaTime;
        newVelocity.y = m_RigidBody.velocity.y;

        m_RigidBody.velocity = newVelocity;
    }

    private void HandleInput()
    {
        m_DesiredVelocity = Vector3.zero;

        m_DesiredVelocity = Input.GetAxisRaw(m_HorizontalKeyboardAxis) * m_RigidBody.transform.right
            + Input.GetAxisRaw(m_VerticalKeyboardAxis) * m_RigidBody.transform.forward;
    }
}