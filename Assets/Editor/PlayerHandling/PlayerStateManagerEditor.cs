using Script.PlayerHandling;
using UnityEditor;
using UnityEngine;

namespace PlayerHandling
{
    [CustomEditor(typeof(PlayerStateManager))]
    public class PlayerStateManagerEditor : Editor
    {
        private PlayerStateManager Target => target as PlayerStateManager;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Jump"))
            {
                Target.Jump();
            }
            
            // if (GUILayout.Button("Crouch"))
            // {
            //     
            // }
            //
            // if (GUILayout.Button("Stun"))
            // {
            //     
            // }
            //
            // if (GUILayout.Button("Slow"))
            // {
            //     
            // }
        

        }
    }
}