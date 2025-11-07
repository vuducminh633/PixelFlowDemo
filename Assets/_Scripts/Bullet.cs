using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("BoxLayer"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
