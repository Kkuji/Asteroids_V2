using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIVisibilityConfig : MonoBehaviour
{
    [SerializeField] private List<GameObject> uiElementsToHideIfPC = new();

    private void Awake()
    {
#if !UNITY_IOS && !UNITY_ANDROID
        foreach (GameObject uiElement in uiElementsToHideIfPC)
            uiElement.SetActive(false);
#endif
    }
}