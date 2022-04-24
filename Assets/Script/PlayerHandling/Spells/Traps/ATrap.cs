using System;
using System.Collections;
using EditorUtilities.Editor.Attributes.AbstractReference;
using PlanetHandling;
using Script.PlayerHandling.Spells.SpellType;
using Script.PlayerHandling.Spells.SpellType.TrapEffect;
using UnityEngine;

namespace Script.PlayerHandling.Spells.Traps
{
    public class ATrap : MonoBehaviour
    {
        [AbstractReference] [SerializeReference]
        private ATrapEffect m_Effect;
        
        [SerializeField] private bool m_FriendlyFire = true;
        
        [SerializeField] private float m_DelayBeforeEffect = 1;
        
        private SpellHandler m_Caster;
        private Collider m_Collider;
        private TrapSpellType m_Spell;

        private void Awake()
        {
            m_Collider = GetComponent<Collider>();
            m_Collider.enabled = false;
            StartCoroutine(c_EnableCollider());
        }

        private IEnumerator c_EnableCollider()
        {
            yield return new WaitForSeconds(m_DelayBeforeEffect);
            m_Collider.enabled = true;
        }

        public void BindTo(SpellHandler spellHandler, TrapSpellType trapSpellType)
        {
            m_Caster = spellHandler;
            m_Spell = trapSpellType;
            var playerObjectPlacer = spellHandler.GetComponent<PlanetObjectPlacer>();
            var objectPlacer = GetComponent<PlanetObjectPlacer>();
            transform.parent = playerObjectPlacer.Collider.GetComponent<PlanetData>().ObjectChild;
            objectPlacer.Collider = playerObjectPlacer.Collider;
            objectPlacer.Coordinate = playerObjectPlacer.Coordinate;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }

            if (!m_FriendlyFire && other.gameObject == m_Caster.gameObject)
            {
                return;
            }
            
            m_Effect.Effect(other.gameObject, m_Spell);
            Destroy(gameObject);
        }
    }
}