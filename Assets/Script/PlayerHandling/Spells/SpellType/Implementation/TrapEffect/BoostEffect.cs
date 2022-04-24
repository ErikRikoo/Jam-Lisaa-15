using System;
using EditorUtilities.Editor.Attributes.AbstractReference;
using UnityEngine;

namespace Script.PlayerHandling.Spells.SpellType.TrapEffect
{
    [Serializable]
    [AbstractNaming(("Boost"))]
    public class BoostEffect : ATrapEffect
    {
        public override void Effect(GameObject _player, TrapSpellType _spell)
        {
            _player.GetComponent<PlayerStateManager>().Boost(_spell.EffectStrength, _spell.EffectDuration);
        }
    }
}