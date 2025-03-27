using UnityEngine;

[CreateAssetMenu(fileName = "AbilityData", menuName = "Scriptable Objects/AbilityData")]
public class AbilityData : ScriptableObject
{
    public string abilityName;
    [TextArea] public string description;
    public ElementType element;
    public AbilityType abilityType;
    public float cooldown;
    public float baseDamage;
    public float duration;
    public int spellSpawnNumber = 1;

    public ColorData colorData;

    public Sprite abilityIcon;
    public GameObject abilityPrefab;

    public virtual void Activate(Transform spawnPoint)
    {
        if(abilityPrefab != null)
        {
            
            Ability abilityLogic = abilityPrefab.GetComponent<Ability>();

            if(abilityLogic != null )
            {

                abilityLogic.abilityData = this;
                //Debug.Log($"{abilityName} executed");
                abilityLogic.Execute();
                
            }
        }
        else
        {
            Debug.LogWarning("No ability logic here dummy");
        }
    }
}
//GameObject abilityObject = Instantiate(abilityPrefab, spawnPoint.position, spawnPoint.rotation);
