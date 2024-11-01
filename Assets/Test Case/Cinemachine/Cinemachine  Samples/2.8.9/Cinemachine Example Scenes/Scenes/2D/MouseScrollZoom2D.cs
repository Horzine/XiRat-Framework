﻿using System;
using UnityEngine;

namespace Cinemachine.Examples
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    [SaveDuringPlay] // Enable SaveDuringPlay for this class
    public class MouseScrollZoom2D : MonoBehaviour
    {
        [Range(0, 10)]
        public float ZoomMultiplier = 1f;
        [Range(0, 100)]
        public float MinZoom = 1f;
        [Range(0, 100)]
        public float MaxZoom = 50f;
        private CinemachineVirtualCamera m_VirtualCamera;
        private float m_OriginalOrthoSize;

        private void Awake()
        {
            m_VirtualCamera = GetComponent<CinemachineVirtualCamera>();
            m_OriginalOrthoSize = m_VirtualCamera.m_Lens.OrthographicSize;

#if UNITY_EDITOR
            // This code shows how to play nicely with the VirtualCamera's SaveDuringPlay functionality
            SaveDuringPlay.SaveDuringPlay.OnHotSave -= RestoreOriginalOrthographicSize;
            SaveDuringPlay.SaveDuringPlay.OnHotSave += RestoreOriginalOrthographicSize;
#endif
        }

#if UNITY_EDITOR
        private void OnDestroy() => SaveDuringPlay.SaveDuringPlay.OnHotSave -= RestoreOriginalOrthographicSize;

        private void RestoreOriginalOrthographicSize() => m_VirtualCamera.m_Lens.OrthographicSize = m_OriginalOrthoSize;
#endif

        private void OnValidate() => MaxZoom = Mathf.Max(MinZoom, MaxZoom);

        private void Update()
        {
            float zoom = m_VirtualCamera.m_Lens.OrthographicSize + (Input.mouseScrollDelta.y * ZoomMultiplier);
            m_VirtualCamera.m_Lens.OrthographicSize = Mathf.Clamp(zoom, MinZoom, MaxZoom);
        }
    }
}
