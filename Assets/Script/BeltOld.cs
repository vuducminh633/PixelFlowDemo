using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BeltOld : MonoBehaviour
{
    public static int beltID = 0;

    public BeltOld beltNext;
    public BeltItemOld beltItem;
    private BeltManagerld manager;
    bool isSpaceTaken;

    private void Awake()
    {
        manager = FindObjectOfType<BeltManagerld>();
    }
    private void Start()
    {
        beltNext = null;
        beltNext = NextBelt();
        gameObject.name = $"Belt {beltID++}";

    }

    private void Update()
    {
        if(beltNext== null)
        {
            beltNext = NextBelt();
        }

        if(beltItem!= null && beltItem.item != null)
        {
            StartCoroutine(StartMoveBelt());
        }
    }
    private Vector3 GetitemPosition()
    {
        return new Vector3(transform.position.x+.3f , transform.position.y +.3f , transform.position.z);
    }

    private IEnumerator StartMoveBelt()
    {
        isSpaceTaken = true;
        if(beltItem.item != null && beltNext!= null && beltNext.isSpaceTaken == false)
        {
            Vector3 toPosition = beltNext.GetitemPosition();
            beltNext.isSpaceTaken = true;

            while(beltItem.item.transform.position != toPosition)
            {
                beltItem.item.transform.position = Vector3.MoveTowards(beltItem.transform.position, toPosition, manager.speed * Time.deltaTime);

                yield return null;
            }
        }

        isSpaceTaken = false;
        beltNext.beltItem = beltItem;
        beltItem = null;
    }

    private BeltOld NextBelt()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);   
        if(Physics.Raycast(ray, out hit, 1f))
        {
            BeltOld nextBelt = hit.collider.GetComponent<BeltOld>();

            if (nextBelt != null)
                return nextBelt;
        }
        return null;    
    }
}

