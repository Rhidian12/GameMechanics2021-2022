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
    [SerializeField] private float m_TimeToRevealObject = 0f;
    [SerializeField] private float m_Cooldown = 0f;

    private bool m_IsPickedUp = false;
    private float m_CooldownTimer = 0f;

    private struct RevealedObjectInformation
    {
        public RevealedObjectInformation(GameObject gameObject)
        {
            currentTimeRevealed = 0f;
            originalMaterial = null;

            // Is the object the one with the visuals?
            if (gameObject.CompareTag("Visuals"))
                originalMaterial = gameObject.GetComponent<MeshRenderer>().material;
            // Search for the Visuals
            else
                foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
                    if (child.gameObject.CompareTag("Visuals"))
                        originalMaterial = child.gameObject.GetComponent<MeshRenderer>().material;
        }

        public RevealedObjectInformation(Material material)
        {
            currentTimeRevealed = 0f;
            originalMaterial = material;
        }

        public float currentTimeRevealed { get; set; }
        public Material originalMaterial { get; set; } 
    }

    private Dictionary<GameObject, RevealedObjectInformation> m_RevealedObjects = new Dictionary<GameObject, RevealedObjectInformation>();

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(m_MaterialToApply);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_CooldownTimer >= 0f)
            m_CooldownTimer -= Time.deltaTime;

        foreach(KeyValuePair<GameObject, RevealedObjectInformation> element in m_RevealedObjects)
        {
            m_RevealedObjects[element.Key].currentTimeRevealed += Time.deltaTime;

            if (element.Value.currentTimeRevealed >= m_TimeToRevealObject)
            {

            }
        }
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
        // Has the InfraredScanner been picked up?
        if (!m_IsPickedUp)
            return;

        // Is the InfraredScanner on cooldown?
        if (m_CooldownTimer >= 0f)
            return;

        // Get the direction to scan
        Vector3 direction = m_Player.transform.forward;

        // We want to ignore layer 2, which is the RaycastIgnore layer
        const int layerMask = 2;

        // Now that we have our scan field, we need to "reveal" everything there by giving it a red hue
        for (float i = -m_ScanAngle; i <= m_ScanAngle; i += (m_ScanAngle / 20f)) // increase the angle by 5%
        {
            // Keep a list of layers we found
            Dictionary<int, GameObject> foundLayers = new Dictionary<int, GameObject>();

            RaycastHit raycastHit;
            // Send a ray at an angle of i ([-m_ScanAngle, m_ScanAngle]), if we hit something we need to apply our red material to it for m_TimeRevealed seconds
            while (Physics.Raycast(m_Player.transform.position, Quaternion.AngleAxis(-i, Vector3.up) * direction, out raycastHit, m_ScanRange, layerMask))
            {
                if (raycastHit.rigidbody) // there is a rigidbody
                {
                    GameObject gameObject = raycastHit.rigidbody.gameObject;

                    // Save the original gameObject Layer, and set the hit gameObject layer to the layerMask. Thus it will get ignored on the next pass
                    foundLayers.Add(gameObject.layer, gameObject);
                    gameObject.layer = layerMask;
                }
                // now redo the raycast to see if there's something behind the object we just hit
            }

            // Did we hit something?
            if (foundLayers.Count > 0)
            {
                foreach (KeyValuePair<int, GameObject> element in foundLayers)
                {
                    element.Value.layer = element.Key; // reset the gameobjects their original layer
                    m_RevealedObjects.Add(element.Value, new RevealedObjectInformation(element.Value)); // add the gameObject to the hit gameObjects
                }
            }
        }

        // For every game object hit, set their material to the Infrared_RevealedMaterial
        foreach (KeyValuePair<GameObject, RevealedObjectInformation> element in m_RevealedObjects)
        {
            // Is the object the one with the visuals?
            if (element.Key.CompareTag("Visuals"))
                element.Key.GetComponent<MeshRenderer>().material = m_MaterialToApply;
            // Search for the Visuals
            else
                foreach (Transform child in element.Key.GetComponentsInChildren<Transform>())
                    if (child.gameObject.CompareTag("Visuals"))
                        child.gameObject.GetComponent<MeshRenderer>().material = m_MaterialToApply;
        }
    }
}
