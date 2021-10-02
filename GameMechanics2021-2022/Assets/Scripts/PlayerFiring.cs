using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerFiring : MonoBehaviour
{
    [SerializeField] private Transform m_BulletSpawnPoint;
    [SerializeField] private GameObject m_BulletPrefab;

    private bool m_IsFiring;

    // Update is called once per frame
    void Update()
    {
        HandleInput();

        Fire();
    }

    private void HandleInput()
    {
        var currentInput = Mouse.current;
        if (currentInput == null)
            return;
        else // I should read branch prediction again
            m_IsFiring = currentInput.leftButton.wasReleasedThisFrame;
    }

    private void Fire()
    {
        if (m_IsFiring)
        {
            GameObject bullet = Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position, Quaternion.identity);
            bullet.GetComponentInChildren<BulletMovement>().Velocity = m_BulletSpawnPoint.forward;
        }
    }
}