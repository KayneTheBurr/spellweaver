using UnityEngine;
using UnityEngine.VFX;

public class WorldVFXManager : MonoBehaviour
{
    public static WorldVFXManager instance;

    [Header("Status Effect VFX Prefabs")]
    public GameObject fireCombustVFX; 
    public GameObject superheatVFX;
    public GameObject fizzledVFX;
    public GameObject shatterVFX;

    public GameObject extinguishVFX;

    public GameObject infectedSurgeVFX;
    public GameObject overloadVFX;
    public GameObject taintedJoltCFX;
    public GameObject atomizeVFX;

    public GameObject plagueVFX;
    public GameObject poisonCombustVFX;
    public GameObject dissolveVFX;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    public void PlayEffectAtLocation(Vector3 position, Status effectType)
    {
        GameObject vfxToPlay = null;

        switch (effectType)
        {
            case Status.FireCombust:
                vfxToPlay = fireCombustVFX;
                break;
            case Status.Superheat:
                vfxToPlay = superheatVFX;
                break;
            case Status.Fizzled:
                vfxToPlay = fizzledVFX;
                break;
            case Status.Shatter:
                vfxToPlay = shatterVFX;
                break;
            case Status.Extinguish:
                vfxToPlay = extinguishVFX;
                break;
            case Status.InfectedSurge:
                vfxToPlay= infectedSurgeVFX;
                break;
            case Status.Overload:
                vfxToPlay = overloadVFX;
                break;
            case Status.ToxicLight:
                vfxToPlay = taintedJoltCFX;
                break;
            case Status.Atomize:
                vfxToPlay = atomizeVFX;
                break;
            case Status.Plague:
                vfxToPlay = plagueVFX;
                break;
            case Status.PoisonCombust:
                vfxToPlay = poisonCombustVFX;
                break;
            case Status.Dissolve:
                vfxToPlay = dissolveVFX;
                break;
        }

        if (vfxToPlay != null)
        {
            GameObject instance = Instantiate(vfxToPlay, position, Quaternion.identity);
            if(instance != null)
            {
                Destroy(instance, 3f);
            }
            
        }
    }
}
