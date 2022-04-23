using System;
using System.Linq;
using UnityEngine;

namespace Script.PlayerHandling
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float m_JumpStrength;

        private Rigidbody m_RigidBody;

        private bool m_IsGrounded = true;

        private void Awake()
        {
            m_RigidBody = GetComponent<Rigidbody>();
        }

        public void Jump()
        {
            if (!m_IsGrounded)
            {
                return;
            }
            m_RigidBody.AddForce(transform.up * m_JumpStrength);
            m_IsGrounded = false;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.contacts.Any(contactPoint => contactPoint.normal == transform.up))
            {
                m_IsGrounded = true;
            }
        }

        public void Crouch()
        {
            
        }
    }
}