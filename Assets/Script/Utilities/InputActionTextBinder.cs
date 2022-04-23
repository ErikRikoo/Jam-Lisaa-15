using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Script.Utilities
{
    public class InputActionTextBinder : MonoBehaviour
    {
        [SerializeField] private string m_GamePadKey;
        [SerializeField] private string m_KeyboardKey;
        
        
        [SerializeField] private string m_ActionName;
        
        [SerializeField] private string m_TextToDisplay;
        
        [SerializeField] private TextMeshProUGUI m_Text;

        public void Bind(PlayerInput _input)
        {
            Debug.Log("Called");
            string key = _input.currentControlScheme switch
            {
                "Gamepad" => m_GamePadKey,
                "Keyboard&Mouse" => m_KeyboardKey
            };

            m_Text.text = String.Format(m_TextToDisplay, key);
        }
    }
}