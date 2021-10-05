using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class InfraredScanner : MonoBehaviour
{
    [SerializeField] private GameObject m_Player;
    [SerializeField] private Material m_MaterialToApply;
    [SerializeField] private float m_ScanRange = 0f;
    [SerializeField] private float m_ScanAngle = 0f;
    [SerializeField] private float m_TimeRevealed = 0f;
    
    private bool m_IsPickedUp = false;

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(m_MaterialToApply); 
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
        if (!m_IsPickedUp)
            return;

        // Get the direction to scan
        Vector3 direction = m_Player.transform.forward;

        // We want to ignore layer 2, which is the RaycastIgnore layer
        const int layerMask = 2;

        // Keep a list of layers we found
        Dictionary<int, GameObject> foundLayers = new Dictionary<int, GameObject>();

        // Now that we have our scan field, we need to "reveal" everything there by giving it a red hue
        for (float i = -m_ScanAngle; i <= m_ScanAngle; i += (m_ScanAngle / 20f)) // increase the angle by 5%
        {
            RaycastHit raycastHit;
            // Send a ray at an angle of i ([-m_ScanAngle, m_ScanAngle]), if we hit something we need to apply our red material to it for m_TimeRevealed seconds
            while (Physics.Raycast(m_Player.transform.position, Quaternion.AngleAxis(-i, Vector3.up) * direction, out raycastHit, m_ScanRange, layerMask))
            {
                if (raycastHit.rigidbody) // there is a rigidbody
                {
                    GameObject gameObject = raycastHit.rigidbody.gameObject;

                    // Save the original Layer, and set the layer to the layerMask
                    foundLayers.Add(gameObject.layer, gameObject);
                    gameObject.layer = layerMask;

                    // now redo the raycast to see if there's something behind the object we just hit
                }
            }

            // Did we hit something?
            if(foundLayers.Count > 0)
            {
                for (int j = 0; j < foundLayers.Count; ++j)
                {
                }
            }
        }
    }
}
