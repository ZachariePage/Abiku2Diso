using UnityEngine;

public enum CastingSpellColdownType
{
    player,
    enemy,
    both
}
public interface ISpellCaster
{
    public void PutAbilityOnColdown();
    
    public void RefreshColdown();
    public bool IsOnColdown();

    public bool CanThrowSpell();

    public bool IsCastingSpell();
    
    public void SetCastingSpell(bool value, CastingSpellColdownType type);

    public void OnAbilityThrown();
    public void OnAbilityFinished(AbilityAftermathInfo abilityAftermathInfo);

    public GridActor GetActor();
    UsedActionTracker GetCooldownTracker();
}
