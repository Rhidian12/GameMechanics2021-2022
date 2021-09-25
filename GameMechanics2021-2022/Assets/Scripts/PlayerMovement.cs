using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputActionAsset playerControls;

    [SerializeField] private float m_JumpStrength;
    private Vector3 m_Direction;
    private Transform m_Transform;

    private InputAction m_Movement;

    // Start is called before the first frame update
    private void Awake()
    {
        m_Transform = gameObject.transform;

        var playerActionMap = playerControls.FindActionMap("Player");

       m_Movement = playerActionMap.FindAction("Movement");
       m_Movement.performed += OnMovement;
       m_Movement.canceled += OnMovement;
       m_Movement.Enable();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        m_Transform.Translate(m_Direction);

        m_Direction.y -= 9.81f;
    }

    private void OnMovement(InputAction.CallbackContext context)
    {
        Debug.Log("Jump!");
        m_Direction.y += m_JumpStrength;
    }
}
