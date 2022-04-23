using System;
using UnityEngine;

namespace PlanetHandling
{
    public class PlanetObjectPlacer : MonoBehaviour
    {
        [SerializeField] private Vector2 m_Coordinate;

        public float Latitude
        {
            get => m_Coordinate.x;
            set
            {
                m_Coordinate.x = value;
                UpdatePosition();
            }
        }

        [SerializeField] private SphereCollider m_Collider;
        
        private void OnValidate()
        {
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            if (m_Collider == null)
            {
                return;
            }

            float radius = m_Collider.transform.lossyScale.x * m_Collider.radius;
            var angles = m_Coordinate * Mathf.Deg2Rad;
            Vector3 newPosition = new Vector3(
                Mathf.Sin(angles.x) * Mathf.Sin(angles.y),
                Mathf.Cos(angles.x),
                Mathf.Sin(angles.x) * Mathf.Cos(angles.y)

            ) * radius;
            transform.position = newPosition;
            transform.up = newPosition - m_Collider.center;
        }
        
        
    }
}