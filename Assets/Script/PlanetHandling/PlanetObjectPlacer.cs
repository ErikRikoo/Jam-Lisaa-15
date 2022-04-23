using System;
using UnityEngine;

namespace PlanetHandling
{
    public class PlanetObjectPlacer : MonoBehaviour
    {
        [SerializeField] private Vector2 m_Coordinate;
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
            Vector3 newPosition = new Vector3(
                Mathf.Sin(m_Coordinate.x) * Mathf.Cos(m_Coordinate.y),
                Mathf.Cos(m_Coordinate.x),
                Mathf.Sin(m_Coordinate.x) * Mathf.Sin(m_Coordinate.y)
            ) * radius;
            transform.position = newPosition;
            transform.up = newPosition - m_Collider.center;
        }
        
        
    }
}