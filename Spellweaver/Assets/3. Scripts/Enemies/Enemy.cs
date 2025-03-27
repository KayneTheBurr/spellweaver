using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public struct DamageInstance
{
    public float damage;
    public ElementType element;
    public Ability sourceAbility;

    public DamageInstance(float damage, ElementType element, Ability sourceAbility)
    {
        this.damage = damage;
        this.element = element;
        this.sourceAbility = sourceAbility;
    }
}
public class Enemy : MonoBehaviour
{
    [SerializeReference]
    public List<StatusEffect> activeEffects = new List<StatusEffect>();

    public Dictionary<ElementType, float> damageByElement = new Dictionary<ElementType, float>();
    private Dictionary<StatusEffect, float> activeMultipliers = new Dictionary<StatusEffect, float>();
    private Dictionary<ElementType, float> elementMultipliers = new Dictionary<ElementType, float>(); //mukltipliers for element specific stuff 
    
    [Header("Damage Calcs")]
    public float damageMultiplier = 1.0f;
    public GameObject floatingDamagePrefab;
    public GameObject floatingDotDmgPrefab;
    
    [Header("Movement values")]
    public bool isMoving = false;
    public float speedMult = 1.0f;
    public float fallSpeed = 10f;
    public float moveSpeed = 1.5f;
    private int currentWaypointIndex = 0;
    private bool isFalling = true;
    [SerializeField] private float waypointWaitTime = 0.5f;

    private Transform[] waypoints;
    private Rigidbody rb;
    private EnemyElementMixer elementMixer;

    public DamageInstance? lastDamageSource;

    public float fireMult;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        elementMixer = GetComponent<EnemyElementMixer>();

        //initializes the mult dictionary with all multipleris at 1.0
        foreach (ElementType element in System.Enum.GetValues(typeof(ElementType)))
        {
            elementMultipliers[element] = 1.0f;  
        }

    }
    private void Start()
    {
        foreach (ElementType element in System.Enum.GetValues(typeof(ElementType)))
        {
            damageByElement[element] = 0;
        }
    }


    public void BeginFall(Vector3 groundPos, Transform[] mywayPoints, float speedMultiplier)
    {
        waypoints = mywayPoints; //given from waypoint manager
        moveSpeed *= speedMultiplier; //inc speed for each new enemy spawned 

        StartCoroutine(FallToGround(groundPos));
    }
    private IEnumerator FallToGround(Vector3 targetPosition)
    {
        while (transform.position.y > targetPosition.y + 0.1f)
        {
            rb.linearVelocity = new Vector3(0, -fallSpeed, 0);
            yield return null;
        }

        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true; // now move using waypoints and not physics 
        isFalling = false;

        StartMovement();
    }
    public void StartMovement()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        currentWaypointIndex = 0;
        isMoving = true;
    }
    private void HandleMovement()
    {
        if (waypoints.Length == 0) return;
        Transform targetWaypoint = waypoints[currentWaypointIndex];

        if (Vector3.Distance(transform.position, targetWaypoint.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, 
                targetWaypoint.position, (moveSpeed * speedMult * Time.deltaTime));
        }
        else
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            StartCoroutine(WaitAtWaypoint());
        }
    }
    private IEnumerator WaitAtWaypoint()
    {
        isMoving = false;
        yield return new WaitForSeconds(waypointWaitTime);
        isMoving = true;
    }
    public float GetMoveSpeed()
    {
        
        return moveSpeed * speedMult; // Adjusted by status effects
    }
    public void ModifySpeedMultiplier(float multiplier)
    {
        speedMult *= multiplier;
    }
    public void RemoveSpeedMultiplier(float multiplier)
    {
        speedMult /= multiplier;
    }


    private void Update()
    {
        //for testing
        fireMult = elementMultipliers[ElementType.Fire];

        if(!isFalling && isMoving)
        {
            HandleMovement();
        }

        float deltaTime = Time.deltaTime;
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            activeEffects[i].UpdateEffect(deltaTime);
        }
    }
    public void TakeDamageOverTime(float damage, ElementType element)
    {
        float finalDamage = damage * damageMultiplier;
        damageByElement[element] += finalDamage;

        if (floatingDotDmgPrefab != null)
        {


            float randomX = Random.Range(-0.5f, 0.5f);
            float randomY = Random.Range(-0.2f, 0.5f);
            float randomZ = Random.Range(-0.5f, 0.5f);
            Vector3 randomOffset = new Vector3(randomX, randomY, randomZ);
            Vector3 spawnPosition = transform.position + Vector3.up * 4f + randomOffset;

            GameObject damageTextObj = Instantiate(floatingDotDmgPrefab, spawnPosition, Quaternion.identity);
            FloatingDamageNumbers floatingDamage = damageTextObj.GetComponent<FloatingDamageNumbers>();

            if (floatingDamage != null)
            {
                Color damageColor = GetElementColor(element); // Function to set color based on element
                floatingDamage.Initialize(finalDamage, damageColor);
            }
        }
    }
    public void TakeDamage(float damage, ElementType element, Ability sourceAbility)
    {
        float finalDamage = damage * damageMultiplier * elementMultipliers[element];
        damageByElement[element] += finalDamage;

        lastDamageSource = new DamageInstance(finalDamage, element, sourceAbility);

        if (floatingDamagePrefab != null)
        {
            float randomX = Random.Range(-0.5f, 0.5f);
            float randomY = Random.Range(-0.2f, 0.5f);
            float randomZ = Random.Range(-0.5f, 0.5f);
            Vector3 randomOffset = new Vector3(randomX, randomY, randomZ);
            Vector3 spawnPosition = transform.position + Vector3.up * 4f + randomOffset;

            GameObject damageTextObj = Instantiate(floatingDamagePrefab, spawnPosition, Quaternion.identity);
            FloatingDamageNumbers floatingDamage = damageTextObj.GetComponent<FloatingDamageNumbers>();

            if (floatingDamage != null)
            {
                Color damageColor = GetElementColor(element); // Function to set color based on element
                floatingDamage.Initialize(finalDamage, damageColor);
            }
        }
    }
    private Color GetElementColor(ElementType element)
    {
        switch (element)
        {
            case ElementType.Fire:
                return new Color(1f, 0f, 0f); 
            case ElementType.Ice:
                return new Color(0f, 0.0f, 1f);
            case ElementType.Lightning:
                return new Color(1f, 0f, 1f); 
            case ElementType.Poison:
                return new Color(0.3f, 1f, 0.3f); 
            default:
                return Color.white; 
        }
    }

    public bool AddEffect(StatusEffect effect)
    {
        bool reactionTriggered = elementMixer.CheckForElementalReaction(effect);
        if (reactionTriggered)
        {
            
            return false; // reaction triggered, no need to add the effect
        }

        Debug.Log($"Apply {effect}");
        activeEffects.Add(effect);
        LogActiveEffects();

        return true;
    }
    public void RemoveEffect(StatusEffect effect)
    {
        Debug.Log($"Remove {effect}");
        activeEffects.Remove(effect);
        LogActiveEffects();
    }
    public void ModifyDamageMultiplier(StatusEffect effect, float multiplier)
    {
        if (activeMultipliers.ContainsKey(effect))
        {
            
            activeMultipliers[effect] = multiplier; // If the effect exists, update it
        }
        else
        {
            activeMultipliers.Add(effect, multiplier);
        }
        
        RecalculateTotalMultiplier();
    }
    public void RemoveDamageMultiplier(StatusEffect effect, float multiplier)
    {
        if (activeMultipliers.ContainsKey(effect))
        {
            activeMultipliers.Remove(effect);
        }
            
        RecalculateTotalMultiplier();
    }
    private void RecalculateTotalMultiplier()
    {
        float totalMult = 1.0f; //Reset from base before doing calcs

        foreach (float mult in activeMultipliers.Values)
        {
            totalMult *= mult;
        }

        damageMultiplier = totalMult;
    }

    public void ModifyElementMultiplier(ElementType element, float multiplier)
    {
        if (!elementMultipliers.ContainsKey(element))
        {
            elementMultipliers[element] = 1.0f;
        }
        elementMultipliers[element] *= multiplier;
    }
    public void RemoveElementMultiplier(ElementType element, float multiplier)
    {
        if (elementMultipliers.ContainsKey(element))
        {
            elementMultipliers[element] /= multiplier;
        }
    }

    
    public Dictionary<ElementType, float> GetDamageByElement()
    {
        return damageByElement;
    }
    public T GetEffect<T>() where T : StatusEffect
    {
        return activeEffects.OfType<T>().FirstOrDefault();
    }
    public bool HasEffect<T>() where T : StatusEffect
    {
        return activeEffects.OfType<T>().Any();
    }


    private void LogActiveEffects()
    {
        if (activeEffects.Count == 0)
        {
            Debug.Log("No active status effects.");
        }
        else
        {
            string effectLog = "Active Effects: ";
            foreach (var effect in activeEffects)
            {
                effectLog += $"{effect.GetType().Name} ({effect.duration - effect.elapsedTime:F2}s), ";
            }
            Debug.Log(effectLog);
        }

        if (activeMultipliers.Count == 0)
        {
            Debug.Log("No active damage multipliers.");
        }
        else
        {
            string multiplierLog = "Active Damage Multipliers: ";
            foreach (var kvp in activeMultipliers)
            {
                multiplierLog += $"{kvp.Key.GetType().Name}: x{kvp.Value:F2}, ";
            }
            Debug.Log(multiplierLog);
        }
    }
    private void LogElementalDamage()
    {
        string log = "Elemental Damage Breakdown: ";
        foreach (var entry in damageByElement)
        {
            log += $"{entry.Key}: {entry.Value} | ";
        }
        //Debug.Log(log);
    }
}
