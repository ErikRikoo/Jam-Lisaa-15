using System.Collections;
using System.Collections.Generic;
using PlanetHandling;
using Script.PlayerHandling.Spells;
using Script.Utilities;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Script.PlayerHandling
{
    public class PlayerLobby : MonoBehaviour
    {
        [SerializeField] private PlanetTool m_Planet;
        [SerializeField] private SphereCollider m_PlanetCollider;
        
        [SerializeField] private TextFader m_TextDisplay;
        
        [SerializeField] private VoidEvent m_GameStarted;
        
        [SerializeField] private Color[] m_PlayerColors;
        [SerializeField] private float[] m_LatitudeOffsets;
        [SerializeField] private InputActionTextBinder[] m_TextBinders;
        [SerializeField] private ImageHolder[] m_PlayerUI;
        [SerializeField] private string[] m_Names = new []{"Day", "Night"};
        
        
        [SerializeField] private Transform m_Parent;
        [SerializeField] private int m_StartTimerDuration;

        [SerializeField] private GameObject m_ReadynessUI;
        

        private List<PlayerInput> m_Players = new();

        private int m_PlayerCount;

        private int PlayerCount
        {
            get => m_PlayerCount;
            set
            {
                m_PlayerCount = value;
                if (m_PlayerCount == 2)
                {
                    OnWaitingForReady();
                }
            }
        }

        [SerializeField]
        private int m_PlayerReadyCount;
        
        
        private void OnWaitingForReady()
        {
            StartCoroutine(c_EnablePlayer());
        }
        
        public int PlayerReadyCount
        {
            get => m_PlayerReadyCount;
            set
            {
                var oldValue = m_PlayerReadyCount;

                m_PlayerReadyCount = value;
                OnReadyChanged(oldValue);
            }
        }

        private void OnReadyChanged(int oldValue)
        {
            if (oldValue < m_PlayerReadyCount)
            {
                if (m_PlayerReadyCount == m_PlayerCount)
                {
                    StartTimer();
                }
            }
            else
            {
                if (m_PlayerReadyCount < m_PlayerCount)
                {
                    StopTimerIfNeeded();
                }
            }
        }

        private Coroutine m_TimerCoroutine;
        
        private void StartTimer()
        {
            if (m_TimerCoroutine != null)
            {
                return;
            }

            m_TimerCoroutine = StartCoroutine(c_Timer());
        }

        private void StopTimerIfNeeded()
        {
            if (m_TimerCoroutine == null)
            {
                return;
            }
            
            StopCoroutine(m_TimerCoroutine);

            m_TimerCoroutine = null;
        }

        private IEnumerator c_Timer()
        {
            for (int time = m_StartTimerDuration; time > 0; --time)
            {
                UpdateText(time);
                yield return new WaitForSeconds(1);
            }

            DisplayGo();
            StartGame();
        }

        private void DisplayGo()
        {
            m_TextDisplay.ChangeText("GO!");
        }

        private void UpdateText(int time)
        {
            m_TextDisplay.ChangeText(time.ToString());
        }

        public void OnPlayerJoined(PlayerInput _input)
        {
            var lobbyPlayer = _input.GetComponent<LobbyPlayerController>();
            if (lobbyPlayer == null)
            {
                return;
            }

            m_Players.Add(_input);
            lobbyPlayer.transform.parent = m_Parent;
            lobbyPlayer.BindToHandler(this, m_PlayerColors[m_PlayerCount], m_TextBinders[m_PlayerCount], _input);
            lobbyPlayer.Name = m_Names[m_PlayerCount];
            var objectPlacer = _input.GetComponent<PlanetObjectPlacer>();
            objectPlacer.Collider = m_PlanetCollider;
            objectPlacer.Latitude = m_Planet.Orientation + m_LatitudeOffsets[m_PlayerCount];
            ++PlayerCount;
        }

        private IEnumerator c_EnablePlayer()
        {
            yield return null;
            foreach (var player in m_Players)
            {
                player.SwitchCurrentActionMap("Menu");
                // TODO: Store it
                player.GetComponent<LobbyPlayerController>().WaitForReady();
            }
        }

        private void StartGame()
        {
            m_GameStarted?.Raise();
            int index = 0;
            foreach (var player in m_Players)
            {
                EnableGameplayInputs(player);
                player.GetComponent<SpellHandler>().Bind(m_PlayerUI[index]);
                ++index;
            }
        }
        
        private void EnableGameplayInputs(PlayerInput _input)
        {
            _input.SwitchCurrentActionMap("Gameplay");
        }
    }
}