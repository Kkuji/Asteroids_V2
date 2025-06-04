using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class FirebaseInit : MonoBehaviour
{
    private const string FIRST_OPEN_EVENT_NAME = "first_open_custom";

    public void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => HandleFirebaseInitCompleted(task));
    }

    private async Task HandleFirebaseInitCompleted(Task<DependencyStatus> task)
    {
        DependencyStatus dependencyStatus = task.Result;

        if (dependencyStatus == DependencyStatus.Available)
        {
            Debug.Log("Firebase successfully initialized");
            LogFirstOpenEvent();
        }
        else
        {
            Debug.LogError($"Could not resolve Firebase dependencies: {dependencyStatus}");
        }
    }

    private void LogFirstOpenEvent()
    {
        if (PlayerPrefs.HasKey("firstOpenFirebase"))
            return;

        PlayerPrefs.SetString("firstOpenFirebase", "true");
        PlayerPrefs.Save();
        FirebaseAnalytics.LogEvent(FIRST_OPEN_EVENT_NAME);
        Debug.Log("First open event sent");
    }
}