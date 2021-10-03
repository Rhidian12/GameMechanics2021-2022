using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] private Camera m_Camera;

    private Vector2 m_MousePosition;

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        // Calculate rotation based on mouse position
        Vector3 newForward = m_Camera.ScreenToWorldPoint(new Vector3(m_MousePosition.x, m_MousePosition.y, m_Camera.nearClipPlane));

        m_Camera.transform.forward = newForward.normalized;
    }

    private void HandleInput()
    {
        m_MousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }
}
