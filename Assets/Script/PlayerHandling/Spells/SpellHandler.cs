using System;
using System.Collections;
using System.Linq;
using Script.PlayerHandling.Spells.SpellType;
using Script.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Script.PlayerHandling.Spells
{
    public class SpellHandler : MonoBehaviour
    {
        [SerializeField] private float m_MinImageScale = 0.1f;
        
        [Min(0)]
        [SerializeField] private float m_Cooldown;
        
        [SerializeField] private SpellCollection m_SpellCollection;

        [SerializeField] private AudioSource m_AudioSource;
        
        
        private int[] m_Order;
        private int m_CurrentPosition;

        private bool m_CanCast = true;
        
        private ImageHolder m_ImageHolder;

        private float m_CurrentCooldownTime;
        private bool m_Rerolling;


        public void Bind(ImageHolder _holder)
        {
            m_ImageHolder = _holder;
            m_ImageHolder.gameObject.SetActive(true);
            Reroll();
        }

        private void Reroll()
        {
            m_Order = new int[m_SpellCollection.Count];
            for (int i = 0; i < m_Order.Length; i++)
            {
                m_Order[i] = i;
            }

            m_Order = m_Order.OrderBy(x => Random.value).ToArray();
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            if (m_ImageHolder.Images.Length != m_SpellCollection.Count)
            {
                return;
            }

            int index = 0;
            int end = m_Order.Length - m_CurrentPosition;
            
            for (; index < end; ++index)
            {
                m_ImageHolder.Images[index].sprite = m_SpellCollection[m_Order[index + m_CurrentPosition]].Image;
                m_ImageHolder.Images[index].gameObject.SetActive(true);
            }

            for (; index < m_Order.Length; ++index)
            {
                m_ImageHolder.Images[index].gameObject.SetActive(false);
            }
        }

        public void RemoveSpell(InputAction.CallbackContext _context)
        {
            if (_context.phase != InputActionPhase.Started)
            {
                return;
            }
            
            if (m_Rerolling)
            {
                return;
            }

            m_CurrentCooldownTime = 0;
            CheckIfShouldReroll();
        }

        private void CheckIfShouldReroll()
        {
            ++m_CurrentPosition;
            UpdateDisplay();
            StopAllCoroutines();
            if (m_CurrentPosition >= m_SpellCollection.Count)
            {
                m_CurrentPosition = 0;
                m_Rerolling = true;
                Reroll();
                StartCoroutine(c_Cooldown(() => m_Rerolling = false));
                return;
            }
            
            StartCoroutine(c_Cooldown());
        }

        public void CastSpell()
        {
            if (!m_CanCast)
            {
                return;
            }
            m_SpellCollection[m_Order[m_CurrentPosition]].CastSpell(this);
            
            CheckIfShouldReroll();
            m_AudioSource?.Play();
        }

        private IEnumerator c_Cooldown(Action _endOfCooldownEffect = null)
        {
            m_CanCast = false;
            float inverseDuration = 1 / m_Cooldown;
            for (m_CurrentCooldownTime = 0; m_CurrentCooldownTime < m_Cooldown; m_CurrentCooldownTime += Time.deltaTime)
            {
                UpdateImages(m_CurrentCooldownTime * inverseDuration);
                yield return null;
            }
            
            m_CanCast = true;
            _endOfCooldownEffect?.Invoke();
            m_Rerolling = false;
        }

        private void UpdateImages(float _ratio)
        {
            int index = 0;
            int end = m_Order.Length - m_CurrentPosition;
            
            for (; index < end; ++index)
            {
                m_ImageHolder.Images[index].transform.localScale = Vector3.one *
                                                                   Mathf.LerpUnclamped(m_MinImageScale, 1, _ratio);
            }
        }
    }
}