﻿using UnityEngine;
using Xi.Config;
using static UnityEngine.InputSystem.InputAction;

namespace Xi.TestCase
{
    public class Test_InputSystem_InputConfig : MonoBehaviour, InputActionConfig.IPlayerActions, InputActionConfig.IHUDActions
    {
        private InputActionConfig _inputObj;

        void InputActionConfig.IPlayerActions.OnFire(CallbackContext context)
            => print($"{nameof(InputActionConfig.IPlayerActions.OnFire)}, Started: {context.started}, Performed: {context.performed}, Canceled: {context.canceled}");
        void InputActionConfig.IPlayerActions.OnLook(CallbackContext context) { }//=> print(nameof(InputActionConfig.IPlayerActions.OnLook));
        void InputActionConfig.IPlayerActions.OnMove(CallbackContext context) => print(nameof(InputActionConfig.IPlayerActions.OnMove));
        void InputActionConfig.IPlayerActions.OnReload(CallbackContext context) => print(nameof(InputActionConfig.IPlayerActions.OnReload));
        void InputActionConfig.IPlayerActions.OnTacticsReload(CallbackContext context)
            => print($"{nameof(InputActionConfig.IPlayerActions.OnTacticsReload)}, Started: {context.started}, Performed: {context.performed}, Canceled: {context.canceled}");
        void InputActionConfig.IHUDActions.OnInteractive(CallbackContext context) { }

        private void Awake()
        {
            _inputObj = new InputActionConfig();
            _inputObj.Player.SetCallbacks(this);
            _inputObj.HUD.SetCallbacks(this);

        }

        private void OnEnable() => _inputObj.Enable();

        private void OnDisable() => _inputObj.Disable();

        void InputActionConfig.IPlayerActions.OnSprint(CallbackContext context)
            => print($"{nameof(InputActionConfig.IPlayerActions.OnSprint)}, Started: {context.started}, Performed: {context.performed}, Canceled: {context.canceled}");
    }
}
