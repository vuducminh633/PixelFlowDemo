using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Belt : MonoBehaviour
{
    private static int _beltID = 0;

    public Belt beltInSequence;
    public BeltItem beltItem;
    public bool isSpaceTaken;

    private BeltManager _beltManager;
    private PlayerInputManager _playerInputManager;

    private void Awake()
    {
        _beltManager = FindObjectOfType<BeltManager>();
        _playerInputManager = FindObjectOfType<PlayerInputManager>();
        beltInSequence = null;
        beltInSequence = FindNextBelt();
        gameObject.name = $"Belt: {_beltID++}";
    }

    private void Update()
    {
        if (beltInSequence == null)
            beltInSequence = FindNextBelt();

        if (beltItem != null && beltItem.item != null)
            StartCoroutine(StartBeltMove());
    }

    public Vector3 GetItemPosition()
    {
        var padding = 0.3f;
        var position = transform.position;
        return new Vector3(position.x, position.y + padding, position.z);
    }

    private IEnumerator StartBeltMove()
    {
        isSpaceTaken = true;

        if (beltItem.item != null && beltInSequence != null && beltInSequence.isSpaceTaken == false)
        {
            Vector3 toPosition = beltInSequence.GetItemPosition();

            beltInSequence.isSpaceTaken = true;

            var step = _beltManager.speed * Time.fixedDeltaTime;

            while (beltItem.item.transform.position != toPosition)
            {
                beltItem.item.transform.position =
                    Vector3.MoveTowards(beltItem.transform.position, toPosition, step);

                yield return null;
            }

            isSpaceTaken = false;
            beltInSequence.beltItem = beltItem;
            beltItem = null;

            RotateItemAtCorner(beltInSequence);  // call before out of loop to prevent null exeption when remove item

            if (OnLoopCompleted(beltInSequence))
            {
                //call function to add obj to the wait point
                beltInSequence.beltItem.transform.position = _playerInputManager.FindWaitPointTransform( beltInSequence.beltItem);
                //and we have to disalbe the movement of the item on the belt
                beltInSequence.beltItem = null;
                beltInSequence.isSpaceTaken = false;

            }


        }
    }

    private bool OnLoopCompleted(Belt beltInSequence)
    {
        //Check if the item has finish a loop by see if it each the final belt
        if (beltInSequence.gameObject.name == "Belt: 4")
        {
            return true;
        }

        return false;

    }

    private Belt FindNextBelt()
    {
        Transform currentBeltTransform = transform;
        RaycastHit hit;

        var forward = transform.forward;

        Ray ray = new Ray(currentBeltTransform.position, forward);

        if (Physics.Raycast(ray, out hit, 1f))
        {
            Belt belt = hit.collider.GetComponent<Belt>();

            if (belt != null)
                return belt;
        }

        return null;
    }

    public void AddItemToBelt(BeltItem item)
    {
        GameObject startBeltObj = GameObject.Find("Belt: 4");
        Belt startBelt = startBeltObj.GetComponent<Belt>();
        if (startBeltObj != null && startBelt.isSpaceTaken == false)
        {

            //If the belt item is in the wait point , we set the occupitation of that point to false
            if (item.currentWaitPoint != null)
            {
                item.currentWaitPoint.SetOccupied(false);
                item.currentWaitPoint = null; // Clear reference to avoid reusing
            }

            item.transform.position = startBelt.GetItemPosition();
            startBelt.beltItem = item;   //NOT FINISH YET   ::: And Somehow it still work=))
        }
        
      
       
    }

    private void RotateItemAtCorner(Belt beltInSequence)
    {
        //Belt8 , Belt 11 , belt 10, belt 6 , thiss will be the point we rotate the obj
        List<Belt> cornerBelt = new List<Belt> ();  
        Belt[] allBelt = FindObjectsOfType<Belt>();
       for(int i = 0; i< allBelt.Length; i++)
        {
            if (allBelt[i].name == "Belt: 8" || allBelt[i].name == "Belt: 11" || allBelt[i].name == "Belt: 10" || allBelt[i].name == "Belt: 6")
            {
                cornerBelt.Add(allBelt[i]);
            }
        }

        if (cornerBelt.Contains(beltInSequence))
        {
            Debug.Log("Item has reach Corner");
            //WE have to make it smooth

            beltInSequence.beltItem.transform.Rotate(0, -90, 0); // 0 -90 -180 90 0  
            
            
            
        }
        
    }

}