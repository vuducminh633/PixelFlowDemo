using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private ParticleSystem trailParticles;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("BoxLayer"))
        {
            BoxEffect box = other.gameObject.GetComponent<BoxEffect>();
            if (box != null)
            {
                box.OnHit();
            }
            HandleTrailAndDestroy();
            // Destroy(other.gameObject);
            // Destroy(gameObject);
        }
    }
    private void HandleTrailAndDestroy()
    {
        if (trailParticles != null)
        {
            trailParticles.Stop();
            trailParticles.transform.parent = null;

            Destroy(trailParticles.gameObject, 1f); 
        }

        Destroy(gameObject);
    }
}
