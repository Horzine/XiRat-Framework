using UnityEngine;
using UnityEngine.UI;

namespace Xi.Extend.UnityExtend
{
    public static class GameObjectExtend
    {
        public static void SetSelfActive(this GameObject go, bool active)
        {
            if (go)
            {
                go.SetActive(active);
            }
        }

        public static void SetSelfActive(this MonoBehaviour mono, bool active)
        {
            if (mono && mono.gameObject)
            {
                mono.gameObject.SetActive(active);
            }
        }

        public static void SetSelfEnable(this MonoBehaviour mono, bool enabled)
        {
            if (mono)
            {
                mono.enabled = enabled;
            }
        }

        public static void SetToggleIsOn(this Toggle toggle, bool isOn)
        {
            if (toggle)
            {
                toggle.isOn = isOn;
            }
        }

        public static void SetButtonInteractable(this Button button, bool interactable)
        {
            if (button)
            {
                button.interactable = interactable;
            }
        }
    }
}