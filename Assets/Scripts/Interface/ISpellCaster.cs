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

    public void OnAbilityThrownEnd();

}
