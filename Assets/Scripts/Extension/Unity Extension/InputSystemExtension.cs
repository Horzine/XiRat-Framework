using UnityEngine;
using UnityEngine.InputSystem;

namespace Xi.Extension.UnityExtension
{
    public static class InputSystemExtension
    {
        public static void HasDoubleClicked(this InputAction inputAction, ref bool targetValue, ref float lastClickTime, float maxClickTime = 0.5f)
        {
            if (inputAction.triggered)
            {
                float timeSinceLastSprintClick = Time.time - lastClickTime;

                if (timeSinceLastSprintClick < maxClickTime)
                {
                    targetValue = true;
                }

                lastClickTime = Time.time;
            }

            if (inputAction.IsPressed() == false)
            {
                targetValue = false;
            }
        }
    }
}
