using UnityEngine;
using UnityEngine.UI;

namespace Script.Utilities
{
    public class ImageHolder : MonoBehaviour
    {
        [SerializeField] private Image[] m_Images;

        public Image[] Images => m_Images;

    }
}