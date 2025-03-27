using UnityEngine;

public class BasicProjectileAbility : ProjectileAbility
{
    
    public override void Execute()
    {
        base.Execute();
        PlayerManager.instance.playerCombatManager.TrackBasicAttacks();
    }
}
