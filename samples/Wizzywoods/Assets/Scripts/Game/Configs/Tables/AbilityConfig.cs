//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;


namespace Game.Config.Tables
{
public sealed partial class AbilityConfig :  Bright.Config.BeanBase 
{
    public AbilityConfig(ByteBuf _buf) 
    {
        Id = _buf.ReadString();
        Type = _buf.ReadString();
        ActPreDelay = _buf.ReadFloat();
        ActDuration = _buf.ReadFloat();
        ActPostDelay = _buf.ReadFloat();
        TargetingType = (ETargetingType)_buf.ReadInt();
        PrefabRes = _buf.ReadString();
        IconRes = _buf.ReadString();
        PostInit();
    }

    public static AbilityConfig DeserializeAbilityConfig(ByteBuf _buf)
    {
        return new AbilityConfig(_buf);
    }

    /// <summary>
    /// 这是Id
    /// </summary>
    public string Id { get; private set; }
    /// <summary>
    /// 类型
    /// </summary>
    public string Type { get; private set; }
    /// <summary>
    /// 动作前摇
    /// </summary>
    public float ActPreDelay { get; private set; }
    /// <summary>
    /// 动作持续
    /// </summary>
    public float ActDuration { get; private set; }
    /// <summary>
    /// 动作后摇
    /// </summary>
    public float ActPostDelay { get; private set; }
    /// <summary>
    /// 目标选取类型
    /// </summary>
    public ETargetingType TargetingType { get; private set; }
    /// <summary>
    /// 预制体资源
    /// </summary>
    public string PrefabRes { get; private set; }
    /// <summary>
    /// 图标资源
    /// </summary>
    public string IconRes { get; private set; }

    public const int __ID__ = -39891732;
    public override int GetTypeId() => __ID__;

    public  void Resolve(Dictionary<string, object> _tables)
    {
        PostResolve();
    }

    public  void TranslateText(System.Func<string, string, string> translator)
    {
    }

    public override string ToString()
    {
        return "{ "
        + "Id:" + Id + ","
        + "Type:" + Type + ","
        + "ActPreDelay:" + ActPreDelay + ","
        + "ActDuration:" + ActDuration + ","
        + "ActPostDelay:" + ActPostDelay + ","
        + "TargetingType:" + TargetingType + ","
        + "PrefabRes:" + PrefabRes + ","
        + "IconRes:" + IconRes + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}