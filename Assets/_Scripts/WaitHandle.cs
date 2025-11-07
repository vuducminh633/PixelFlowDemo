using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitHandle : MonoBehaviour
{
    private List<WaitPoint> waitPoints = new List<WaitPoint>();


    private void Awake()
    {
       
        waitPoints.AddRange(GetComponentsInChildren<WaitPoint>());
    }


    //List to retur all the valid wait point
    public List<WaitPoint> GetValidWaitPoint()
    {
        List<WaitPoint > validWaitPoint = new List<WaitPoint>();
        foreach (WaitPoint point in waitPoints)
        {
            if (IsWaitPointValid(point))
            {
                validWaitPoint.Add(point);  
            }
        }

        return validWaitPoint;  
    }  

    //a funciton to check if the point is valid 
    bool IsWaitPointValid(WaitPoint point)
    {
        if (point.occupied)
        {
            return false;
        }

        return true;
    }
}
