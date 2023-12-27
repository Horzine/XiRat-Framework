#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Serialization;

namespace TetraCreations.Attributes
{
    // DrawIf Attribute cannot be used with PathPropertyDrawer, to disable or enable it simply change the _editable value.
    [System.Serializable]
    public class PathReference
    {
        [FormerlySerializedAs("GUI")] public string GUID;
        [SerializeField] private string _cachedPath;
        [SerializeField] private bool _editable = true;

        [SerializeField] private bool _autoUpdate = true;
        [SerializeField] private bool _enableDebug;

        /// <summary>
        /// Display a folder reference by it's GUID or/and path.
        /// </summary>
        /// <param name="editable">Make the PathReference a readOnly field.</param>
        /// <param name="autoUpdate">If true the cached path and the GUID will be set to empty if the asset doesn't exist in AssetDatabase.</param>
        /// <param name="enableDebug">If true it will return a warning if the cached path and GUID are referencing an asset that doesn't exist anymore.</param>
        public PathReference(bool editable = true, bool autoUpdate = true, bool enableDebug = true)
        {
            _editable = editable;
            _autoUpdate = autoUpdate;
            _enableDebug = enableDebug;
        }

        #region Properties
        // Path is now cached and updated automatically in PathPropertyDrawer if GUID value changed.
        public string Path 
        {
            get 
            {
                #if UNITY_EDITOR
                if(AssetDatabase.IsValidFolder(_cachedPath)) { return _cachedPath; }

                // Maybe the folder has been moved or renamed, we need to update the cached path
                if (string.IsNullOrEmpty(GUID) == false)
                {
                    _cachedPath = AssetDatabase.GUIDToAssetPath(GUID);
                    return _cachedPath;
                }
                #endif

                return _cachedPath;
            }
        }

        public bool Editable { get => _editable; protected set => _editable = value; }
        public string CachedPath { get => _cachedPath; protected set => _cachedPath = value; }
        public bool AutoUpdate { get => _autoUpdate; protected set => _autoUpdate = value; }
        public bool EnableDebug { get => _enableDebug; protected set => _enableDebug = value; }
        #endregion

        #if UNITY_EDITOR
        #region Public Methods
        /// <summary>
        /// Set PathReference GUID and Path using a guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public bool SetGUID(string guid)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            if(string.IsNullOrEmpty(path) || AssetDatabase.IsValidFolder(path) == false) { return false; }

            GUID = guid;
            _cachedPath = path;

            return true;
        }

        /// <summary>
        /// Set PathReference GUID and Path using a path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool SetPath(string path)
        {
            if(AssetDatabase.IsValidFolder(path) == false) { return false; }

            GUID = AssetDatabase.AssetPathToGUID(path);
            _cachedPath = path;

            return true;
        }

        /// <summary>
        /// Check if both GUID and the path in cache are not empty or null
        /// </summary>
        /// <returns></returns>
        public bool IsSet()
        {
            if(string.IsNullOrEmpty(GUID) || string.IsNullOrEmpty(_cachedPath)) { return false; }

            return true;
        }

        /// <summary>
        /// Does the cached path is valid and the GUID is founded
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return AssetDatabase.IsValidFolder(_cachedPath) && string.IsNullOrEmpty(AssetDatabase.GUIDToAssetPath(GUID)) == false;
        }

        /// <summary>
        /// Update either the Path using the GUID or the GUID using the path
        /// </summary>
        public bool Update()
        {
            // Update the invalid path in cache
            if (AssetDatabase.IsValidFolder(_cachedPath) == false && string.IsNullOrEmpty(GUID) == false)
            {
                _cachedPath = AssetDatabase.GUIDToAssetPath(GUID);
                return !string.IsNullOrEmpty(_cachedPath);
            }

            // Update the missing GUID
            if (string.IsNullOrEmpty(GUID) && AssetDatabase.IsValidFolder(_cachedPath))
            {
                GUID = AssetDatabase.AssetPathToGUID(_cachedPath);
                return !string.IsNullOrEmpty(GUID);
            }

            return true;
        }
        #endregion
        #endif
    }
}
