using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerInputManager : MonoBehaviour
{

    public WaitPoint currentWaitPoint { get; private set; }
    private Belt firstBelt;

    

    private void Start()
    {
        firstBelt  = GameObject.Find("Belt: 8").GetComponent<Belt>();  
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            PlayerInput(Input.mousePosition);
        }

#elif UNITY_ANDROID
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PlayerInput(Input.GetTouch(0).position);
        }
#endif
    }

    private void PlayerInput(Vector3 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f))
        {
           
            if (hit.transform.GetComponent<BeltItem>()!= null)  // i will change this later
            {
                // Put the item on thee convey belt  
                if (hit.transform.GetComponent<BeltItem>()?.IsInConveybelt() == false)
                {
                    firstBelt.AddItemToBelt(GetPressedItem(hit));
                }
            }
        }
        
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red, 1f);
    }

    public BeltItem GetPressedItem(RaycastHit hit)
    {
        return hit.transform.GetComponent<BeltItem>();  
    }

    public Vector3 FindWaitPointTransform(BeltItem item)
    {
        List<WaitPoint> validWaitPoint = new List<WaitPoint>();

        foreach(WaitHandle waitHandle in CollectWaitPoint())
        {

            if (waitHandle != null)
            {
                validWaitPoint.AddRange(waitHandle.GetValidWaitPoint());
             
            }
          
        }

        //Set the wait point of the belt item to the first wait point of list
        
        currentWaitPoint = validWaitPoint[0];
        currentWaitPoint.SetOccupied(true);

        //if the belt obj is to mobe to the belt
        //Set some condition for when the belt item move to belt
        // currentWaitPoint.SetOccupied(false);

        item.currentWaitPoint = currentWaitPoint;  

        return currentWaitPoint.transform.position;
        
    }

    private List<WaitHandle> CollectWaitPoint()
    {
        List<WaitHandle> collectedWaitPoint = new List<WaitHandle>();
        WaitHandle[] waitHandlesPoint = FindObjectsOfType<WaitHandle>();    

        foreach(var wait in waitHandlesPoint)
        {
            collectedWaitPoint.Add(wait);   
        }

        return collectedWaitPoint;
    } 

    //function to check if allwait point are full
    public bool IsAllWaitPointFull()
    {
        WaitHandle waitHandles = FindObjectOfType<WaitHandle>();

        foreach (WaitPoint point in waitHandles.GetComponentsInChildren<WaitPoint>())
        {

            if (!point.occupied)
            {
                return false;
            }
        }

        return true;

    }

}
