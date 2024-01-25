using System;
using UnityEditor;
using UnityEngine;
using Xi.Framework;
using Xi.Gameplay.Character.Player;

namespace Xi.TestCase
{
    public class Test_PlayerInput : MonoBehaviour
    {
        public PlayerInput input;

        private void Start()
        {
            InputManager.Instance.SetInputEnable_Player(true);
            input = new PlayerInput(InputManager.Instance.Player);
        }

        private void Update() => input.OnUpdate(Time.deltaTime);
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(Test_PlayerInput))]
    public class Test_PlayerInputEditor : Editor
    {

        private void OnEnable()
        {
            EditorApplication.update += OnEditorUpdate;
        }

        private void OnDisable()
        {
            EditorApplication.update -= OnEditorUpdate;
        }

        private void OnEditorUpdate() => Repaint();

        public override void OnInspectorGUI()
        {
            Test_PlayerInput test = (Test_PlayerInput)target;
            if (test == null || test.input == null)
            {
                return;
            }

            EditorGUILayout.LabelField("Move Input", test.input.MoveInput.ToString());
            EditorGUILayout.LabelField("Raw Look Input", test.input.RawLookInput.ToString());
            EditorGUILayout.LabelField("Look Input", test.input.LookInput.ToString());
            EditorGUILayout.LabelField("Crouch Input", test.input.CrouchInput.ToString());
            EditorGUILayout.LabelField("Tactical Sprint Input", test.input.TacticalSprintInput.ToString());
            EditorGUILayout.LabelField("Sprint Input", test.input.SprintInput.ToString());
            EditorGUILayout.LabelField("Jump Input", test.input.JumpInput.ToString());
            EditorGUILayout.LabelField("Slide Input", test.input.SlideInput.ToString());
        }
    }
#endif


}
