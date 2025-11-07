using System;
using System.Linq.Expressions;
using UnityEngine;

public class BeltItem : MonoBehaviour
{
    public GameObject item;

    public Transform gunPoint;

    public float bulletSpeed = 4f;

    public GameObject bulletPrefab;

    public float bulletCoolDown = .2f;
    private float lasttimeShoot  = 0;

    public WaitPoint currentWaitPoint;

    MeshRenderer itemMesh;

    private Collider lastshotTarget;

    private void Awake()
    {
        itemMesh = GetComponent<MeshRenderer>();    
        item = gameObject;
        gameObject.name = "Belt_Item " + itemMesh.material.name;
       
    }

    private void Update()
    {
        //launch a raycast
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000))
        {
            if(hit.collider.gameObject.layer == LayerMask.NameToLayer("BoxLayer") && IsInConveybelt())  // layer in front will block ray (i think)
            {
             
                // it must have the same material
                if(hit.transform.GetComponent<MeshRenderer>().material.name == itemMesh.material.name)
                {
            
                    if (Time.time >= lasttimeShoot + bulletCoolDown && hit.collider!= lastshotTarget)
                    {
                        ShootBullet(hit.collider);
                        lastshotTarget = hit.collider;  // assign the target we just shoot
                        lasttimeShoot = Time.time; // reset cooldown timer
                    }
                   
                }
             
            }
          
        }
        else
        {
            lastshotTarget = null;
        }

        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red, 1f);




    }

    private void ShootBullet(Collider objHit)
    {
        // check the direction
        Vector3 directionToTarget = (objHit.transform.position - transform.position).normalized;


        GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(directionToTarget * bulletSpeed, ForceMode.Impulse);

        
    }

    public bool IsInConveybelt()
    {
        Collider[] hit = Physics.OverlapBox(transform.position, new Vector3(.4f, 1f, .4f), Quaternion.identity);

        foreach(Collider collider in hit)
        {
            if (collider.GetComponent<Belt>() != null)
            {
                return true;
            }
        }
        return false;   
            
    }

    private void OnDrawGizmosSelected()
    {

        Vector3 halfExtents = new Vector3(.4f, 1f, .4f);
        Vector3 center = transform.position;

       
        Gizmos.color = new Color(0f, 0f, 1f, 0.3f);
        Gizmos.DrawCube(center, halfExtents); 

    }


}
