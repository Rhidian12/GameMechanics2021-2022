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

        Vector3 newVelocity = Vector3.zero;
        newVelocity.y = m_RigidBody.velocity.y;

        if (currentInput.wKey.isPressed)
            newVelocity += new Vector3(0f, 0f, m_Speed * Time.deltaTime);

        if (currentInput.dKey.isPressed)
            newVelocity += new Vector3(m_Speed * Time.deltaTime, 0f, 0f);

        if (currentInput.sKey.isPressed)
            newVelocity += new Vector3(0f, 0f, -m_Speed * Time.deltaTime);

        if (currentInput.aKey.isPressed)
            newVelocity += new Vector3(-m_Speed * Time.deltaTime, 0f, 0f);

        if (currentInput.spaceKey.isPressed)
            if (Physics.Raycast(new Ray(m_RigidBody.position, new Vector3(0f, -1f))))
                newVelocity += new Vector3(0f, m_JumpStrength * Time.deltaTime, 0f);

        m_RigidBody.velocity = newVelocity;
    }
}
