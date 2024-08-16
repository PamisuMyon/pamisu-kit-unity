namespace PamisuKit.Common.Pool
{

    public interface IPoolElement
    {
        void OnSpawnFromPool();

        void OnReleaseToPool();
    }
}