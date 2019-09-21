using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UICanvas : MonoBehaviour
{
    [SerializeField] private GameObject _buttonUpgradeProcedure, _buttonUpgradeTram, _textMoneyPlayer;
    [Inject] private SceneScriptData _sceneScriptData;
    [Inject] private LevelAllScene _levelTramData;
    private List<GameObject> _buttonsUpgrade = new List<GameObject>();

    private void Start()
    {
        CreateButtonUpgradeProcedure(0);
        ChangeTextTramButton();
    }

    private void CreateButtonUpgradeProcedure(int idProc)
    {
        if (_buttonsUpgrade.Count != 0)
        {
            _buttonsUpgrade[idProc-1].transform.Find("name").GetComponent<Text>().text = _sceneScriptData.ProcedureList.GetNameProcedure(idProc-1);
            _buttonsUpgrade[idProc-1].transform.Find("text").GetComponent<Text>().text = "Upgrade: " + _levelTramData.GetCostUpgrade(idProc-1) + "$";
        }
        if (_buttonsUpgrade.Count < _sceneScriptData.ProcedureList.GetCountArray() && _buttonsUpgrade.Count == idProc)
        {
            var newButton = Instantiate(_buttonUpgradeProcedure, transform);
            newButton.transform.Find("name").GetComponent<Text>().text = _sceneScriptData.ProcedureList.GetNameProcedure(idProc);
            newButton.transform.Find("text").GetComponent<Text>().text = "Разблокировать: " + _levelTramData.GetCostUpgrade(idProc) + "$";

            newButton.GetComponent<Button>().onClick.AddListener(() => OnClickButtonUnlocked(idProc));

            _buttonsUpgrade.Add(newButton);
        }
    }

    private void OnClickButtonUnlocked(int idProc)
    {
        if (_levelTramData.UpgradeProcedure(idProc))
            CreateButtonUpgradeProcedure(idProc + 1);
    }

    private void ChangeTextTramButton()
    {
        _buttonUpgradeTram.transform.Find("text").GetComponent<Text>().text = "Upgrade Tram: " + _levelTramData.GetCostTramUpdate();
    }

    public void OnClickButtonUpdateTram()
    {
        if (_levelTramData.UpgradeTram())
            ChangeTextTramButton();
    }

    public void SetTextMoneyPlayer(int money)
    {
        _textMoneyPlayer.transform.Find("text").GetComponent<Text>().text = money + "$";
    }
}
