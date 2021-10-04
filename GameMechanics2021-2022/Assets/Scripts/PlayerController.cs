using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerMovement m_PlayerMovementScript;
    [SerializeField] private PlayerFiring m_PlayerFiringScript;
    [SerializeField] private PlayerAim m_PlayerAimScript;

    private string m_HorizontalKeyboardAxis = "KeyboardHorizontal";
    private string m_VerticalKeyboardAxis = "KeyboardVertical";
    private string m_PlayerFiringAxis = "Firing";
    private string m_MouseX = "MouseX";
    private string m_MouseY = "MouseY";

    // Update is called once per frame
    void Update()
    {
        HandlePlayerMovementInput();
        HandlePlayerFiringInput();
        HandlePlayerAimInput();
    }

    private void HandlePlayerMovementInput()
    {
        m_PlayerMovementScript.DesiredVelocity = Input.GetAxisRaw(m_HorizontalKeyboardAxis) * m_PlayerMovementScript.Rigidbody.transform.right
            + Input.GetAxisRaw(m_VerticalKeyboardAxis) * m_PlayerMovementScript.Rigidbody.transform.forward;
    }

    private void HandlePlayerFiringInput()
    {
        if (m_PlayerFiringScript.m_HasFired)
        {
            if (Input.GetAxisRaw(m_PlayerFiringAxis) < 1f)
                m_PlayerFiringScript.m_HasFired = false;
        }
        else
        {
            if (Input.GetAxisRaw(m_PlayerFiringAxis) > 0f)
                m_PlayerFiringScript.m_HasFired = Input.GetAxisRaw(m_PlayerFiringAxis) > 0f ? true : false;
        }
    }

    private void HandlePlayerAimInput()
    {
        m_PlayerAimScript.m_CurrentRotation = new Vector2(Input.GetAxisRaw(m_MouseX), Input.GetAxisRaw(m_MouseY));
    }
}
