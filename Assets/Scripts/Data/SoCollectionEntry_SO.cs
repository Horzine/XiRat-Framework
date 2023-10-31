using UnityEngine;
using Xi.Extent.Attribute;
#if UNITY_EDITOR
#endif

namespace Xi.Data
{
    public abstract class SoCollectionEntry_SO : ScriptableObject
    {
        [ReadOnly, SerializeField] private int m_ConfigId = 0;
        [SerializeField] private string m_DisplayName;
        public void SetIntConfigId(int value) => m_ConfigId = value;
        public int GetIntConfigId(bool silence = false)
        {
            if (!SoCollection_SO.ConfigIdValid(m_ConfigId) && !silence)
            {
                Debug.LogError("ConfigIdValid", this);
            }

            return m_ConfigId;
        }
        public string ConfigId => GetIntConfigId().ToString();
        public string DisplayName => m_DisplayName;
        public abstract string ToTxt_Comment();
        public abstract string ToTxt_Header();
        public abstract string ToTxt_Type();
        public abstract string ToTxt_Entry();
    }
}

