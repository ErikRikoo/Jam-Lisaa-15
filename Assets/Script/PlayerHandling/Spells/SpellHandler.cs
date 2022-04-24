using System;
using System.Collections;
using System.Linq;
using Script.PlayerHandling.Spells.SpellType;
using Script.Utilities;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Script.PlayerHandling.Spells
{
    public class SpellHandler : MonoBehaviour
    {
        [Min(0)]
        [SerializeField] private float m_Cooldown;
        
        [SerializeField] private SpellCollection m_SpellCollection;
        
        private int[] m_Order;
        private int m_CurrentPosition;

        private bool m_CanCast = true;
        
        private ImageHolder m_ImageHolder;


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
            
            Debug.Log(end);
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

        public void CastSpell(bool _consumeSpell = true)
        {
            if (!m_CanCast)
            {
                return;
            }

            if (_consumeSpell)
            {
                m_SpellCollection[m_Order[m_CurrentPosition]].CastSpell(this);
            }
            
            m_CanCast = false;
            ++m_CurrentPosition;
            UpdateDisplay();
            if (m_CurrentPosition >= m_SpellCollection.Count)
            {
                StartCoroutine(c_Cooldown(() =>
                {
                    m_CurrentPosition = 0;
                    Reroll();
                }));

                return;
            }
            
            StartCoroutine(c_Cooldown());
        }

        private IEnumerator c_Cooldown(Action _endOfCooldownEffect = null)
        {
            yield return new WaitForSeconds(m_Cooldown);
            m_CanCast = true;
            _endOfCooldownEffect?.Invoke();
        }
    }
}