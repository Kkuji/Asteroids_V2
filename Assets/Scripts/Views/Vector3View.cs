using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using MVVM;

namespace UI.Views
{
    public class Vector3View : MonoBehaviour
    {
        [Data("Vector3")] [SerializeField] public TMP_Text vector3Text;
    }
}