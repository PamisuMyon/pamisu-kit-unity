using System;
using UnityEngine;

namespace PamisuKit.Framework
{
    public interface IDirector
    {
        Transform Trans { get; }
        
        GameObject Go { get; }
        
        Region Region { get; }
        
        TSystem GetSystem<TSystem>() where TSystem : class, ISystem;

        ISystem GetSystem(Type type);

        void RegisterService<TService>(TService service) where TService : class;

        TService GetService<TService>() where TService : class;

        bool RemoveService<TService>();

        bool RemoveService(object service);

    }
    
    public enum DirectorMode
    {
        Normal, Global
    }
    
}