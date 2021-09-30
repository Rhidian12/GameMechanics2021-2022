using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float m_Speed;

    private Vector3 m_DesiredVelocity;
    private Rigidbody m_RigidBody;

    private void Awake()
    {
        m_RigidBody = gameObject.GetComponent<Rigidbody>();
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
        var currentInput = Keyboard.current;
        if (currentInput == null)
            return;

        m_DesiredVelocity = Vector3.zero;

        if (currentInput.wKey.isPressed)
            m_DesiredVelocity += Vector3.forward;

        if (currentInput.dKey.isPressed)
            m_DesiredVelocity += Vector3.right;

        if (currentInput.sKey.isPressed)
            m_DesiredVelocity += Vector3.back;

        if (currentInput.aKey.isPressed)
            m_DesiredVelocity += Vector3.left;
    }
}