using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DamageTimeManager : MonoBehaviour
{
    public static DamageTimeManager instance;

    public float combatDuration;
    public float countdownTime = 3;
    public float timer;
    public bool isCombatActive;
    public bool canAim = false;

    [Header("Countdown things")]
    public TextMeshProUGUI countdownText;
    public GameObject countdownPanel;
    private Coroutine countdownCoroutine;


    public EndCombatManager endCombatManager;
    public List<Enemy> allEnemies = new List<Enemy>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void OnEnable()
    {
        
    }

    void Start()
    {
        EnemySpawner.instance.combatDuration = combatDuration;
        //endCombatManager = FindFirstObjectByType<EndCombatManager>();
        
        if (countdownCoroutine == null)
        {
            countdownPanel.SetActive(true);
            countdownCoroutine = StartCoroutine(StartCombatCountdown());
        }

    }
    private IEnumerator StartCombatCountdown()
    {
        isCombatActive = false;
        canAim = false;
        for (int i = (int)countdownTime; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        EnemySpawner.instance.StartSpawning();
        canAim = true;
        countdownText.text = "GO!";
        yield return new WaitForSeconds(0.5f);

        countdownPanel.SetActive(false);
        countdownCoroutine = null; // Reset so it can restart next time
        StartCombat();
    }
    void Update()
    {
        if (!isCombatActive) return;

        timer -= Time.deltaTime;

        if (DamageUIManager.instance != null)
        {
            DamageUIManager.instance.UpdateCombatTimer(timer);
        }

        if (timer <= 0)
        {
            EndCombat();
        }
    }
    public void StartCombat()
    {
        timer = combatDuration;
        isCombatActive = true;
        //allEnemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None).ToList();
        
    }
    public void EndCombat()
    {
        isCombatActive = false;
        ShowCombatResults();
    }
    private void ShowCombatResults()
    {
        Dictionary<ElementType, float> totalDamage = new Dictionary<ElementType, float>();

        foreach (ElementType element in System.Enum.GetValues(typeof(ElementType)))
        {
            totalDamage[element] = 0;
        }

        foreach (Enemy enemy in allEnemies)
        {
            Dictionary<ElementType, float> enemyDamage = enemy.GetDamageByElement();
            foreach (var element in enemyDamage)
            {
                totalDamage[element.Key] += element.Value;
            }
        }
        if (endCombatManager != null)
        {
            endCombatManager.ShowEndCombatPanel(totalDamage);
        }
    }
}
