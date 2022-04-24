using System;
using EditorUtilities.Editor.Attributes.AbstractReference;
using UnityEngine;

namespace Script.PlayerHandling.Spells.SpellType.TrapEffect
{
    [Serializable]
    [AbstractNaming(("Slow"))]
    public class SlowEffect : ATrapEffect
    {
        public override void Effect(GameObject _player, TrapSpellType _spell)
        {
            _player.GetComponent<PlayerStateManager>().Slow(_spell.EffectStrength, _spell.EffectDuration);
        }
    }
}