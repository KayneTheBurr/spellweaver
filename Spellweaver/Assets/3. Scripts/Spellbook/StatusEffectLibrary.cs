using System.Collections.Generic;
using UnityEngine;

public class StatusEffectLibrary : MonoBehaviour
{
    public static StatusEffectLibrary instance;

    [SerializeField] private List<StatusEffectData> allStatusEffects = new List<StatusEffectData>();
    private Dictionary<Status, StatusEffectData> statusEffectLookup = new Dictionary<Status, StatusEffectData>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }

        DontDestroyOnLoad(gameObject);
        InitializeDatabase();
    }
    private void InitializeDatabase()
    {
        foreach (StatusEffectData data in allStatusEffects)
        {
            if (!statusEffectLookup.ContainsKey(data.statusType))
            {
                statusEffectLookup.Add(data.statusType, data);
            }
        }
    }
    public StatusEffectData GetStatusEffectData(Status status)
    {
        return statusEffectLookup.ContainsKey(status) ? statusEffectLookup[status] : null;
    }
    public List<StatusEffectData> GetAllStatusList()
    {
        return allStatusEffects;
    }
}
