using System;
using UnityEngine;

namespace Script.PlayerHandling.Spells.SpellType.TrapEffect
{
    [Serializable]
    public abstract class ATrapEffect
    {
        public abstract void Effect(GameObject _player, TrapSpellType _spell);
    }
}