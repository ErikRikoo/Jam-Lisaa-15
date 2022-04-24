using UnityEngine;

namespace Script.PlayerHandling.Spells.SpellType
{
    [CreateAssetMenu(fileName = "Dispell Effect", menuName = "Player/Spells/Dispell", order = 0)]
    public class DispellCastEffect : CasterEffectSpellType
    {
        [SerializeField] private float m_EffectDuration;
        
        
        public override void CastSpell(SpellHandler spellHandler)
        {
            spellHandler.GetComponent<PlayerStateManager>().DispellDuring(m_EffectDuration);
        }
    }
}