using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [Header("Flags")]
    public bool isPerformingAction;

    [Header("Ability Info")]
    public List<AbilityData> mySpells = new List<AbilityData>();
    public AbilityData basicAttack;
    public Transform spellSpawnPoint1;
    public Transform spellSpawnPoint2;
    public Transform spellSpawnPoint3;

    [Header("Player Sub Managers")]
    public PlayerAbilityManager playerAbilityManager {  get; private set; }
    public PlayerCombatManager playerCombatManager { get; private set; }

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

        playerAbilityManager = GetComponent<PlayerAbilityManager>();
        playerCombatManager = GetComponent<PlayerCombatManager>();

        //spellSpawnPoint = GetComponentInChildren<SpellSpawnPoint>();
        //if (spellSpawnPoint == null) Debug.LogWarning("no spawn point set fix me!");
    }
    void Start()
    {
        PlayerCamera.instance.player = this;
        PlayerInputManager.instance.player = this;
    }

    void Update()
    {
        PlayerCamera.instance.HandleAllCameraAction();
    }

    public delegate void LoadoutConfirmed();
    public event LoadoutConfirmed OnLoadoutConfirmed;

    public void ConfirmLoadout(AbilityData[] spells, AbilityData attack)
    {
        //Debug.Log("Confirmed please?");
        SetAbilites(spells, attack);
        OnLoadoutConfirmed?.Invoke();
        playerCombatManager.SetCoolDownsToZero();
    }

    public void SetAbilites(AbilityData[] spells, AbilityData attack)
    {
        basicAttack = attack;
        mySpells.Clear();

        foreach (AbilityData spell in spells)
        {
            mySpells.Add(spell);
        }
    }
    public void ResetAllAbilities()
    {
        basicAttack = null;
        mySpells.Clear();
        //Debug.Log("All abilites reset");
    }
    public Transform GetSpellSpawnPoint(int spawnNumber)
    {
        if(spawnNumber == 1)
        {
            return spellSpawnPoint1;
        }
        else if(spawnNumber ==2)
        {
            return spellSpawnPoint2;
        }
        else if(spawnNumber == 3)
        {
            return spellSpawnPoint3;
        }
        else
        {
            return transform;
        }
    }
}
