using System;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace Script.Utilities
{
    public class RotationOverTime : MonoBehaviour
    {
        [Header("Settings")]
        [Tooltip("A speed of 1 is equivalent to a complete turn per second")]
        [SerializeField] private FloatVariable m_Speed;
        [SerializeField] private Vector3 m_Axis;
        
        [Header("Objects")]
        [SerializeField] private Transform m_ObjectToRotate;

        private void Awake()
        {
            if (m_Speed == null)
            {
                Debug.LogError("Rotation Over Time should have a Speed");
                DestroyImmediate(this);
            }
            
            if (m_ObjectToRotate == null)
            {
                Debug.LogError("Rotation Over Time should have an Object to Rotate");
                DestroyImmediate(this);
            }
        }

        private void Update()
        {
            var newRotation = Quaternion.Euler(m_Axis * (m_Speed.Value * Time.deltaTime * 360));
            m_ObjectToRotate.localRotation *= newRotation;
        }
    }
}