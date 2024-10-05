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
public sealed partial class ExampleSingleton :  Bright.Config.BeanBase 
{
    public ExampleSingleton(ByteBuf _buf) 
    {
        NewbieDiscountTimes = _buf.ReadInt();
        AffectionIncrementLimit = _buf.ReadFloat();
        GiftCooldown = _buf.ReadLong();
        PostInit();
    }

    public static ExampleSingleton DeserializeExampleSingleton(ByteBuf _buf)
    {
        return new Examples.ExampleSingleton(_buf);
    }

    /// <summary>
    /// 新手商店优惠次数
    /// </summary>
    public int NewbieDiscountTimes { get; private set; }
    /// <summary>
    /// 单日好感度增加上限
    /// </summary>
    public float AffectionIncrementLimit { get; private set; }
    /// <summary>
    /// 礼物冷却时间
    /// </summary>
    public long GiftCooldown { get; private set; }

    public const int __ID__ = 209511622;
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
        + "NewbieDiscountTimes:" + NewbieDiscountTimes + ","
        + "AffectionIncrementLimit:" + AffectionIncrementLimit + ","
        + "GiftCooldown:" + GiftCooldown + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}