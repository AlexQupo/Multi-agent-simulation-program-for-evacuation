using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointScript : MonoBehaviour
{
    public string parentName;
    public int number;
    //tigerLILI
    public void CollisionFromChild(Collider target)
    {
        if(target.name == parentName)
        {
            GameObject.Find(parentName).GetComponentInParent<DrawPath>().childPointNum = number;
            GameObject.Find(parentName).GetComponentInParent<DrawPath>().isChildPoint = true;
        }
    }
}
