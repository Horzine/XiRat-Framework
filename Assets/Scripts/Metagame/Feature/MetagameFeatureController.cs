using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Xi.Tools;

namespace Xi.Metagame.Feature
{
    public enum MetagameFeatureEnum
    {
        Main,
        Mission,
        Arsenal,
        XXX,
    }

    public class MetagameFeatureController : MonoBehaviour
    {
        [SerializeField] private List<MetagameFeatureObj> _allFeatureObjList = new();
        private Dictionary<int, MetagameFeatureObj> _allFeatureObjDictionary = new();
        private MetagameFeatureObj _currentActiveFeatureObj;

        public MetagameFeatureObj CurrentActiveFeatureObj
        {
            get => _currentActiveFeatureObj; set
            {
                if (_currentActiveFeatureObj != value && _currentActiveFeatureObj)
                {
                    _currentActiveFeatureObj.SetupDeactive();
                }

                _currentActiveFeatureObj = value;
                if (value)
                {
                    value.SetupActive();
                }
            }
        }

        private void Awake() => _allFeatureObjDictionary = _allFeatureObjList.ToDictionary(item => (int)item.FeatureEnum);

        private void Start()
        {
            foreach (var item in _allFeatureObjList)
            {
                item.Setup();
            }

            var mainFeature = GetFeatureObj<MetagameFeatureObj>(MetagameFeatureEnum.Main);
            if (mainFeature != null)
            {
                CurrentActiveFeatureObj = mainFeature;
            }
        }

        public T GetFeatureObj<T>(MetagameFeatureEnum enumValue) where T : MetagameFeatureObj
        {
            var result = GetFeatureObj(enumValue);
            if (result == null)
            {
                XiLogger.LogError($"No find '{enumValue}' this FeatureObj");
                return null;
            }

            if (result is not T featureObj)
            {
                XiLogger.LogError($"enumValue = {enumValue}, result type is {result.GetType()}, not match {typeof(T)}");
                return null;
            }

            return featureObj;
        }

        public MetagameFeatureObj GetFeatureObj(MetagameFeatureEnum enumValue)
        {
            if (!_allFeatureObjDictionary.TryGetValue((int)enumValue, out var result))
            {
                XiLogger.LogError($"No find '{enumValue}' this FeatureObj");
                return null;
            }

            return result;
        }

        public void ActiveFeature(MetagameFeatureEnum enumValue)
        {
            if (_currentActiveFeatureObj != null && _currentActiveFeatureObj.FeatureEnum == enumValue)
            {
                XiLogger.Log($"Same as current enumValue '{enumValue}'");
                return;
            }

            var target = GetFeatureObj(enumValue);
            if (target != null)
            {
                CurrentActiveFeatureObj = target;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            _allFeatureObjList.Clear();
            var allObj = GetComponentsInChildren<MetagameFeatureObj>();
            foreach (var item in allObj)
            {
                _allFeatureObjList.Add(item);
            }

            XiLogger.Log($"Setup allFeatureObj with child", this);
        }
#endif
    }
}
