using UnityEngine;

public class BreakFence : MonoBehaviour
{
    public Rigidbody[] fenceParts;
    public float explosionForce = 200f;   // słabsza eksplozja
    public float explosionRadius = 2f;    // mniejszy zasięg
    public float disableColliderDelay = 1f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (Rigidbody part in fenceParts)
            {
                part.isKinematic = false;
                part.AddExplosionForce(explosionForce, transform.position, explosionRadius);

                StartCoroutine(DisableColliderAfterDelay(part, disableColliderDelay));
            }
        }
    }

    private System.Collections.IEnumerator DisableColliderAfterDelay(Rigidbody part, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        Collider col = part.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        part.isKinematic = true; // zamraża w miejscu, by już nie turlał się
    }
}