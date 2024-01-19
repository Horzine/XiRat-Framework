using UnityEngine;
using Xi.Extend.Attribute;
#if UNITY_EDITOR
#endif

namespace Xi.Data
{
    public abstract class SoCollectionEntry_SO : ScriptableObject
    {
        [ReadOnly, SerializeField] private int _configId = 0;
        [SerializeField] private string m_DisplayName;
        public void SetIntConfigId(int value) => _configId = value;
        public int GetIntConfigId(bool silence = false)
        {
            if (!SoCollection_SO.ConfigIdValid(_configId) && !silence)
            {
                Debug.LogError("ConfigIdValid", this);
            }

            return _configId;
        }
        public string ConfigId => GetIntConfigId().ToString();
        public string DisplayName => m_DisplayName;
        public abstract string ToTxt_Comment();
        public abstract string ToTxt_Header();
        public abstract string ToTxt_Type();
        public abstract string ToTxt_Entry();
    }
}

