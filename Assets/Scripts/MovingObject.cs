using UnityEngine;

public class MovingObject : MonoBehaviour
{
    protected float _speedMoving;
    protected int _numberPoint = 0;

    protected void MovementToPoint(Vector3 point, int arrayLength)
    {
        if (Vector3.Distance(transform.position, point) < 0.05f)
        {
            if (_numberPoint == arrayLength)
            {
                gameObject.SetActive(false);
                return;
            }
            _numberPoint++;
        }
        transform.position = Vector3.MoveTowards(transform.position, point, _speedMoving * Time.deltaTime);
        transform.LookAt(point);
    }
}
