using UnityEngine;

public interface ISpellCaster
{
    public void PutAbilityOnColdown();
    
    public void RefreshColdown();
    public bool IsOnColdown();

    public bool CanThrowSpell();

}
