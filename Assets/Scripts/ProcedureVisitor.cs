using UnityEngine;

public class ProcedureVisitor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Visitor")
        {
            other.GetComponent<VisitorMovement>().ProcedureIsStart();
        }
    }
}
