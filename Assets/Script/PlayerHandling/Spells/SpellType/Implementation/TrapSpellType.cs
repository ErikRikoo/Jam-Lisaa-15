using Script.PlayerHandling.Spells.Traps;
using UnityEngine;

namespace Script.PlayerHandling.Spells.SpellType
{
    [CreateAssetMenu(fileName = "Trap Spell", menuName = "Player/Spells/Trap", order = 0)]
    public class TrapSpellType : ASpellType
    {
        [SerializeField] private ATrap m_Trap;
        [SerializeField] private float m_EffectDuration;

        public float EffectDuration => m_EffectDuration;

        [SerializeField] private float m_EffectStrength;

        public float EffectStrength => m_EffectStrength;

        public override void CastSpell(SpellHandler spellHandler)
        {
            Instantiate(m_Trap).BindTo(spellHandler, this);
        }
    }
}