using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfraredScanner : MonoBehaviour
{
    [SerializeField] private GameObject m_Player;
    [SerializeField] private float m_ScanRange = 0f;
    [SerializeField] private float m_ScanAngle = 0f;
    
    private bool m_IsPickedUp = false;

    // Start is called before the first frame update
    void Start()
    {
                
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_IsPickedUp = true;

            // TODO: Add infra red scanner to player
        }
    }

    public void ScanAhead()
    {
        // Get the direction to scan
        Vector3 direction = m_Player.transform.forward;

        // Make 2 vectors pointing towards the outer edges of our scan radius
        Vector3 leftEdge = Quaternion.AngleAxis(-m_ScanAngle, Vector3.up) * direction;
        Vector3 rightEdge = Quaternion.AngleAxis(m_ScanAngle, Vector3.up) * direction;
    
        // Now that we have our scan field, we need to "reveal" everything there by giving it a red hue
    }
}
