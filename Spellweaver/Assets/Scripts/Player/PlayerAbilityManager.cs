using UnityEngine;

public class PlayerAbilityManager : MonoBehaviour
{
    PlayerManager player;

    public AbilityData[] spellList = new AbilityData[4];
    public AbilityData basicAttack;

    private void Awake()
    {
        player = GetComponent<PlayerManager>();
    }
    public void AssignBasicAttack(AbilityData ability)
    {
        basicAttack = ability;
        //Debug.Log($"Assigned {ability.abilityName} as Basic Attack");
    }
    public void AssignAbility(int spellSlot, AbilityData ability)
    {
        if (spellSlot >= 0 && spellSlot < spellList.Length)
        {
            spellList[spellSlot] = ability;
            //Debug.Log($"Assigned {ability.abilityName} to slot {spellSlot}");
        }
    }
    public void RemoveAbility(int spellSlot)
    {
        spellList[spellSlot] = null;
    }
    public void ConfirmLoadout()
    {
        player.ConfirmLoadout(spellList, basicAttack);
        //Debug.Log("Loadout confirmed, starting DPS phase.");
    }
    public void ResetSpells()
    {
        basicAttack = null;
        for (int i = 0; i < spellList.Length; i++)
        {
            spellList[i] = null;
        }
    }


    private void siphonByDistance()
    {
        int siphonRadius = 4;

        Collider[] nearbyOrbs = Physics.OverlapSphere(gameObject.transform.position, siphonRadius);
        int numberOfOrbsInRadius = 0;

        foreach ( var col in nearbyOrbs)
        {
            Projectile orb = col.GetComponentInParent<Projectile>();
            
            if(orb != null)
            {
                //everything in here MUST be an orb
                if(orb.isActiveAndEnabled)//use siphonable here
                {
                    //do stuff on siphonable orbs
                    numberOfOrbsInRadius++;
                }
                else
                {
                    //do stuff on non siphonable orbs here 
                }
            }
            else
            {
                //this will occur for everything NOT an orb,
                //so if you wanted the ground/walls to react for some reason
            }

        }

        if (numberOfOrbsInRadius > 0)
        {
            //if there is a siphonable orb in my radius, do this 
        }
        else
        {
            //if there are no siphonable orbs around me, do this 
        }
    }
}


