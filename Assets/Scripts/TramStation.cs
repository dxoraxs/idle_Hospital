using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class TramStation : MonoBehaviour
{
    [SerializeField] private GameObject _prefabTram;
    private int _constTimeToNextWave;
    [SerializeField] private UnityEvent _restartTramWay;
    [Inject] private InitialSettings _initialSettings;
    private float _timerToNextWave = -2;

    private void OnValidate()
    {
        if (_constTimeToNextWave < 1) _constTimeToNextWave = 1;
    }

    private void Start()
    {
        foreach (bool flag in _initialSettings.InitialSettingProcedure.IsOpenProcedures)
        {
            if (flag)
            {
                StartTramMovement();
                return;
            }
        }
    }

    public void StartTramMovement()
    {
        _constTimeToNextWave = _initialSettings.InitialSettingTram.TimeToNextWave;
        ResetPeriodNextWave();
        RestartTram();
    }

    private void Update()
    {
        if (_timerToNextWave > 0) _timerToNextWave -= Time.fixedDeltaTime;
        else if (_timerToNextWave<-1 || _prefabTram.activeSelf)
        {
            return;
        }
        else
        {
            RestartTram();
        }
    }

    public void ResetPeriodNextWave()
    {
        _timerToNextWave = _constTimeToNextWave;
    }

    private void RestartTram()
    {
        ResetPeriodNextWave();
        _restartTramWay.Invoke();
    }

}
