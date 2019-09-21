using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class ProcedureList : MonoBehaviour
{
    [Inject] private InitialSettings _initialSettings;
    private List<Transform> ProceduresBuilding = new List<Transform>();

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            ProceduresBuilding.Add(child);
        }
        for (int i=0; i< ProceduresBuilding.Count;i++)
        {
            if (i < _initialSettings.InitialSettingProcedure.IsOpenProcedures.Length)
                if (_initialSettings.InitialSettingProcedure.IsOpenProcedures[i])
                    ProceduresBuilding[i].gameObject.SetActive(true);
        }
    }

    public void SetActiveProcedure(int idProc)
    {
        ProceduresBuilding[idProc].gameObject.SetActive(true);
    }

    public int GetRandomNumberProcedure()
    {
        return Random.Range(0,(from building in ProceduresBuilding
                    where building.gameObject.activeSelf
                     select building).Count());
    }

    public int GetCountArray()
    {
        return ProceduresBuilding.Count;
    }

    public string GetNameProcedure(int idProc)
    {
        return ProceduresBuilding[idProc].name;
    }

    public Vector3 GetPositionProcedure(int idProc)
    {
        return ProceduresBuilding[idProc].position;
    }

    public Color GetColorIdProcedure(int idProc)
    {
        return ProceduresBuilding[idProc].Find("Building").GetComponent<Renderer>().material.color;
    }
}