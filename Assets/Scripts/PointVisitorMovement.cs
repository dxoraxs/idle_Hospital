using System.Collections.Generic;
using UnityEngine;

public class PointVisitorMovement : MonoBehaviour
{
    [HideInInspector]public List<Transform> WayVisitors;
    public Transform StartPoint, FinishPoint;

    private void Start()
    {
        int i = 1;
        while (transform.Find(i.ToString()))
        {
            WayVisitors.Add(transform.Find(i.ToString()));
            i++;
        }
    }
}
