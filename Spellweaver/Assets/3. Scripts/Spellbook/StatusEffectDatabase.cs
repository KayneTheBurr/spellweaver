using System.Collections.Generic;
using UnityEngine;

public class StatusEffectDatabase : MonoBehaviour
{
    public static StatusEffectDatabase instance;

    private HashSet<Status> discoveredEffects = new HashSet<Status>();
    private HashSet<Status> newlyDiscoveredEffects = new HashSet<Status>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void DiscoverEffect(Status status)
    {
        if (!discoveredEffects.Contains(status))
        {
            discoveredEffects.Add(status);
            newlyDiscoveredEffects.Add(status);
            Debug.Log($"discovered {status}");
        }
    }
    public bool IsEffectDiscovered(Status status)
    {
        return discoveredEffects.Contains(status);
    }
    public bool IsEffectNew(Status status)
    {
        return newlyDiscoveredEffects.Contains(status);
    }
    public HashSet<Status> GetAllDiscoveredEffects()
    {
        return new HashSet<Status>(discoveredEffects);
    }
    public HashSet<Status> GetNewEffects()
    {
        return new HashSet<Status>(newlyDiscoveredEffects);
    }
    public void MarkEffectAsViewed(Status status)
    {
        if(newlyDiscoveredEffects.Contains(status))
        {
            newlyDiscoveredEffects.Remove(status);
        }
    }
}
