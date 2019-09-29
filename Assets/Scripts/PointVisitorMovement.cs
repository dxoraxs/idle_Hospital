using System.Collections.Generic;
using UnityEngine;

public class PointVisitorMovement : MonoBehaviour
{
    private List<Transform> WayVisitors = new List<Transform>();
    [SerializeField] private Transform StartPoint, FinishPoint;

    private void Start()
    {
        int i = 1;
        while (transform.Find(i.ToString()))
        {
            WayVisitors.Add(transform.Find(i.ToString()));
            i++;
        }
    }

    public Vector3 GetStartPosition
    {
        get => StartPoint.position;
    }

    public Vector3 GetFinishPosition
    {
        get => FinishPoint.position;
    }

    public Vector3 GetPointPosition(int index)
    {
        return WayVisitors[index].position;
    }

    public int GetCountList
    {
        get => WayVisitors.Count;
    }
}
