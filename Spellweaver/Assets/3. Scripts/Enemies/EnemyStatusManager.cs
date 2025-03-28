using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyStatusManager : MonoBehaviour
{
    public SkinnedMeshRenderer enemyMesh;

    Enemy enemy;
    
    [Header("Materials")]
    public Material baseMaterial;
    public Material scorchMaterial;
    public Material brittleMaterial;
    public Material freezeMaterial;
    public Material chargedMaterial;
    public Material stunMaterial;
    public Material plagueMaterial;

    [Header("VFX Objects")]
    public GameObject burnVFX; 
    public GameObject scorchVFX;
    public GameObject wetVFX;
    public GameObject toxicBlazeVFX;
    public GameObject steamedVFX;

    public GameObject chillVFX;
    public GameObject superchargedVFX;
    public GameObject cryotoxinVFX;

    public GameObject shockVFX;

    public GameObject poisonVFX;

    private Dictionary<Status, GameObject> activeVFX = new Dictionary<Status, GameObject>();
    //private Dictionary<Status, int> poisonStacks = new Dictionary<Status, int>();
    public int poisonStacks = 0;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }
    private void Start()
    {
        InitializeEffects();
    }
    private void InitializeEffects()
    {
        activeVFX[Status.Burn] = burnVFX;
        activeVFX[Status.Scorch] = scorchVFX;
        activeVFX[Status.Wet] = wetVFX;
        activeVFX[Status.ToxicBlaze] = toxicBlazeVFX;
        activeVFX[Status.Steamed] = steamedVFX;

        activeVFX[Status.Chill] = chillVFX;
        activeVFX[Status.Supercharged] = superchargedVFX;
        activeVFX[Status.Cryotoxin] = cryotoxinVFX;

        activeVFX[Status.Shock] = shockVFX;
        
        activeVFX[Status.Poison] = poisonVFX;
        
        foreach (var vfx in activeVFX.Values)
        {
            if (vfx != null) vfx.SetActive(false);
        }
    }
    public void ApplyEffect(Status status)
    {
        if (status == Status.Poison)
        {
            //poisonStacks[status]++;
            poisonStacks++;
            UpdatePoisonEffect();
            return;
        }
        if (activeVFX.ContainsKey(status) && activeVFX[status] != null)
        {
            activeVFX[status].SetActive(true);
        }
        HandleMaterialSwap(status, true);
    }
    public void RemoveEffect(Status status)
    {
        if(status == Status.Poison)
        {
            poisonStacks --;
            UpdatePoisonEffect();
            return;
        }
        if (activeVFX.ContainsKey(status) && activeVFX[status] != null)
        {
            activeVFX[status].SetActive(false);
        }
        HandleMaterialSwap(status, false);
    }
    public void HandleMaterialSwap(Status status, bool applyEffect)
    {
        if (status == Status.Freeze)
            enemyMesh.material = applyEffect ? freezeMaterial : baseMaterial;
        else if (status == Status.Scorch)
            enemyMesh.material = applyEffect ? scorchMaterial : baseMaterial;
        else if (status == Status.Charged)
            enemyMesh.material = applyEffect ? chargedMaterial : baseMaterial;
        else if (status == Status.Stun)
            enemyMesh.material = applyEffect ? stunMaterial : baseMaterial;
        else if(status == Status.Brittle)
            enemyMesh.material = applyEffect ? brittleMaterial : baseMaterial;
        else if (status == Status.Plague)
            enemyMesh.material = applyEffect ? plagueMaterial : baseMaterial;
    }
    private void UpdatePoisonEffect()
    {
        if (poisonStacks == 0)
        {
            poisonVFX.GetComponent<VisualEffect>().SetFloat("PoisonRate", 1f);
            if (poisonVFX != null) poisonVFX.SetActive(false);
        }
        else
        {
            if (poisonVFX != null)
            {
                poisonVFX.SetActive(true);
                float poisonRate = poisonStacks * 15;
                poisonVFX.GetComponent<VisualEffect>().SetFloat("PoisonRate", poisonRate);
            }
        }
    }
}
