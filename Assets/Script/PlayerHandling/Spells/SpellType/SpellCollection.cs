using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Script.PlayerHandling.Spells.SpellType
{
    [CreateAssetMenu(fileName = "SpellCollection", menuName = "Player/Spells/Collection", order = 0)]
    public class SpellCollection : ScriptableObject, IEnumerable<ASpellType>
    {
        [Expandable]
        [SerializeField] private ASpellType[] m_Spells;

        public int Count => m_Spells.Length;

        public ASpellType this[int _index] => m_Spells[_index];
        
        public IEnumerator<ASpellType> GetEnumerator()
        {
            foreach (var spell in m_Spells)
            {
                yield return spell;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}