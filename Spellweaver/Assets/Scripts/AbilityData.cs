using UnityEngine;

[CreateAssetMenu(fileName = "AbilityData", menuName = "Scriptable Objects/AbilityData")]
public class AbilityData : ScriptableObject
{
    public string abilityName;
    public ElementType element;
    public float cooldown;
    public GameObject abilityPrefab;

    public virtual void Activate(Transform spawnPoint)
    {
        if(abilityPrefab != null)
        {
            GameObject abilityObject = Instantiate(abilityPrefab, spawnPoint.position, Quaternion.identity);

            Ability abilityLogic = abilityObject.GetComponent<Ability>();

            if(abilityLogic != null )
            {
                abilityLogic.element = element;
                abilityLogic.Execute();
            }
        }
        else
        {
            Debug.LogWarning("No ability logic here dummy");
        }

    }




}
