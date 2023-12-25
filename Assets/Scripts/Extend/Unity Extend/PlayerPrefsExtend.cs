using System;
using UnityEngine;

namespace Xi.Extend
{
    public static class PlayerPrefsExtend
    {
        public static void SaveValue(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
            PlayerPrefs.Save();
        }

        public static void SaveValue(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
            PlayerPrefs.Save();
        }

        public static void SaveValue(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
            PlayerPrefs.Save();
        }

        public static void SaveValue(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
            PlayerPrefs.Save();
        }

        public static void SaveValue(string key, byte[] value)
        {
            string base64String = value != null ? Convert.ToBase64String(value) : null;
            PlayerPrefs.SetString(key, base64String);
            PlayerPrefs.Save();
        }

        public static int GetInt(string key, int defaultValue = 0) => PlayerPrefs.GetInt(key, defaultValue);

        public static string GetString(string key, string defaultValue = "") => PlayerPrefs.GetString(key, defaultValue);

        public static bool GetBool(string key, bool defaultValue = false) => PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;

        public static float GetFloat(string key, float defaultValue = 0.0f) => PlayerPrefs.GetFloat(key, defaultValue);

        public static byte[] GetBytes(string key, byte[] defaultValue = null)
        {
            string base64String = PlayerPrefs.GetString(key, defaultValue != null ? Convert.ToBase64String(defaultValue) : null);
            return !string.IsNullOrEmpty(base64String) ? Convert.FromBase64String(base64String) : defaultValue;
        }
    }
}