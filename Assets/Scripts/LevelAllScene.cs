using UnityEngine;
using Zenject;

public class LevelAllScene : MonoBehaviour
{
    [Inject] private SceneScriptData _sceneScriptData;
    [Inject] private InitialSettings _initialSettings;

    private int[] _levelProcedure;
    private int _playerMoney;

    private void ChangePlayerMoney(int value)
    {
        _playerMoney += value;
        _sceneScriptData.UICanvas.SetTextMoneyPlayer(_playerMoney);
    }

    private void Start()
    {
        _levelProcedure = new int[_sceneScriptData.ProcedureList.GetCountArray()];
        _playerMoney = _initialSettings.InitialPlayerMoney;
        _sceneScriptData.UICanvas.SetTextMoneyPlayer(_playerMoney);
    }

    private int MultiplyUpgrade(int number, float multiply)
    {
        return (int)(Mathf.Ceil(number * multiply));
    }


    public string GetCostTramUpdate()
    {
        return _initialSettings.InitialSettingTram.CostUpgradeTram.ToString();
    }

    public bool UpgradeTram()
    {
        if (_playerMoney >= _initialSettings.InitialSettingTram.CostUpgradeTram)
        {
            ChangePlayerMoney(-_initialSettings.InitialSettingTram.CostUpgradeTram);
            _initialSettings.InitialSettingTram.CounterPassanger++;
            _initialSettings.InitialSettingTram.CostUpgradeTram *= 5;
            return true;
        }
        else return false;
    }

    public bool UpgradeProcedure(int idProc)
    {
        if (_playerMoney >= _initialSettings.InitialSettingProcedure.CostUpgradeProcedure[idProc])
        {
            if (idProc==0)
            {
                if (_levelProcedure[0] ==0)
                {
                    _sceneScriptData.TramStation.StartTramMovement();
                }
            }
            ChangePlayerMoney(-_initialSettings.InitialSettingProcedure.CostUpgradeProcedure[idProc]);
            _levelProcedure[idProc]++;
            _initialSettings.InitialSettingProcedure.CostUpgradeProcedure[idProc] = MultiplyUpgrade(_initialSettings.InitialSettingProcedure.CostUpgradeProcedure[idProc], 1.15f);
            _initialSettings.InitialSettingProcedure.IncomeProcedure[idProc] = MultiplyUpgrade(_initialSettings.InitialSettingProcedure.IncomeProcedure[idProc], 1.15f);

            if (_levelProcedure[idProc] == 1)
            {

                _sceneScriptData.ProcedureList.SetActiveProcedure(idProc);
            }
            return true;
        }
        else return false;
    }
    
    public void MoneyArrived(int idProc)
    {
        ChangePlayerMoney(_initialSettings.InitialSettingProcedure.IncomeProcedure[idProc]);
    }

    public string GetCostUpgrade(int idProc)
    {
        return _initialSettings.InitialSettingProcedure.CostUpgradeProcedure[idProc].ToString();
    }
}