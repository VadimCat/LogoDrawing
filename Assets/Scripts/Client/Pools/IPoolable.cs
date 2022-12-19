namespace Client.Pools
{
    public interface IPoolable
    {
        void Spawn();
        void DeSpawn();
    }
}