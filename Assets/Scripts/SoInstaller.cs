using Zenject;
using UnityEngine;

[CreateAssetMenu(fileName ="SoInstaller", menuName = "Create SO/SO Installer")]
public class SoInstaller : ScriptableObjectInstaller
{
    [SerializeField]
    private InitialSettings _initialSettings;

    public override void InstallBindings()
    {
        Container.BindInstances(_initialSettings);
    }
}
