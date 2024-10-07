namespace Game.Framework
{
    /// <summary>
    /// <para>Data object to be serialized</para>
    /// <para>被序列化的数据对象</para>
    /// </summary>
    public interface ISerializee
    {
        /// <summary>
        /// <para>Pre-serialization operation, where the non-serializable fields are converted to serializable fields</para>
        /// <para>序列化前操作，在这里将无法序列化的字段转换为可序列化的字段（例如BigInteger 转 string）</para>
        /// </summary>
        void PreSerialize();
        
        /// <summary>
        /// <para>After deserialization, add empty fields and assign values to fields that cannot be serialized.</para>
        /// <para>反序列化后操作，在这里补充为空的字段、给无法序列化的字段赋值</para>
        /// </summary>
        void PostDeserialize();
    }
    
}