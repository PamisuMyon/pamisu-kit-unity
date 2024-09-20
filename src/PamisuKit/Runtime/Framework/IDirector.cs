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
    }
    
    public enum DirectorMode
    {
        Normal, Global
    }
    
}