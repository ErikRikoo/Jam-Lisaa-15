using System;
using EditorUtilities.Editor.Attributes.AbstractReference;
using UnityEngine;

namespace Script.PlayerHandling.Spells.SpellType.TrapEffect
{
    [Serializable]
    [AbstractNaming("Stun")]
    public class StunEffect : ATrapEffect
    {
        public override void Effect(GameObject _player, TrapSpellType _spell)
        {
            _player.GetComponent<PlayerStateManager>().Stun(_spell.EffectDuration);
        }
    }
}