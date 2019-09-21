using System.Collections;
using UnityEngine;
using Zenject;

public class TramMovement : MovingObject
{
    [SerializeField] private GameObject _prefabVisitor;
    [SerializeField] protected Transform[] _objectMovementPoint;
    private float _speedSpawnVisitor;
    [Inject] private SceneScriptData _sceneScriptData;
    [Inject] private InitialSettings _initialSettings;
    [Inject] private LevelAllScene _levelTramData;
    private TramStady _tramStady = TramStady.Disembarkation;
    private Coroutine _coroutineTramAct;
    private bool _landingStop;
    private float _timerDestroyVisitor =0;

    private void Start()
    {
        _speedSpawnVisitor = _initialSettings.InitialSettingTram.TimeToSpawnVisitor;
        _speedMoving = _initialSettings.InitialSettingTram.SpeedTram;
    }

    enum TramStady
    {
        Arrival,
        Disembarkation,
        Movement,
        Boarding,
        Departure
    }

    private void OnValidate()
    {
        if (_speedSpawnVisitor < 0) _speedSpawnVisitor = 0;
        if (_speedMoving < 0) _speedMoving = 0;
    }

    private void Update()
    {
        switch(_tramStady)
        {
            case TramStady.Arrival:
                MovementToPoint(_objectMovementPoint[_numberPoint].position, _objectMovementPoint.Length - 1);
                if (Mathf.Abs(transform.position.x) < 0.1f && transform.position.z < 0)
                {
                    _coroutineTramAct = StartCoroutine(TramStationDisembarkation());
                    _tramStady++;
                }
                break;
            case TramStady.Movement:
                MovementToPoint(_objectMovementPoint[_numberPoint].position, _objectMovementPoint.Length - 1);
                break;
            case TramStady.Disembarkation:
            case TramStady.Boarding:
                // wait
                break;
            case TramStady.Departure:
                MovementToPoint(_objectMovementPoint[_numberPoint].position, _objectMovementPoint.Length - 1);
                break;
        }
    }

    public void RestartWaveTram()
    {
        gameObject.SetActive(true);
        _numberPoint = 0;
        _tramStady = 0;
        transform.position = _objectMovementPoint[_numberPoint].position;
        if (_coroutineTramAct != null) StopCoroutine(_coroutineTramAct);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Finish" && Mathf.Abs(transform.position.x) < 0.5f)
        {
            _tramStady = TramStady.Boarding;
            _landingStop = other.GetComponent<FinishStation>().GetCountList();
            if (_landingStop)
            {
                if (_timerDestroyVisitor < 0)
                {
                    other.GetComponent<FinishStation>().GetFirstVisitor();
                    _timerDestroyVisitor = _speedSpawnVisitor;
                }
                else _timerDestroyVisitor -= Time.deltaTime;
            }
            else 
                _tramStady = TramStady.Departure;
        }
    }

    IEnumerator TramStationDisembarkation()
    {
        for (int i = 0; i< _initialSettings.InitialSettingTram.CounterPassanger; i++)
        {
            yield return new WaitForSeconds(_speedSpawnVisitor);
            Instantiate(_prefabVisitor, transform.position, new Quaternion()).GetComponent<VisitorMovement>().SetScriptData(_sceneScriptData, _initialSettings.SpeedVisitor, _levelTramData);
        }
        yield return new WaitForSeconds(_speedSpawnVisitor);
        _tramStady++;
    }

}