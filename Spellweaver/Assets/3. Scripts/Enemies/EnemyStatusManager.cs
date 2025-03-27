using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusManager : MonoBehaviour
{
    [HideInInspector] public SkinnedMeshRenderer enemyMesh;

    [Header("Materials")]
    public Material baseMaterial;
    public Material scorchMaterial;
    public Material freezeMaterial;
    public Material superchargedMaterial;
    public Material chargedMaterial;
    public Material stunMaterial;

    [Header("VFX Objects")]
    public GameObject burnVFX, poisonVFX, shockVFX, freezeVFX, scorchVFX;
    public GameObject plagueVFX, stunVFX, superchargedVFX;

    private Dictionary<Status, GameObject> activeVFX = new Dictionary<Status, GameObject>();
    private Dictionary<Status, int> poisonStacks = new Dictionary<Status, int>();

    private void Awake()
    {

    }
    private void Start()
    {
        InitializeEffects();
    }

    private void InitializeEffects()
    {
        activeVFX[Status.Burn] = burnVFX;
        activeVFX[Status.Poison] = poisonVFX;
        activeVFX[Status.Shock] = shockVFX;
        activeVFX[Status.Freeze] = freezeVFX;
        activeVFX[Status.Scorch] = scorchVFX;
        activeVFX[Status.Plague] = plagueVFX;
        activeVFX[Status.Stun] = stunVFX;
        activeVFX[Status.Supercharged] = superchargedVFX;

        foreach (var vfx in activeVFX.Values)
        {
            if (vfx != null) vfx.SetActive(false);
        }
    }
    public void ApplyEffect(Status status)
    {
        if(activeVFX.ContainsKey(status) && activeVFX[status] != null)
        {
            activeVFX[status].SetActive(true);
        }
        HandleMaterialSwap(status, true);
    }
    public void RemoveEffect(Status status)
    {
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
        else if (status == Status.Supercharged)
            enemyMesh.material = applyEffect ? superchargedMaterial : baseMaterial;
    }
}
