using UnityEngine;

namespace Script.PlayerHandling.Spells.SpellType
{
    public abstract class ASpellType : ScriptableObject
    {
        [SerializeField] private Sprite m_Image;

        public Sprite Image => m_Image;

        public abstract void CastSpell(SpellHandler spellHandler);
    }
}