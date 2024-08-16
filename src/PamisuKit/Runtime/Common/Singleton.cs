namespace PamisuKit.Common
{
    public class Singleton<T> where T : class, new()
    {
        private static T m_Instance;
        
        private static readonly object LockObj = new();
        
        public static T Instance
        {
            get
            {
                if (null == m_Instance)
                {
                    lock (LockObj)
                    {
                        m_Instance ??= new T();
                    }
                }
                return m_Instance;
            }
        }
        
    }
}