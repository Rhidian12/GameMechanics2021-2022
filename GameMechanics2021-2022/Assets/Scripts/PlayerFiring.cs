using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFiring : MonoBehaviour
{
    public bool m_HasFired = false;

    [SerializeField] private Transform m_BulletSpawnPoint;
    [SerializeField] private GameObject m_BulletPrefab;

    private bool m_HasShotBullet = false;
    private int m_Counter = 0;
    private int m_RateOfFire = 144; // the higher the rate of fire, the longer it takes to shoot again. Maybe make a seperate weapon class?

    // Update is called once per frame
    void Update()
    {
        Fire();
    }

    private void Fire()
    {
        // All of this would be better of in a Weapon class
        if (m_HasFired && !m_HasShotBullet)
        {
            GameObject bullet = Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position, Quaternion.identity);
            bullet.GetComponentInChildren<BulletMovement>().Velocity = m_BulletSpawnPoint.forward;

            m_HasShotBullet = true;
        }

        if (m_HasShotBullet)
        {
            if(++m_Counter >= m_RateOfFire)
            {
                m_Counter = 0;
                m_HasShotBullet = false;
                m_HasFired = false;
            }
        }
    }
}