using System;
using UnityEngine;

namespace PlanetHandling
{
    public class PlanetTool : MonoBehaviour
    {
        [Header("Settings")]
        [Range(10, 170)]
        [SerializeField] private float m_Orientation;
        [SerializeField] private Vector3 m_Axis = new Vector3(0, -1, 0);
        
        [Header("Objects")]
        [SerializeField] private Transform[] m_ObjectToRotate;
        

        private void OnValidate()
        {
            UpdateObjects();
        }

        private void UpdateObjects()
        {
            foreach (var t in m_ObjectToRotate)
            {
                if (t == null)
                {
                    continue;
                }
                t.localRotation = Quaternion.Euler(m_Axis * m_Orientation);
            }
        }
    }
}