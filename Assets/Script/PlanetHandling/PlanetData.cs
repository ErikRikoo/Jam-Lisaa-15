using UnityEngine;

namespace PlanetHandling
{
    public class PlanetData : MonoBehaviour
    {
        [SerializeField] private Transform m_ObjectChild;

        public Transform ObjectChild => m_ObjectChild;

    }
}