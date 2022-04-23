using System;
using System.Collections;
using PlanetHandling;
using UnityAtoms.BaseAtoms;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Script.PlayerHandling
{
    enum State
    {
        Normal, Stunned, Slow, Boosted
    }
    
    
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
        private State m_State;
        
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

        #if UNITY_EDITOR
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
        #endif

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
            if (m_State == State.Boosted || m_State == State.Stunned)
            {
                return;
            }
            
            StopAllCoroutines();
            m_State = State.Slow;
            ChangeSpeed(-_slowRatio, _slowDuration);
            m_OnSlow?.Invoke();
        }

        public void Boost(float _boostRatio, float _boostDuration)
        {
            StopAllCoroutines();
            m_State = State.Boosted;
            ChangeSpeed(_boostRatio, _boostDuration);
            m_OnBoost?.Invoke();
        }
        
        public void Stun(float _stunTime)
        {
            if (m_State == State.Boosted)
            {
                return;
            }
            StopAllCoroutines();
            m_State = State.Stunned;
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

            m_State = State.Normal;
        }

        private void UpdateCurrentPosition(float speed)
        {
            CurrentPosition += speed * 2 * Time.deltaTime;
        }
    }
}