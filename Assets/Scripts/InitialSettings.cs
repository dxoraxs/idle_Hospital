using System;
using UnityEngine;

[CreateAssetMenu(fileName = "InitialSettings", menuName = "Create SO/Initial Settings")]
public class InitialSettings : ScriptableObject
{
    [Space(15)]
    public int InitialPlayerMoney;
    [Range(1, 5)] public float SpeedVisitor;
    [Header("Procedure Settings")]
    public ProcedureSettings InitialSettingProcedure;

    [Header("Tram Settings")]
    public TramSettings InitialSettingTram;

    private void OnValidate()
    {
        if (InitialSettingProcedure.IncomeProcedure.Length != InitialSettingProcedure.CostUpgradeProcedure.Length)
        {
            InitialSettingProcedure.IncomeProcedure = new int[InitialSettingProcedure.CostUpgradeProcedure.Length];
            InitialSettingProcedure.IsOpenProcedures = new bool[InitialSettingProcedure.CostUpgradeProcedure.Length];
        }

        if (InitialSettingTram != null)
            if (InitialSettingTram.CounterPassanger < 1)
                InitialSettingTram.CounterPassanger = 1;
    }
}

[Serializable]public class TramSettings
{
    public int CounterPassanger;
    [Range(0.1f, 1.5f)] public float TimeToSpawnVisitor;
    [Range(4, 12)] public float SpeedTram;
    public int CostUpgradeTram;
    public int TimeToNextWave;
}

[Serializable]public class ProcedureSettings
{
    public int[] CostUpgradeProcedure;
    public int[] IncomeProcedure;
    public bool[] IsOpenProcedures;
}