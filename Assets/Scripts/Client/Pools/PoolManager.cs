namespace Client.Pools
{
    public class PoolManager
    {
        // private Dictionary<Type, Pool> pools = new Dictionary<Type, Pool>();
        //
        // public void CreatePool<T>() where T : MonoBehaviour
        // {
        //     if (!pools.ContainsKey(typeof(T)))
        //     {
        //         pools.Add(typeof(T), new Pool());
        //     }
        //
        // }
        //
        // public T Spawn<T>(GameObject prefab, Vector3 position = default, Quaternion rotation = default,
        //     Transform parent = null) where T : MonoBehaviour
        // {
        //     var val = pools[typeof(T)].Spawn(prefab, position, rotation, parent);
        //     return val.GetComponent<T>();
        // }
        //
        //
        // public void DeSpawn<T>(GameObject obj)
        // {
        //     pools[typeof(T)].DeSpawn(obj);
        // }
        //
        //
        // public void Dispose()
        // {
        //     foreach (var poolsValue in pools.Values)
        //     {
        //         poolsValue.Dispose();
        //     }
        //
        //     pools.Clear();
        // }
    }
}