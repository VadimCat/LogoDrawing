using System.Collections.Generic;
using UnityEngine;

public class UpdateService : MonoBehaviour
{
    private List<IUpdatable> updatables = new();

    private void Update()
    {
        
        foreach (var updatable in updatables)
        {
            updatable.OnUpdate();
        }
    }

    public void Add(IUpdatable updatable)
    {
        updatables.Add(updatable);
    }

    public void Remove(IUpdatable updatable)
    {
        updatables.Remove(updatable);
    }
    
}

public interface IUpdatable
{
    public void OnUpdate();
}
