using UnityEngine;

namespace PamisuKit.Framework
{
    public interface IEntity
    {
        Transform Trans { get; }
        
        GameObject Go { get; }
        
        Region Region { get; }
        
        bool IsActive { get; }
        
    }
}