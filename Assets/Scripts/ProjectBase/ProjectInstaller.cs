using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reflex.Core;

public class ProjectInstaller : MonoBehaviour, IInstaller
{
    public void InstallBindings(ContainerDescriptor descriptor)
    {
        descriptor.AddSingleton(typeof(ContainerDescriptor));
        //throw new System.NotImplementedException();
    }
}
