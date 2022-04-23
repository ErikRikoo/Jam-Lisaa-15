using System;
using System.Collections;
using System.Collections.Generic;
using Script.Utilities;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Script.PlayerHandling
{
    public class LobbyPlayerController : MonoBehaviour
    {
        [SerializeField] private Renderer m_RendererToUpdate;
        private PlayerLobby m_PlayerLobby;
        private bool m_IsReady;
        private Vector3 m_OriginalScale;
        private InputActionTextBinder m_InputActionBinder;
        private PlayerInput m_Input;

        private void Awake()
        {
            m_OriginalScale = transform.localScale;
        }

        public void BindToHandler(PlayerLobby playerLobby, Color _playerColor,
            InputActionTextBinder inputActionTextBinder, PlayerInput playerInput)
        {
            m_PlayerLobby = playerLobby;
            m_InputActionBinder = inputActionTextBinder;
            m_Input = playerInput;
            if (m_RendererToUpdate == null)
            {
                return;
            }
            
            m_RendererToUpdate.material.color = _playerColor;
        }
        
        public void OnReady(InputAction.CallbackContext _context)
        {
            if (m_IsReady)
            {
                return;
            }
            m_InputActionBinder.gameObject.SetActive(false);
            
            ++m_PlayerLobby.PlayerReadyCount;
            transform.localScale *= 1.1f;
            m_IsReady = true;
        }

        public void OnCancelReady(InputAction.CallbackContext _context)
        {
            if (!m_IsReady)
            {
                return;
            }
            m_InputActionBinder.gameObject.SetActive(true);

            --m_PlayerLobby.PlayerReadyCount;
            transform.localScale = m_OriginalScale;
            m_IsReady = false;
        }

        public void WaitForReady()
        {
            m_InputActionBinder.Bind(m_Input);
            m_InputActionBinder.gameObject.SetActive(true);
        }
    }
}