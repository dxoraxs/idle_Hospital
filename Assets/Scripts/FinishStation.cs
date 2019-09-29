using System.Collections.Generic;
using UnityEngine;

public class FinishStation : MonoBehaviour
{
    private List<GameObject> VisitorGoHome = new List<GameObject>();

    public bool GetCountList()
    {
        return VisitorGoHome.Count>0;
    }

    public void GetFirstVisitor()
    {
        Destroy(VisitorGoHome[0]);
        VisitorGoHome.RemoveAt(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Visitor")
        {
            VisitorGoHome.Add(other.gameObject);
            other.GetComponent<VisitorMovement>().StayInStation();
        }
    }
}