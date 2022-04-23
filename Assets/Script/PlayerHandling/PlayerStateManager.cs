using System;
using PlanetHandling;
using UnityAtoms.BaseAtoms;
using UnityEditor;
using UnityEngine;

namespace Script.PlayerHandling
{
    [RequireComponent(typeof(Rigidbody), typeof(ConstantForce), typeof(PlanetObjectPlacer))]
    public class PlayerStateManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float m_GravityScale;
        [SerializeField] private FloatVariable m_PlanetSpeed;

        private ConstantForce m_ConstantForce;
        private PlanetObjectPlacer m_ObjectPlacer;
        private Rigidbody m_Rigidbody;

        private float m_OriginalLatitude;
        
        [SerializeField,Range(-1, 1)] 
        private float m_CurrentPosition;

        public float CurrentPosition
        {
            set
            {
                m_CurrentPosition = Mathf.Clamp(value, -1, 1);
                UpdatePosition();
            }
        }


        private void Awake()
        {
            GetComponent<Rigidbody>().useGravity = false;
            GetRequiredComponents();
            m_OriginalLatitude = m_ObjectPlacer.Latitude;
        }

        private void Start()
        {
            UpdateGravity();
        }

        private void UpdateGravity()
        {
            m_ConstantForce.force = -transform.up * m_GravityScale;
        }

        private void GetRequiredComponents()
        {
            m_ConstantForce = GetComponent<ConstantForce>();
            m_ObjectPlacer = GetComponent<PlanetObjectPlacer>();
            m_Rigidbody = GetComponent<Rigidbody>();
        }

        private void OnValidate()
        {
            if (EditorApplication.isPlaying)
            {
                if (m_ObjectPlacer == null)
                {
                    return;
                }
                UpdatePosition();
            }
        }

        private void UpdatePosition()
        {
            m_ObjectPlacer.Latitude = Mathf.LerpUnclamped(m_OriginalLatitude, m_OriginalLatitude + 90, m_CurrentPosition);
            UpdateGravity();
        }

        public void Jump()
        {
            m_Rigidbody.AddForce(transform.up * 200);
        }
    }
}