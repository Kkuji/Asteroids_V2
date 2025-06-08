using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using MVVM;

namespace UI.Views
{
    public class InvView : MonoBehaviour
    {
        [Data("Int")] [SerializeField] public TMP_Text intText;
    }
}