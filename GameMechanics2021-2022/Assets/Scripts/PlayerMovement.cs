using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputActionAsset playerControls;

    [SerializeField] private float m_JumpStrength;
    private Vector3 m_Direction;
    private Rigidbody m_RigidBody;

    private InputAction m_Movement;

    private void Awake()
    {
        m_RigidBody = gameObject.GetComponent<Rigidbody>();

        var playerActionMap = playerControls.FindActionMap("Player");

        m_Movement = playerActionMap.FindAction("Movement");
        m_Movement.performed += OnMovement;
        m_Movement.Enable();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        m_RigidBody.MovePosition(m_Direction);

        m_Direction += Physics.gravity * Time.deltaTime;

        if (m_Direction.y < 0f)
            m_Direction.y = 0f;

        Debug.Log(m_Direction);
    }

    private void OnMovement(InputAction.CallbackContext context)
    {
        if (context.control.name.Equals("space"))
        {
            Debug.Log("Jump!");
            m_Direction.y += m_JumpStrength * Time.deltaTime;
        }
        else if (context.control.name.Equals("w"))
        {
            Debug.Log("Forward!");
            m_Direction.z += 50f * Time.deltaTime;
        }
    }
}
