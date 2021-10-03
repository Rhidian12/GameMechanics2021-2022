using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] private Camera m_Camera;

    private Vector2 m_MousePosition;
    private Vector2 m_CurrentRotation;

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        // Calculate rotation based on mouse position
        m_CurrentRotation.x += m_MousePosition.x;
        m_CurrentRotation.y -= m_MousePosition.y;

        m_CurrentRotation.x = Mathf.Repeat(m_CurrentRotation.x, 360);
        m_CurrentRotation.y = Mathf.Clamp(m_CurrentRotation.y, -80f, 80f);

        m_Camera.transform.rotation = Quaternion.Euler(m_CurrentRotation.y, m_CurrentRotation.x, 0);

        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void HandleInput()
    {
        var currentInput = Mouse.current;
        if (currentInput != null)
            m_MousePosition = currentInput.position.ReadValue();
    }
}
