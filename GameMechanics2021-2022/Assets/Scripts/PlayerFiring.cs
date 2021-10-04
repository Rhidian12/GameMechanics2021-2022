using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFiring : MonoBehaviour
{
    public bool m_HasFired = false;

    [SerializeField] private Transform m_BulletSpawnPoint;
    [SerializeField] private GameObject m_BulletPrefab;


    // Update is called once per frame
    void Update()
    {
        Fire();
    }

    private void Fire()
    {
        if (!m_HasFired)
        {
            GameObject bullet = Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position, Quaternion.identity);
            bullet.GetComponentInChildren<BulletMovement>().Velocity = m_BulletSpawnPoint.forward;
        }
    }
}