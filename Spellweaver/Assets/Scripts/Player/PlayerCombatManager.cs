using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCombatManager : MonoBehaviour
{
    private PlayerManager player;
    private int basicAttackCounter = 0;
    public float actionCooldown = 0.8f;
    public float[] abilityCooldowns = new float[4];
    public float basicAttackCooldown;

    private void Awake()
    {
        player = GetComponent<PlayerManager>();
        
    }
    private void Update()
    {
        HandleCooldowns();
    }
    private void HandleCooldowns()
    {
        if (SceneManager.GetActiveScene().buildIndex != 2) return;

        for (int i = 0; i < abilityCooldowns.Length; i++)
        {
            if (abilityCooldowns[i] > 0)
            {
                abilityCooldowns[i] -= Time.deltaTime;
            }
            else
            {
                abilityCooldowns[i] = 0;
            }
        }
        if(basicAttackCooldown > 0)
        {
            basicAttackCooldown -= Time.deltaTime;
        }
        else
        {
            basicAttackCooldown = 0;
        }
        DamageUIManager.instance.UpdateCooldowns();
    }
    public void TrackBasicAttacks()
    {
        basicAttackCounter++;

        if(basicAttackCounter >= 3)
        {
            basicAttackCounter = 0;
            //will trigger status next hit
        }
    }
    public bool DoesThisTriggerStatusEffect()
    {
        return basicAttackCounter == 0;
    }
    public void SetCoolDownsToZero()
    {
        
        for (int i = 0; i < abilityCooldowns.Length; i++)
        {
            abilityCooldowns[i] = 0;
        }
        //Debug.Log("reset all cooldowns");
    }
    public void AttemptBasicAttack()
    {
        if (player.basicAttack == null) return;
        if (player.isPerformingAction) return;
        if (basicAttackCooldown > 0) return;

        StartCoroutine(ActionCooldown());

        player.basicAttack.Activate(transform);
        basicAttackCooldown = player.basicAttack.cooldown;

        DamageUIManager.instance.StartBasicCooldown(player.basicAttack.cooldown);

    }
    public void AttemptAbility(int slotNum)
    {
        Debug.Log($"Try ability {slotNum}");
        int index = slotNum - 1;//which slot in spell slots this is
        
        if (index < 0 || index >= player.mySpells.Count) return;
        
        if (abilityCooldowns[index] > 0) return;
        if (player.isPerformingAction) return;

        StartCoroutine(ActionCooldown());

        player.mySpells[index].Activate(transform);

        // start cooldown timer
        abilityCooldowns[index] = player.mySpells[index].cooldown;
        DamageUIManager.instance.StartCooldown(index, abilityCooldowns[index]);
    }
    private IEnumerator ActionCooldown()//make this require an input time potentially
        //then this would act as an "animation" time for each attack
    {
        player.isPerformingAction = true;
        yield return new WaitForSeconds(actionCooldown);
        player.isPerformingAction = false;
    }
}
