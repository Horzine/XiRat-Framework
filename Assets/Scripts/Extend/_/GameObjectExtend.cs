﻿ /*==============================================================
	  * Copyright 2018 Tencent Inc. 
	  *
      *  作者：Zach (zachzhong@21kunpeng.com)
      *  时间：#DATETIME#
      *  文件名：GameObjectExtend
      *  说明： GameObject 或者Mono的扩展
      ========================================*/
using UnityEngine;
using UnityEngine.UI;

public static class GameObjectExtend{

	public static void SetSelfActive(this GameObject go ,bool active)
    {
        if (go)
        {
            go.SetActive(active);
        }
    }

    public static void SetSelfActive(this MonoBehaviour mono, bool active)
    {
        if (mono)
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

	public static void SafeSetTrigger(this Animator animtor, string trigger)
	{
		if (animtor)
		{
			animtor.SetTrigger(trigger);
		}
	}

    public static void SetToggleIsOn(this Toggle toggle,bool isOn)
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
