using System;
using UnityEngine;

namespace Xi.Tools
{
    internal static class DebugSetting
    {
        public static readonly GUILayoutOption[] fpsButton = new GUILayoutOption[2] { width, height };

        public static readonly GUILayoutOption[] button = new GUILayoutOption[2] { boxFix, heightLow };

        public static GUILayoutOption width => GUILayout.Width(80f);

        public static GUILayoutOption height => GUILayout.Height(30f);

        public static GUILayoutOption heightLow => GUILayout.Height(25f);

        public static GUILayoutOption heightLowest => GUILayout.Height(20f);

        public static GUILayoutOption heightFix => GUILayout.Height(windowHeight * 0.4f);

        public static GUILayoutOption box => GUILayout.Width((windowWidth - 32f) / 2f);

        private static GUILayoutOption boxFix => GUILayout.Width((windowWidth - 46f) / 4f);

        public static Rect rect => new(0f, 0f, windowWidth, windowHeight);

        public static Rect windowRect => new(10f, 22f, windowWidth - 20f, windowHeight - 32f);

        public static Vector3 scale => new(windowScale, windowScale, 1f);

        private static float windowScale => (screenWidth / 1920f) + (screenHeight / 1080f);

        private static float screenWidth => Screen.width;

        private static float screenHeight => Screen.height;

        private static float windowWidth => screenWidth / windowScale;

        private static float windowHeight => screenHeight / windowScale;

        public static Vector3 Vector3Field(Vector3 value)
        {
            string text = GUILayout.TextField(value.x.ToString("F"), Array.Empty<GUILayoutOption>());
            string text2 = GUILayout.TextField(value.y.ToString("F"), Array.Empty<GUILayoutOption>());
            string text3 = GUILayout.TextField(value.z.ToString("F"), Array.Empty<GUILayoutOption>());
            return text == value.x.ToString("F") && text2 == value.y.ToString("F") && text3 == value.z.ToString("F")
                ? value
                : float.TryParse(text, out float result) && float.TryParse(text2, out float result2) && float.TryParse(text3, out float result3)
                ? new Vector3(result, result2, result3)
                : value;
        }

        public static Vector2 Vector2Field(Vector2 value)
        {
            string text = GUILayout.TextField(value.x.ToString("F"), Array.Empty<GUILayoutOption>());
            string text2 = GUILayout.TextField(value.y.ToString("F"), Array.Empty<GUILayoutOption>());
            return text == value.x.ToString("F") && text2 == value.y.ToString("F")
                ? value
                : float.TryParse(text, out float result) && float.TryParse(text2, out float result2) ? (Vector2)new Vector3(result, result2) : value;
        }

        public static float FloatField(float value)
        {
            string text = GUILayout.TextField(value.ToString("F"), Array.Empty<GUILayoutOption>());
            return text == value.ToString("F") ? value : float.TryParse(text, out float result) ? result : value;
        }

        public static int IntField(int value)
        {
            string text = GUILayout.TextField(value.ToString(), Array.Empty<GUILayoutOption>());
            return text == value.ToString() ? value : int.TryParse(text, out int result) ? result : value;
        }

        public static Enum EnumField(Enum value)
        {
            foreach (Enum value2 in Enum.GetValues(value.GetType()))
            {
                if (GUILayout.Toggle(value.Equals(value2), $" {value2}", Array.Empty<GUILayoutOption>()))
                {
                    value = value2;
                }
            }

            return value;
        }
    }
}