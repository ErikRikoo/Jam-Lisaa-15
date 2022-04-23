using Script.PlayerHandling;
using UnityEditor;
using UnityEngine;

namespace PlayerHandling
{
    [CustomEditor(typeof(PlayerStateManager))]
    public class PlayerStateManagerEditor : Editor
    {
        private float m_EffectDuration;
        private float m_EffectRatio;
        
        private PlayerStateManager Target => target as PlayerStateManager;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            // if (GUILayout.Button("Jump"))
            // {
            //     Target.Jump();
            // }
            
            // if (GUILayout.Button("Crouch"))
            // {
            //     
            // }
            //
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Debug", GUI.skin.label);
            m_EffectDuration = EditorGUILayout.FloatField("Effect Duration", m_EffectDuration);
            m_EffectRatio = EditorGUILayout.FloatField("Effect Strength", m_EffectRatio);
            
            if (GUILayout.Button("Stun"))
            {
                Target.Stun(m_EffectDuration);
            }
            
            if (GUILayout.Button("Slow"))
            {
                Target.Slow(m_EffectRatio, m_EffectDuration);
            }
        
            if (GUILayout.Button("Boost"))
            {
                Target.Boost(m_EffectRatio, m_EffectDuration);
            }

        }
    }
}