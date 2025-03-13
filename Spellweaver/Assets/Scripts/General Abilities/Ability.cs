using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public AbilityData abilityData;
    

    public virtual void Execute()
    {
        Debug.Log($"Perform {abilityData.abilityName}");
    }
}
