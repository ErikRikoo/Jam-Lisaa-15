using System;
using System.Collections;
using Script.PlayerHandling;
using Script.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Script
{
    public class DefeatScript : MonoBehaviour
    {
        [SerializeField] private AudioSource m_DefeatAudioSource;
        [SerializeField] private AudioSource m_VictoryAudioSource;
        [SerializeField] private GameObject m_UI;
        [SerializeField] private GameObject[] m_ObjectToDisable;
        [SerializeField] private TextFader m_Winner;

        [SerializeField] private float m_UIDelay;
        
        
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }

            foreach (var o in m_ObjectToDisable)
            {
                o.SetActive(false);
            }
            
            
            m_DefeatAudioSource.Play();

            foreach (var playerInput in GameObject.FindObjectsOfType<PlayerInput>())
            {
                playerInput.SwitchCurrentActionMap("Idle");
            }

            StartCoroutine(c_Wait(other.gameObject));
        }

        private IEnumerator c_Wait(GameObject go)
        {
            yield return new WaitForSeconds(m_UIDelay);
            m_UI.SetActive(true);
            m_VictoryAudioSource.Play();
            string name = go.GetComponent<LobbyPlayerController>().Name;
            m_Winner.ChangeText(name + " won !");
        }
    }
}