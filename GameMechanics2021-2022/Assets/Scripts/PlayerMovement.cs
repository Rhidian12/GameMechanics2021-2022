using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float m_JumpStrength;
    [SerializeField] private float m_Speed;

    private Rigidbody m_RigidBody;

    private void Awake()
    {
        m_RigidBody = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void FixedUpdate()
    {
        m_RigidBody.velocity.Set(
            Mathf.Clamp(m_RigidBody.velocity.x, -m_Speed, m_Speed),
            Mathf.Clamp(m_RigidBody.velocity.y, -m_JumpStrength, m_JumpStrength),
            Mathf.Clamp(m_RigidBody.velocity.z, -m_Speed, m_Speed));

        Debug.Log(m_RigidBody.velocity);
    }

    private void HandleMovement()
    {
        var currentInput = Keyboard.current;
        if (currentInput == null)
            return;

        if (currentInput.wKey.isPressed)
            m_RigidBody.velocity += new Vector3(0f, 0f, m_Speed * Time.deltaTime);
        else if (currentInput.dKey.isPressed)
            m_RigidBody.velocity += new Vector3(m_Speed * Time.deltaTime, 0f, 0f);
        else if (currentInput.sKey.isPressed)
            m_RigidBody.velocity += new Vector3(0f, 0f, -m_Speed * Time.deltaTime);
        else if (currentInput.aKey.isPressed)
            m_RigidBody.velocity += new Vector3(-m_Speed * Time.deltaTime, 0f, 0f);
    }
}
