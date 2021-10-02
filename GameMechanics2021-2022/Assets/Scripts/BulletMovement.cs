using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [SerializeField] private float m_Speed;

    private Vector3 m_Velocity;
    private Rigidbody m_Rigidbody;

    public Vector3 Velocity
    {
        get => m_Velocity;
        set
        {
            if (value.sqrMagnitude <= 1f)
                m_Velocity = value;
            else
                m_Velocity = value.normalized;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = gameObject.GetComponent<Rigidbody>();

        m_Velocity = Vector3.zero;
    }

    void FixedUpdate()
    {
        m_Rigidbody.velocity = m_Velocity * m_Speed * Time.fixedDeltaTime;
    }
}
