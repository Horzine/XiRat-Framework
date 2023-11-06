using UnityEngine;
using UnityEngine.UI;

namespace Xi.Extend.UnityExtend
{
    public static class GameObjectExtend
    {
        public static void SetParent(this GameObject child, GameObject parent) => child.transform.SetParent(parent.transform);

        public static void SetSelfActive(this GameObject go, bool active)
        {
            if (go)
            {
                go.SetActive(active);
            }
        }

        public static void SetSelfActive(this Component comp, bool active)
        {
            if (comp)
            {
                SetSelfActive(comp.gameObject, active);
            }
        }

        public static void SetSelfEnable(this Behaviour behaviour, bool enabled)
        {
            if (behaviour)
            {
                behaviour.enabled = enabled;
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

        public static T GetOrAddComponent<T>(this Component comp) where T : Component
            => comp.gameObject.GetOrAddComponent<T>();

        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            if (!go.TryGetComponent<T>(out var comp))
            {
                comp = go.AddComponent<T>();
            }

            return comp;
        }

        public static string PrintGameObjectTreePath(this GameObject go)
        {
            var c = go.transform.parent;
            string r = go.name;
            while (c != null)
            {
                r = $"{c.name}->{r}";
                c = c.parent;
            }

            return r;
        }

        public static void SetLayerRecursively(this GameObject go, int layer)
        {
            if (go == null)
            {
                return;
            }

            go.layer = layer;
            for (int i = 0; i < go.transform.childCount; ++i)
            {
                go.transform.GetChild(i).gameObject.SetLayerRecursively(layer);
            }
        }

        public static bool HasChild(this GameObject parent, GameObject child) => child.transform.IsChildOf(parent.transform);

        public static bool IsChildOf(this GameObject child, GameObject parent) => child.transform.IsChildOf(parent.transform);

        public static void DestroySelf(this GameObject go)
        {
            if (go == null)
            {
                return;
            }

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                Object.DestroyImmediate(go);
            }
            else
#endif
            {
                Object.Destroy(go);
            }
        }

        public static void DestroySelfGameObject(this Component comp)
        {
            if (comp && comp.gameObject)
            {
                comp.gameObject.DestroySelf();
            }
        }

        public static void DestroyObjectAndReleaseReference(this MonoBehaviour _, ref GameObject targetGo)
        {
            if (targetGo)
            {
                Object.Destroy(targetGo);
                targetGo = null;
            }
        }

        public static void DestroyObjectAndReleaseReference<T>(this MonoBehaviour _, ref T target) where T : Component
        {
            if (target && target.gameObject)
            {
                Object.Destroy(target.gameObject);
                target = null;
            }
        }

        public static T GetOrAddComponentAsChild<T>(this Component comp, string name = "", bool includeInactive = false) where T : Component
            => comp && comp.gameObject ? comp.gameObject.GetOrAddComponentAsChild<T>(name, includeInactive) : null;

        public static T GetOrAddComponentAsChild<T>(this GameObject go, string name = "", bool includeInactive = false) where T : Component
        {
            if (!go)
            {
                return null;
            }

            var newComp = go.GetComponentInChildren<T>(includeInactive);
            if (newComp == null)
            {
                if (string.IsNullOrEmpty(name))
                {
                    name = typeof(T).FullName;
                }

                var child = new GameObject(name);
                newComp = child.AddComponent<T>();
                child.SetParent(go);
            }

            return newComp;
        }
    }
}