using System.Collections.Generic;
using UnityEngine;

public class VisitorMovement : MovingObject
{
    [SerializeField] private LayerMask _visitorMask;
    private List<Vector3> TrackMov = new List<Vector3>();
    private ProcedureList _procedures;
    private PointVisitorMovement _pointMovement;
    private LevelAllScene _levelScene;
    private Vector3 _targetProcedure;
    private VisitorStage _visitorStage = VisitorStage.Move;
    private float timerProcedure;
    private Animator _animator;
    private bool _isMove, _isEndProcedure;
    private int _numberProcedure;

    public void ProcedureIsStart()
    {
        _isEndProcedure = true;
        _visitorStage = VisitorStage.Procedure;
        timerProcedure = 5f;
        _animator.enabled = true;
    }
    
    public bool GetEndProcedure()
    {
        return _isEndProcedure;
    }

    public int GetNumberProcedure()
    {
        return _numberProcedure;
    }

    public void SetScriptData(SceneScriptData _sceneScriptData, float speed, LevelAllScene levelScene)
    {
        _procedures = _sceneScriptData.ProcedureList;
        _pointMovement = _sceneScriptData.PointVisitorMovement;
        _levelScene = levelScene;
        _speedMoving = speed;
    }

    public void StayInStation()
    {
        _visitorStage = VisitorStage.Boarding;
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        SelectRandomProcedure();
        FindMinimalPathToProcedure();
    }

    private void Update()
    {
        switch (_visitorStage)
        {
            case VisitorStage.Move:
                if (_isMove) MovementToPoint(TrackMov[_numberPoint], TrackMov.Count - 1);
                break;
            case VisitorStage.Procedure:
                ProcedureWithVisitor();
                break;
            case VisitorStage.Boarding:
                if (_isMove && Mathf.Abs(transform.position.z - TrackMov[_numberPoint].z) > 0.1f)
                    MovementToPoint(TrackMov[_numberPoint], TrackMov.Count - 1);
                break;
        }
    }

    private void FixedUpdate()
    {
        _isMove = FindFrontVisitor();
    }

    private void ProcedureWithVisitor()
    {
        timerProcedure -= Time.deltaTime;
        if (timerProcedure < 0)
        {
            _visitorStage = VisitorStage.Move;
            _numberPoint++;
            _animator.enabled = false;
            _levelScene.MoneyArrived(_numberProcedure);
        }
    }

    private void SelectRandomProcedure()
    {
        _numberProcedure = _procedures.GetRandomNumberProcedure();
        _targetProcedure = _procedures.GetPositionProcedure(_numberProcedure);
        transform.Find("Body/ColorProcedure").GetComponent<Renderer>().material.color = _procedures.GetColorIdProcedure(_numberProcedure);
    }

    private void FindMinimalPathToProcedure()
    {
        TrackMov.Add(_pointMovement.GetStartPosition);
        float minimalDistanceToProcedure = distancePointToPoint(_targetProcedure);
        int countPoint = 0;
        while (minimalDistanceToProcedure != Vector3.Distance(TrackMov[TrackMov.Count -1], _targetProcedure))
        {
            TrackMov.Add(_pointMovement.GetPointPosition(countPoint));
            countPoint++;
        }
        TrackMov.Add(_targetProcedure);
        countPoint--;
        while (countPoint< _pointMovement.GetCountList)
        {
            TrackMov.Add(_pointMovement.GetPointPosition(countPoint));
            countPoint++;
        }
        TrackMov.Add(_pointMovement.GetFinishPosition);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position + transform.forward * 0.7f + new Vector3(0, 0.75f, 0), 0.2f);
    }

    private bool FindFrontVisitor()
    {
        Collider[] visitors = Physics.OverlapSphere(transform.position + transform.forward * 0.7f + new Vector3(0, 0.75f, 0), 0.2f, _visitorMask);
        foreach(Collider visitor in visitors)
        {
            VisitorMovement visScripts = visitor.GetComponent<VisitorMovement>();
            if (visScripts.GetNumberProcedure() != _numberProcedure && visScripts.GetEndProcedure())
                return false;
            else if (visScripts.GetNumberProcedure() == _numberProcedure && !visScripts.GetEndProcedure() && _isEndProcedure)
                return true;
            else if (visScripts.GetNumberProcedure() != _numberProcedure && !visScripts.GetEndProcedure() && !_isEndProcedure)
                return true;
            else if (visScripts.GetNumberProcedure() != _numberProcedure && !visScripts.GetEndProcedure() && _isEndProcedure)
                return true;
            else return false;
        }
        return true;
    }

    private float distancePointToPoint(Vector3 firstPoint)
    {
        float returnFloat = int.MaxValue;
        for (int i =0; i< _pointMovement.GetCountList; i++)
        {
            float interimDistance = Vector3.Distance(firstPoint, _pointMovement.GetPointPosition(i));
            if (returnFloat > interimDistance)
                returnFloat = interimDistance;
        }
        return returnFloat;
    }

    enum VisitorStage
    {
        Move,
        Procedure,
        Boarding
    }
}
