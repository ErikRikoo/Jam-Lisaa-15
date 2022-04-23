using System.Collections;
using System.Linq;
using Script.PlayerHandling.Spells.SpellType;
using UnityEngine;
using UnityEngine.UI;

namespace Script.PlayerHandling.Spells
{
    public class SpellHandler : MonoBehaviour
    {
        [Min(0)]
        [SerializeField] private float m_Cooldown;
        
        [SerializeField] private SpellCollection m_SpellCollection;
        [SerializeField] private Image m_Prefab;
        [SerializeField] private Transform m_Placeholder;
        
        
        private Image[] m_Displays;
        
        private int[] m_Order;
        private int m_CurrentPosition;

        private bool m_CanCast = true;

        private void Awake()
        {
            m_Displays = new Image[m_SpellCollection.Count];
            for (var i = 0; i < m_Displays.Length; i++)
            {
                m_Displays[i] = Instantiate(m_Prefab, m_Placeholder);
            }
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
            if (m_Displays.Length != m_SpellCollection.Count)
            {
                return;
            }

            int index = 0;
            int end = m_Order.Length - m_CurrentPosition;
            for (; index < end; ++index)
            {
                m_Displays[index].sprite = m_SpellCollection[m_Order[index + m_CurrentPosition]].Image;
            }

            for (; index < m_Order.Length; ++index)
            {
                m_Displays[index].gameObject.SetActive(false);
            }
        }

        public void CastSpell()
        {
            if (!m_CanCast)
            {
                return;
            }

            m_CanCast = false;
            ++m_CurrentPosition;
            StartCoroutine(c_Cooldown());
        }

        private IEnumerator c_Cooldown()
        {
            yield return new WaitForSeconds(m_Cooldown);
            m_CanCast = true;
        }
    }
}