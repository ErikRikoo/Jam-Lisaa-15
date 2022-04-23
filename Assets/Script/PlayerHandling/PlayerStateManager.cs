﻿using System;
using System.Collections;
using PlanetHandling;
using UnityAtoms.BaseAtoms;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Script.PlayerHandling
{
    [RequireComponent(typeof(Rigidbody), typeof(ConstantForce), typeof(PlanetObjectPlacer))]
    public class PlayerStateManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float m_GravityScale;
        [SerializeField] private FloatVariable m_PlanetSpeed;

        [Header("Events")]
        [SerializeField] private UnityEvent m_OnBoost;
        [SerializeField] private UnityEvent m_OnSlow;
        [SerializeField] private UnityEvent m_OnStun;
        

        private ConstantForce m_ConstantForce;
        private PlanetObjectPlacer m_ObjectPlacer;
        private Rigidbody m_Rigidbody;

        private float m_OriginalLatitude;
        
        [SerializeField,Range(-1, 1)] 
        private float m_CurrentPosition;

        public float CurrentPosition
        {
            get => m_CurrentPosition;
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

        private void ChangeSpeed(float _ratio, float _duration)
        {
            StartCoroutine(c_MoveInInverseSpeed(_ratio, _duration));
        }

        public void Slow(float _slowRatio, float _slowDuration)
        {
            ChangeSpeed(-_slowRatio, _slowDuration);
            m_OnSlow?.Invoke();
        }

        public void Boost(float _boostRatio, float _boostDuration)
        {
            ChangeSpeed(_boostRatio, _boostDuration);
            m_OnBoost?.Invoke();
        }
        
        public void Stun(float _stunTime)
        {
            ChangeSpeed(-1, _stunTime);
            m_OnStun?.Invoke();
        }

        private IEnumerator c_MoveInInverseSpeed(float _slowRatio, float _slowDuration)
        {
            for (float time = 0; time < _slowDuration; time += Time.deltaTime)
            {
                UpdateCurrentPosition(m_PlanetSpeed.Value * _slowRatio);
                yield return null;
            }
        }

        private void UpdateCurrentPosition(float speed)
        {
            CurrentPosition += speed * 2 * Time.deltaTime;
        }

        // public void Jump()
        // {
        //     m_Rigidbody.AddForce(transform.up * 200);
        // }
    }
}