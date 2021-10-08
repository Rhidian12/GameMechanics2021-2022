using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class InfraredScanner : MonoBehaviour
{
    [SerializeField] private Material m_MaterialToApply;
    [SerializeField] private float m_ScanRange = 0f;
    [SerializeField] private float m_ScanAngle = 0f;
    [SerializeField] private float m_TimeToRevealObject = 0f;
    [SerializeField] private float m_Cooldown = 0f;

    private GameObject m_Player;
    private Transform m_InfraredSocket;
    private bool m_IsPickedUp = false;
    private float m_CooldownTimer = 0f;

    private struct RevealedObjectInformation
    {
        public RevealedObjectInformation(GameObject gameObject)
        {
            currentTimeRevealed = 0f;
            originalMaterial = null;
            meshRenderer = null;

            // Is the object the one with the visuals?
            if (gameObject.CompareTag("Visuals"))
            {
                meshRenderer = gameObject.GetComponent<MeshRenderer>();
                originalMaterial = meshRenderer.material;
            }
            // Search for the Visuals
            else
            {
                foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
                {
                    if (child.gameObject.CompareTag("Visuals"))
                    {
                        meshRenderer = gameObject.GetComponent<MeshRenderer>();
                        originalMaterial = meshRenderer.material;
                    }
                }
            }
        }

        public float currentTimeRevealed { get; set; }
        public Material originalMaterial { get; set; }
        public MeshRenderer meshRenderer { get; set; }
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

        List<GameObject> objectsToBeUnrevealed = new List<GameObject>();

        foreach(KeyValuePair<GameObject, RevealedObjectInformation> element in m_RevealedObjects)
        {
            // I wish this were C++ instead of C#
            // stop trying to help me C#
            // let me shoot myself in the fucking foot dumbass language
            RevealedObjectInformation temp = element.Value;
            temp.currentTimeRevealed += Time.deltaTime;
            m_RevealedObjects[element.Key] = temp;

            if (temp.currentTimeRevealed >= m_TimeToRevealObject)
            {
                element.Value.meshRenderer.material = element.Value.originalMaterial;
                objectsToBeUnrevealed.Add(element.Key);
            }
        }

        foreach(GameObject key in objectsToBeUnrevealed)
            m_RevealedObjects.Remove(key);
    }

    private void OnTriggerEnter(Collider other)
    {
        // if we have picked the scanner up, ignore this function
        if (m_IsPickedUp)
            return;

        // is the player trying to pick the scanner up?
        if (other.CompareTag("Player")) // Player Tag should be the root
        {
            m_IsPickedUp = true;

            foreach (Transform transform in other.transform.GetComponentInChildren<Transform>())
            {
                // Find the infrared socket
                if (transform.name.Equals("InfraredSocket"))
                {
                    m_InfraredSocket = transform;
                    break;
                }
            }

            // Make sure we have the player
            m_Player = other.gameObject;
            
            // parent the scanner to the infrared socket
            transform.parent = m_InfraredSocket;

            // Set this script in the PlayerController as the infrared scanner script
            m_Player.GetComponent<PlayerController>().SetInfraredScannerScript = this;
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

        // print a debug string
        Debug.Log("SCANNING");

        // Set the m_CooldownTimer
        m_CooldownTimer = m_Cooldown;

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
