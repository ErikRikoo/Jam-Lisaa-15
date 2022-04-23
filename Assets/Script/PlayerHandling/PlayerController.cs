using System;
using UnityEngine;

namespace Script.PlayerHandling
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float m_JumpStrength;

        private Rigidbody m_RigidBody;

        private void Awake()
        {
            m_RigidBody = GetComponent<Rigidbody>();
        }

        public void Jump()
        {
            m_RigidBody.AddForce(transform.up * m_JumpStrength);
        }

        public void Crouch()
        {
            
        }
    }
}