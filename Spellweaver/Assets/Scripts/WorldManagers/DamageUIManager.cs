using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageUIManager : MonoBehaviour
{
    public static DamageUIManager instance;

    public TextMeshProUGUI effectName;

    [Header("AbilityHUD")]
    public Image[] abilityIcons = new Image[4];
    public Image[] cooldownClocks = new Image[4];
    public Image basicAttackIcon;
    public Image basicAttackClock;

    [Header("Enemy Damage real time menu ")]
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI effectText;
    private Enemy trackedEnemy;

    [Header("Combat Timer UI")]
    public TextMeshProUGUI combatTimerText;

    [Header("Cooldown Numbers")]
    private float[] cooldownTimers = new float[4]; // Current cooldowns
    private float[] maxCooldowns = new float[4];  // Max cooldown per ability
    private float basicCooldownTimer;
    private float basicMaxCooldown;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        InitializeAbilityIcons();

        Enemy enemy = FindFirstObjectByType<Enemy>(); 
        if (enemy != null)
        {
            SetTrackedEnemy(enemy);
        }
        for (int i = 0; i < cooldownClocks.Length; i++)
        {
            cooldownClocks[i].fillAmount = 0;
        }
        basicAttackClock.fillAmount = 0;
    }
    public void UpdateCombatTimer(float timeRemaining)
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        combatTimerText.text = $"Time Left: {minutes:00}:{seconds:00}";
    }
    public void InitializeAbilityIcons()
    {
        var abilityManager = PlayerManager.instance.playerAbilityManager;

        for (int i = 0; i < abilityIcons.Length; i++)
        {
            if (abilityManager.spellList[i] != null)
            {
                abilityIcons[i].sprite = abilityManager.spellList[i].abilityIcon;
                abilityIcons[i].gameObject.SetActive(true);
            }
            else
            {
                abilityIcons[i].gameObject.SetActive(false);
            }
        }

        if (abilityManager.basicAttack != null)
        {
            basicAttackIcon.sprite = abilityManager.basicAttack.abilityIcon;
            basicAttackIcon.gameObject.SetActive(true);
        }
        else
        {
            basicAttackIcon.gameObject.SetActive(false);
        }
    }
    public void SetTrackedEnemy(Enemy enemy)
    {
        trackedEnemy = enemy;
        UpdateUI();
    }
    private void Update()
    {
        if (trackedEnemy != null)
            UpdateUI();
    }
    private void UpdateUI()
    {
        if (trackedEnemy == null) return;

        // Update damage breakdown
        string damageLog = "Damage Taken:\n";
        foreach (var entry in trackedEnemy.GetDamageByElement())
        {
            damageLog += $"{entry.Key}: {entry.Value}\n";
        }
        damageText.text = damageLog;

        // Update active status effects
        string effectLog = "Status Effects:\n";
        foreach (var effect in trackedEnemy.activeEffects)
        {
            effectLog += $"{effect.GetType().Name} ({effect.duration - effect.elapsedTime}s)\n";
        }
        effectText.text = effectLog;
    }
    public void UpdateCooldowns()
    {
        for (int i = 0; i < cooldownTimers.Length; i++)
        {
            if (cooldownTimers[i] > 0)
            {
                cooldownTimers[i] -= Time.deltaTime;
                cooldownClocks[i].fillAmount = cooldownTimers[i] / maxCooldowns[i];
            }
            else
            {
                cooldownClocks[i].fillAmount = 0;
            }
        }

        if (basicCooldownTimer > 0)
        {
            basicCooldownTimer -= Time.deltaTime;
            basicAttackClock.fillAmount = basicCooldownTimer / basicMaxCooldown;
        }
        else
        {
            basicAttackClock.fillAmount = 0;
        }
    }
    public void StartCooldown(int slot, float cooldown)
    {
        if (slot >= 0 && slot < cooldownTimers.Length)
        {
            cooldownTimers[slot] = cooldown;
            maxCooldowns[slot] = cooldown;
            cooldownClocks[slot].fillAmount = 1;
        }
    }
    public void StartBasicCooldown(float cooldown)
    {
        basicCooldownTimer = cooldown;
        basicMaxCooldown = cooldown;
        basicAttackClock.fillAmount = 1;
    }
    public void UpdateEffectName(string effect)
    {
        effectName.text = effect;
    }
}
