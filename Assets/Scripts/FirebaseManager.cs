using UnityEngine;
using Firebase;
using Firebase.Extensions;

public class FirebaseManager : MonoBehaviour
{
    public static bool IsReady { get; private set; }

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            DependencyStatus status = task.Result;

            if (status == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;

                IsReady = true;

                Debug.Log("Firebase initialized successfully!");
            }
            else
            {
                Debug.LogError("Firebase initialization failed: " + status);
            }
        });
    }
}