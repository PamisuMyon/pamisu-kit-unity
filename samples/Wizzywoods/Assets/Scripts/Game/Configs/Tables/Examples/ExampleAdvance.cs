//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;


namespace Game.Config.Tables.Examples
{
public sealed partial class ExampleAdvance :  Bright.Config.BeanBase 
{
    public ExampleAdvance(ByteBuf _buf) 
    {
        {int __n0 = System.Math.Min(_buf.ReadSize(), _buf.Size);SomeArray1 = new int[__n0];for(var __index0 = 0 ; __index0 < __n0 ; __index0++) { int __e0;__e0 = _buf.ReadInt(); SomeArray1[__index0] = __e0;}}
        {int __n0 = System.Math.Min(_buf.ReadSize(), _buf.Size);SomeArray2 = new float[__n0];for(var __index0 = 0 ; __index0 < __n0 ; __index0++) { float __e0;__e0 = _buf.ReadFloat(); SomeArray2[__index0] = __e0;}}
        {int n0 = System.Math.Min(_buf.ReadSize(), _buf.Size);SomeArray3 = new System.Collections.Generic.List<string>(n0);for(var i0 = 0 ; i0 < n0 ; i0++) { string _e0;  _e0 = _buf.ReadString(); SomeArray3.Add(_e0);}}
        {int __n0 = System.Math.Min(_buf.ReadSize(), _buf.Size);SomeArray4 = new int[__n0][];for(var __index0 = 0 ; __index0 < __n0 ; __index0++) { int[] __e0;{int __n1 = System.Math.Min(_buf.ReadSize(), _buf.Size);__e0 = new int[__n1];for(var __index1 = 0 ; __index1 < __n1 ; __index1++) { int __e1;__e1 = _buf.ReadInt(); __e0[__index1] = __e1;}} SomeArray4[__index0] = __e0;}}
        {int n0 = System.Math.Min(_buf.ReadSize(), _buf.Size);SomeMap1 = new System.Collections.Generic.Dictionary<string, float>(n0 * 3 / 2);for(var i0 = 0 ; i0 < n0 ; i0++) { string _k0;  _k0 = _buf.ReadString(); float _v0;  _v0 = _buf.ReadFloat();     SomeMap1.Add(_k0, _v0);}}
        SomeBean1 = Reward.DeserializeReward(_buf);
        SomeBean2 = Reward.DeserializeReward(_buf);
        {int n0 = System.Math.Min(_buf.ReadSize(), _buf.Size);SomeBeanList = new System.Collections.Generic.List<Reward>(n0);for(var i0 = 0 ; i0 < n0 ; i0++) { Reward _e0;  _e0 = Reward.DeserializeReward(_buf); SomeBeanList.Add(_e0);}}
        PostInit();
    }

    public static ExampleAdvance DeserializeExampleAdvance(ByteBuf _buf)
    {
        return new Examples.ExampleAdvance(_buf);
    }

    /// <summary>
    /// 这是一个int数组,用逗号分隔
    /// </summary>
    public int[] SomeArray1 { get; private set; }
    /// <summary>
    /// 这是一个float数组，用分号分隔
    /// </summary>
    public float[] SomeArray2 { get; private set; }
    /// <summary>
    /// 这是一个string列表，用◆分隔
    /// </summary>
    public System.Collections.Generic.List<string> SomeArray3 { get; private set; }
    /// <summary>
    /// 这是一个二维int数组
    /// </summary>
    public int[][] SomeArray4 { get; private set; }
    /// <summary>
    /// 这是一个键为string，值为float的字典
    /// </summary>
    public System.Collections.Generic.Dictionary<string, float> SomeMap1 { get; private set; }
    /// <summary>
    /// 道具id
    /// </summary>
    public Reward SomeBean1 { get; private set; }
    /// <summary>
    /// 也可以这样写成一格
    /// </summary>
    public Reward SomeBean2 { get; private set; }
    /// <summary>
    /// 自定义类型列表
    /// </summary>
    public System.Collections.Generic.List<Reward> SomeBeanList { get; private set; }

    public const int __ID__ = 796461053;
    public override int GetTypeId() => __ID__;

    public  void Resolve(Dictionary<string, object> _tables)
    {
        SomeBean1?.Resolve(_tables);
        SomeBean2?.Resolve(_tables);
        foreach(var _e in SomeBeanList) { _e?.Resolve(_tables); }
        PostResolve();
    }

    public  void TranslateText(System.Func<string, string, string> translator)
    {
        SomeBean1?.TranslateText(translator);
        SomeBean2?.TranslateText(translator);
        foreach(var _e in SomeBeanList) { _e?.TranslateText(translator); }
    }

    public override string ToString()
    {
        return "{ "
        + "SomeArray1:" + Bright.Common.StringUtil.CollectionToString(SomeArray1) + ","
        + "SomeArray2:" + Bright.Common.StringUtil.CollectionToString(SomeArray2) + ","
        + "SomeArray3:" + Bright.Common.StringUtil.CollectionToString(SomeArray3) + ","
        + "SomeArray4:" + Bright.Common.StringUtil.CollectionToString(SomeArray4) + ","
        + "SomeMap1:" + Bright.Common.StringUtil.CollectionToString(SomeMap1) + ","
        + "SomeBean1:" + SomeBean1 + ","
        + "SomeBean2:" + SomeBean2 + ","
        + "SomeBeanList:" + Bright.Common.StringUtil.CollectionToString(SomeBeanList) + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}