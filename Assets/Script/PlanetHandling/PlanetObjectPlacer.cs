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

        public Vector2 Coordinate
        {
            get => m_Coordinate;
            set
            {
                m_Coordinate = value;
                UpdatePosition();
            }
        }

        [SerializeField] public SphereCollider Collider;
        
        private void OnValidate()
        {
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            if (Collider == null)
            {
                return;
            }

            float radius = Collider.transform.lossyScale.x * Collider.radius;
            var angles = m_Coordinate * Mathf.Deg2Rad;
            Vector3 newPosition = new Vector3(
                Mathf.Sin(angles.x) * Mathf.Sin(angles.y),
                Mathf.Cos(angles.x),
                Mathf.Sin(angles.x) * Mathf.Cos(angles.y)

            ) * radius;
            transform.position = newPosition;
            transform.up = newPosition - Collider.center;
        }
        
        
    }
}