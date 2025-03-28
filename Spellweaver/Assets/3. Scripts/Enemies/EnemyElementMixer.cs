using NUnit.Framework;
using NUnit.Framework.Internal.Commands;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyElementMixer : MonoBehaviour
{
    #region
    private Enemy enemy;

    private string elementMixName = "";

    private int poisonStacks = 0;
    private const int maxPoisonStacks = 4;
    public GameObject floorFirePrefab;

    [Header("Effect damage and multipliers")]
    public float infectedSurgeMult = 0.6f;
    public float superChargedMultiplier = 0.8f;
    public float atomizedMultiplier = 3f;
    public float atomizeRadius = 6f;
    public float overloadDamage = 500f;
    public float overloadRadius = 10f;

    [Header("Effect Durations")]
    public float stunDuration = 3.5f;
    public float chargedDuration = 30f;
    public float scorchDuration = 6f;
    public float steamedDuration = 4f;
    public float wetDuration = 6f;
    public float brittleDuration = 5f;
    public float freezeDuration = 2.5f;
    public float cryotoxinDuration = 2f;
    public float superchargedDuration = 5f;

    [Header("DoT Values")]
    public float toxicLightMultiplier = 0.25f;
    public float toxicLightDuration = 8f;
    public float toxicLightTickRate = 1f;
    public float taintedJoltRadius = 10f;

    public float plagueBaseDamage = 0f;
    public float plagueDuration = 20f;
    public float plagueTickRate = 1f;

    [Header("Electrocute Values")]
    public int maxChains = 5;
    public float electrocuteMultiplier = 0.75f;
    public float chainRange = 8f;
    public float damageFalloff = 0.75f;
    #endregion

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }
    public bool CheckForElementalReaction(StatusEffect newEffect)
    {
        bool reactionTriggered = false;

        switch (newEffect)
        {
            // fire reactions 
            case BurnEffect when enemy.HasEffect<BurnEffect>():
                TriggerScorch();
                StatusEffectDatabase.instance.DiscoverEffect(Status.Scorch);
                elementMixName = "Scorch";
                reactionTriggered = true;
                break;
            case BurnEffect when enemy.HasEffect<PoisonEffect>():
                TriggerFireCombust(newEffect);
                WorldVFXManager.instance.PlayEffectAtLocation(enemy.transform.position, Status.FireCombust);
                StatusEffectDatabase.instance.DiscoverEffect(Status.FireCombust);
                elementMixName = "Fire Combust";
                reactionTriggered = true;
                break;
            case BurnEffect when enemy.HasEffect<WetEffect>():
                TriggerSteamed();
                StatusEffectDatabase.instance.DiscoverEffect(Status.Steamed);
                elementMixName = "Steamed";
                reactionTriggered = true;
                break;
            case BurnEffect when enemy.HasEffect<ChillEffect>():
                TriggerWet();
                StatusEffectDatabase.instance.DiscoverEffect(Status.Wet);
                elementMixName = "Wet";
                reactionTriggered = true;
                break;
            case BurnEffect when enemy.HasEffect<PlagueEffect>():
                StatusEffectDatabase.instance.DiscoverEffect(Status.ToxicBlaze);
                TriggerToxicBlaze();
                elementMixName = "Toxic Blaze";
                reactionTriggered = true;
                break;
            case BurnEffect when enemy.HasEffect<ChargedEffect>():
                WorldVFXManager.instance.PlayEffectAtLocation(enemy.transform.position, Status.Superheat);
                StatusEffectDatabase.instance.DiscoverEffect(Status.Superheat);
                TriggerSuperheat();
                elementMixName = "Superheat";
                reactionTriggered = true;
                break;
            case BurnEffect when enemy.HasEffect<SteamedEffect>():
                WorldVFXManager.instance.PlayEffectAtLocation(enemy.transform.position, Status.Fizzled);
                StatusEffectDatabase.instance.DiscoverEffect(Status.Fizzled);
                TriggerFizzled();
                Debug.Log("Fizzled");
                //does not apply burn
                reactionTriggered = true;
                break;
            case BurnEffect when enemy.HasEffect<BrittleEffect>():
                TriggerShatter();
                WorldVFXManager.instance.PlayEffectAtLocation(enemy.transform.position, Status.Shatter);
                StatusEffectDatabase.instance.DiscoverEffect(Status.Shatter);
                elementMixName = "Shatter";
                reactionTriggered = true;
                break;

            // ice reactions
            case ChillEffect when enemy.HasEffect<WetEffect>():
                TriggerFreeze();
                StatusEffectDatabase.instance.DiscoverEffect(Status.Freeze);
                elementMixName = "Freeze";
                reactionTriggered = true;
                break;
            case ChillEffect when enemy.HasEffect<ChillEffect>():
                TriggerBrittle();
                StatusEffectDatabase.instance.DiscoverEffect(Status.Brittle);
                elementMixName = "Brittle";
                reactionTriggered = true;
                break;
            case ChillEffect when enemy.HasEffect<PoisonEffect>():
                TriggerCryotoxin();
                StatusEffectDatabase.instance.DiscoverEffect(Status.Cryotoxin);
                elementMixName = "Cryotoxin";
                reactionTriggered = false; //cryotoxin still leaves both the poison and chill on as well
                break;
            case ChillEffect when enemy.HasEffect<ChargedEffect>():
                TriggerSupercharged();
                StatusEffectDatabase.instance.DiscoverEffect(Status.Supercharged);
                elementMixName = "Supercharged";
                reactionTriggered = true;
                break;
            case ChillEffect when enemy.HasEffect<ToxicBlazeEffect>():
                TriggerExtinguish();
                WorldVFXManager.instance.PlayEffectAtLocation(enemy.transform.position, Status.Extinguish);
                StatusEffectDatabase.instance.DiscoverEffect(Status.Extinguish);
                elementMixName = "Extinguish";
                reactionTriggered = true;
                break;

            // lightning reactions
            case ShockedEffect when enemy.HasEffect<ShockedEffect>():
                TriggerCharged();
                StatusEffectDatabase.instance.DiscoverEffect(Status.Charged);
                elementMixName = "Charged";
                reactionTriggered = true;
                break;
            case ShockedEffect when enemy.HasEffect<ChargedEffect>():
                TriggerStun();
                StatusEffectDatabase.instance.DiscoverEffect(Status.Stun);
                elementMixName = "Stun";
                reactionTriggered = true;
                break;
            case ShockedEffect when enemy.HasEffect<WetEffect>():
                TriggerElectrocute();
                StatusEffectDatabase.instance.DiscoverEffect(Status.Electrocute);
                elementMixName = "Electrocute";
                reactionTriggered = true;
                break;
            case ShockedEffect when enemy.HasEffect<PlagueEffect>():
                TriggerInfectedSurge();
                WorldVFXManager.instance.PlayEffectAtLocation(enemy.transform.position, Status.InfectedSurge);
                StatusEffectDatabase.instance.DiscoverEffect(Status.InfectedSurge);
                elementMixName = "Viral Surge";
                reactionTriggered = false; //still applies shock
                break;
            case ShockedEffect when enemy.HasEffect<ScorchEffect>():
                TriggerOverload();
                WorldVFXManager.instance.PlayEffectAtLocation(enemy.transform.position, Status.Overload);
                StatusEffectDatabase.instance.DiscoverEffect(Status.Overload);
                elementMixName = "Overload";
                reactionTriggered = true;
                break;
            case ShockedEffect when enemy.HasEffect<PoisonEffect>():
                TriggerToxicLight();
                WorldVFXManager.instance.PlayEffectAtLocation(enemy.transform.position, Status.ToxicLight);
                StatusEffectDatabase.instance.DiscoverEffect(Status.ToxicLight);
                elementMixName = "Toxic Light";
                reactionTriggered = false; //still applies shock as well
                break;
            case ShockedEffect when enemy.HasEffect<FrozenEffect>():
                TriggerAtomize();
                WorldVFXManager.instance.PlayEffectAtLocation(enemy.transform.position, Status.Atomize);
                StatusEffectDatabase.instance.DiscoverEffect(Status.Atomize);
                elementMixName = "Atomize";
                reactionTriggered = true;
                break;

            // poison reactions
            case PoisonEffect when enemy.HasEffect<FrozenEffect>():
                TriggerDissolve();
                WorldVFXManager.instance.PlayEffectAtLocation(enemy.transform.position, Status.Dissolve);
                StatusEffectDatabase.instance.DiscoverEffect(Status.Dissolve);
                elementMixName = "Dissolve";
                reactionTriggered = true;
                break;
            case PoisonEffect when enemy.HasEffect<BurnEffect>():
                TriggerPoisonCombust(newEffect);
                WorldVFXManager.instance.PlayEffectAtLocation(enemy.transform.position, Status.PoisonCombust);
                StatusEffectDatabase.instance.DiscoverEffect(Status.PoisonCombust);
                elementMixName = "Poison Combust";
                reactionTriggered = true;
                break;
            case PoisonEffect:
                poisonStacks++;
                if (poisonStacks > maxPoisonStacks)
                {
                    poisonStacks = 0;
                    TriggerPlague();
                    StatusEffectDatabase.instance.DiscoverEffect(Status.Plague);
                    elementMixName = "Plague";
                    reactionTriggered = true;
                }
                break;

            default:
                break;
        }
        DamageUIManager.instance.UpdateEffectName(elementMixName);
        return reactionTriggered;
    }

    //fire triggered reactions
    private void TriggerScorch()
    {
        Debug.Log("Trigger Scorch");

        StatusEffect burnEffect = enemy.GetEffect<BurnEffect>();
        if (burnEffect != null)
        {
            burnEffect.RemoveEffect(); 
            enemy.RemoveEffect(burnEffect);
        }

        ScorchEffect scorch = new ScorchEffect();
        scorch.ApplyScorch(enemy, scorchDuration); //increased fire damage taken 
    }
    private void TriggerFireCombust(StatusEffect burn)
    {
        Debug.Log("Trigger Flame Combust");
        Ability sourceAbility = enemy.lastDamageSource?.sourceAbility ?? null;

        if (enemy.lastDamageSource.HasValue)
        {
            DamageInstance lastHit = enemy.lastDamageSource.Value;
            sourceAbility = lastHit.sourceAbility;
        }

        if (burn is BurnEffect burnDOT)  // this is a check and a CAST(ask tim )
        {
            float combustDamage = burnDOT.dotEffect.totalDamage / 2;
            enemy.TakeDamage(combustDamage, ElementType.Fire, sourceAbility);
        }
    }
    private void TriggerSteamed()
    {
        Debug.Log("Trigger Steamed");

        StatusEffect wetEffect = enemy.GetEffect<WetEffect>();
        if (wetEffect != null)
        {
            wetEffect.RemoveEffect();
            enemy.RemoveEffect(wetEffect);
        }
        
        SteamedEffect steam = new SteamedEffect();
        steam.ApplySteamed(enemy, steamedDuration); // 80% Fire resistance effect
    }
    private void TriggerToxicBlaze()
    {
        Debug.Log("Trigger Toxic Blaze");
        //Get the plague info that is being removed
        PlagueEffect plague = enemy.GetEffect<PlagueEffect>();
        if (plague != null)
        {
            plague.RemoveEffect();
            enemy.RemoveEffect(plague);
        }

        // Makes plague tick twice as fast but half duration
        ToxicBlazeEffect blaze = new ToxicBlazeEffect();
        blaze.ApplyToxicBlaze(enemy, plague.dotEffect.totalDamage, plagueDuration/2, plagueTickRate*2);
    }
    private void TriggerWet()
    {
        Debug.Log("Trigger Wet");
        
        StatusEffect chillEffect = enemy.GetEffect<ChillEffect>();
        if (chillEffect != null)
        {
            chillEffect.RemoveEffect();
            enemy.RemoveEffect(chillEffect);
        }
        WetEffect wet = new WetEffect();
        wet.ApplyWet(enemy, wetDuration); //wet does nothing on its own 
    }
    private void TriggerFizzled()
    {
        //Does hitting a steamed enemy remove steamed? for now yes
        StatusEffect steamed = enemy.GetEffect<SteamedEffect>();
        if (steamed != null)
        {
            steamed.RemoveEffect();
            enemy.RemoveEffect(steamed);
        }
    }
    private void TriggerSuperheat()
    {
        Debug.Log("Trigger Superheat");

        StatusEffect chargedEffect = enemy.GetEffect<ChargedEffect>();
        if (chargedEffect != null)
        {
            chargedEffect.RemoveEffect();
            enemy.RemoveEffect(chargedEffect);
        }

        if (floorFirePrefab != null)
        {
            GameObject floorFire = Instantiate(floorFirePrefab, enemy.transform);
            //SCRIPT ON THE FLOORfIREprEFAB HANDLES ITS DAMAGE APPLICATION
        }
    }
    private void TriggerShatter()
    {
        Debug.Log("Trigger shatter");

        StatusEffect brittle = enemy.GetEffect<BrittleEffect>();
        if (brittle != null)
        {
            brittle.RemoveEffect();
            enemy.RemoveEffect(brittle);
        }
    }

    //ice triggered reactions
    private void TriggerFreeze()
    {
        Debug.Log("Trigger Freeze");
        
        StatusEffect wet = enemy.GetEffect<WetEffect>();
        if (wet != null)
        {
            wet.RemoveEffect();
            enemy.RemoveEffect(wet);
        }

        FrozenEffect freeze = new FrozenEffect();
        freeze.ApplyFrozen(enemy, freezeDuration);
    }
    private void TriggerBrittle()
    {
        Debug.Log("Trigger Brittle");

        StatusEffect chill = enemy.GetEffect<ChillEffect>();
        if (chill != null)
        {
            chill.RemoveEffect();
            enemy.RemoveEffect(chill);
        }
        
        BrittleEffect brittle = new BrittleEffect();
        brittle.ApplyBrittle(enemy, brittleDuration);
    }
    private void TriggerCryotoxin()
    {
        Debug.Log("Trigger cryotoxin");
        //does not remove poison or chill, just adds the cryotoxin on top
        CryoToxinEffect cryo = new CryoToxinEffect();
        cryo.ApplyCryotoxin(enemy, cryotoxinDuration);
    }
    private void TriggerSupercharged()
    {
        Debug.Log("Trigger supercharged");
        
        StatusEffect charged = enemy.GetEffect<ChargedEffect>();
        if (charged != null)
        {
            charged.RemoveEffect();
            enemy.RemoveEffect(charged);
        }

        float iceProcDamage = 100;
        Ability sourceAbility = null;

        if (enemy.lastDamageSource.HasValue)
        {
            DamageInstance lastHit = enemy.lastDamageSource.Value;
            iceProcDamage = lastHit.damage * superChargedMultiplier;
            sourceAbility = lastHit.sourceAbility;
        }
            
        enemy.TakeDamage(iceProcDamage, ElementType.Ice, sourceAbility);
        SuperchargedEffect supra = new SuperchargedEffect();
        supra.ApplySupercharged(enemy, superchargedDuration);
    }
    private void TriggerExtinguish()
    {
        Debug.Log("Trigger extinguish");
        
        StatusEffect blaze = enemy.GetEffect<ToxicBlazeEffect>();
        if (blaze != null)
        {
            blaze.RemoveEffect();
            enemy.RemoveEffect(blaze);
        }
    }

    //trigger lightning reactions
    private void TriggerCharged()
    {
        Debug.Log("Trigger charged");
        
        ShockedEffect shock = enemy.GetEffect<ShockedEffect>();
        if (shock != null)
        {
            enemy.RemoveDamageMultiplier(shock, shock.damageMultiplier);
            shock.RemoveEffect();
            enemy.RemoveEffect(shock);
        }
        
        ChargedEffect charged = new ChargedEffect();
        charged.ApplyCharged(enemy, chargedDuration); 
    }
    private void TriggerElectrocute()
    {
        Debug.Log("Trigger electrocute");
        
        StatusEffect wet = enemy.GetEffect<WetEffect>();
        if (wet != null)
        {
            wet.RemoveEffect();
            enemy.RemoveEffect(wet);
        }

        float chainDamage = 100f;  
        Ability sourceAbility = enemy.lastDamageSource?.sourceAbility ?? null;

        if (enemy.lastDamageSource.HasValue)
        {
            DamageInstance lastHit = enemy.lastDamageSource.Value;
            chainDamage = lastHit.damage * electrocuteMultiplier;  
            sourceAbility = lastHit.sourceAbility;
        }
        ElectrocuteEffect electrocute = new ElectrocuteEffect();
        electrocute.ApplyElectrocute(enemy, chainDamage, maxChains, chainRange, damageFalloff, sourceAbility);
    }
    private void TriggerInfectedSurge()
    {
        Debug.Log("Trigger infected surge");
        float lightningDamage = 100;
        Ability sourceAbility = null;

        if (enemy.lastDamageSource.HasValue)
        {
            DamageInstance lastHit = enemy.lastDamageSource.Value;
            lightningDamage = lastHit.damage * infectedSurgeMult;
            sourceAbility = lastHit.sourceAbility;
        }
        foreach (Enemy target in FindObjectsByType<Enemy>(FindObjectsSortMode.None))
        {
            if (target.HasEffect<PlagueEffect>())
            {
                target.TakeDamage(lightningDamage, ElementType.Lightning, sourceAbility);
            }
        }
    }
    private void TriggerStun()
    {
        Debug.Log("Trigger Stun");
        
        StatusEffect charged = enemy.GetEffect<ChargedEffect>();
        if (charged != null)
        {
            charged.RemoveEffect();
            enemy.RemoveEffect(charged);
        }

        StunEffect stun = new StunEffect();
        stun.ApplyStun(enemy, stunDuration);
    }
    private void TriggerOverload()
    {
        Debug.Log("Trigger Overload");
        
        StatusEffect scorch = enemy.GetEffect<ScorchEffect>();
        if (scorch != null)
        {
            scorch.RemoveEffect();
            enemy.RemoveEffect(scorch);
        }

        Ability sourceAbility = enemy.lastDamageSource?.sourceAbility ?? null;

        //explode with a small aoe that consumes the scorch, this feels lack luster still 
        //this one does NOT scale with damage done, so it does a flat damage, making it beneficial
        //to trigger this as rapidly as possible.

        if (enemy.lastDamageSource.HasValue)
        {
            DamageInstance lastHit = enemy.lastDamageSource.Value;
            sourceAbility = lastHit.sourceAbility;
        }
        Collider[] nearbyEnemies = Physics.OverlapSphere(enemy.transform.position, overloadRadius);
        foreach (Collider col in nearbyEnemies)
        {
            Enemy target = col.GetComponent<Enemy>();
            if (target != null) //does damage to the current enemy as well
            {
                target.TakeDamage(overloadDamage, ElementType.Lightning, sourceAbility);

            }
        }
    }
    private void TriggerToxicLight()
    {
        Debug.Log("Trigger toxic light");
        //does not remove the poison
        //getting th ability that caused this and having it also spread a poison effect on the enemy
        //applies a poison stack to nearby enemies when a poisoned enemy is hit with lightning

        float spreadPoisonDamage = 30f;
        Ability sourceAbility = enemy.lastDamageSource?.sourceAbility ?? null;

        if (enemy.lastDamageSource.HasValue)
        {
            DamageInstance lastHit = enemy.lastDamageSource.Value;
            spreadPoisonDamage = lastHit.damage * toxicLightMultiplier;
            sourceAbility = lastHit.sourceAbility;
        }
        Collider[] nearbyEnemies = Physics.OverlapSphere(enemy.transform.position, taintedJoltRadius);
        foreach (Collider col in nearbyEnemies)
        {
            Enemy target = col.GetComponent<Enemy>();
            if (target != null && target != enemy)
            {
                PoisonEffect poison = new PoisonEffect();
                poison.ApplyPoison(enemy, spreadPoisonDamage, toxicLightDuration, toxicLightTickRate);
            }
        }
    }
    private void TriggerAtomize()
    {
        Debug.Log("Trigger atomize");
        
        StatusEffect freeze = enemy.GetEffect<FrozenEffect>();
        if (freeze != null)
        {
            freeze.RemoveEffect();
            enemy.RemoveEffect(freeze);
        }

        float explosionDamage = 200f;
        Ability sourceAbility = enemy.lastDamageSource?.sourceAbility ?? null;

        if (enemy.lastDamageSource.HasValue)
        {
            DamageInstance lastHit = enemy.lastDamageSource.Value;
            explosionDamage = lastHit.damage * atomizedMultiplier;
            sourceAbility = lastHit.sourceAbility;
        }

        Collider[] nearbyEnemies = Physics.OverlapSphere(enemy.transform.position, atomizeRadius);
        foreach (Collider col in nearbyEnemies)
        {
            Enemy target = col.GetComponent<Enemy>();
            if (target != null)
            {
                target.TakeDamage(explosionDamage, ElementType.Lightning, sourceAbility);
                target.TakeDamage(explosionDamage, ElementType.Ice, sourceAbility);
            }
        }
    }

    //poison reactions
    private void TriggerPlague()
    {
        Debug.Log("Trigger plague");
        float totalPlagueDamage = plagueBaseDamage;

        List<PoisonEffect> poisonStacks = enemy.activeEffects.OfType<PoisonEffect>().ToList();
        foreach (var poison in poisonStacks)
        {
            totalPlagueDamage += poison.dotEffect.totalDamage; 
            poison.RemoveEffect();
            enemy.RemoveEffect(poison);
        }

        PlagueEffect plague = new PlagueEffect();
        plague.ApplyPlague(enemy, totalPlagueDamage, plagueDuration, plagueTickRate);
    }
    private void TriggerDissolve()
    {
        Debug.Log("Trigger dissolve");
        //this just does not apply poison and also removes freeze
        
        StatusEffect freeze = enemy.GetEffect<FrozenEffect>();
        if (freeze != null)
        {
            freeze.RemoveEffect();
            enemy.RemoveEffect(freeze);
        }
    }
    private void TriggerPoisonCombust(StatusEffect poison)
    {
        Debug.Log("Trigger Poison Combust");
        Ability sourceAbility = enemy.lastDamageSource?.sourceAbility ?? null;
        
        if (enemy.lastDamageSource.HasValue)
        {
            DamageInstance lastHit = enemy.lastDamageSource.Value;
            sourceAbility = lastHit.sourceAbility;
        }

        if (poison is PoisonEffect poisonDOT)  // this is a check and a CAST(ask tim )
        {
            float combustDamage = poisonDOT.dotEffect.totalDamage / 2;
            enemy.TakeDamage(combustDamage, ElementType.Poison, sourceAbility);
        }
    }
}