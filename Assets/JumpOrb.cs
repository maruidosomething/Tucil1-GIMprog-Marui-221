using System.Collections;
using UnityEngine;

public class JumpOrb : MonoBehaviour
{
    [Header("Settings")]
    public float respawnTime = 3f;
    public float bounceForce = 20f;

    [Header("Effects (Optional)")]
    public GameObject collectEffect;

    private Collider orbCollider;
    private MeshRenderer orbRenderer;

    private void Start()
    {
        orbCollider = GetComponent<Collider>();
        orbRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {

        PlayerMovement pm = other.GetComponentInParent<PlayerMovement>();
        
        Rigidbody rb = other.attachedRigidbody;

        if (pm != null && rb != null)
        {
            pm.RefillJumps();

            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);

            if (collectEffect != null)
                Instantiate(collectEffect, transform.position, Quaternion.identity);

            StartCoroutine(DisableAndRespawn());
        }
    }

    private IEnumerator DisableAndRespawn()
    {
        orbCollider.enabled = false;
        orbRenderer.enabled = false;

        yield return new WaitForSeconds(respawnTime);

        orbCollider.enabled = true;
        orbRenderer.enabled = true;
    }
}