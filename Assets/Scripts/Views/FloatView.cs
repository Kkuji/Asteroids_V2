using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using MVVM;

namespace UI.Views
{
    public class FloatView : MonoBehaviour
    {
        [Data("Float")] [SerializeField] public TMP_Text floatText;
    }
}