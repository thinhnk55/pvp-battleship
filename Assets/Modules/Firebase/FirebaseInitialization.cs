using Firebase;
using Firebase.Crashlytics;
using Firebase.Extensions;
using System;
using System.Threading.Tasks;
using UnityEngine;
namespace FirebaseIntegration
{
    public class FirebaseInitialization : MonoBehaviour
    {
        static public bool initialized = false;
        public delegate void Callback();
        public static Callback OnInitialized;
        public static Task InitTask;
        void Start()
        {
            Progress<Task> progress = new Progress<Task>();
            InitTask = FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    FirebaseApp app = FirebaseApp.DefaultInstance;
                    Crashlytics.ReportUncaughtExceptionsAsFatal = true;
                    initialized = true;
                    OnInitialized?.Invoke();
                    Debug.Log("Firebase Initialize successfully");
                }
                else
                {
                    Debug.LogError(System.String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                    initialized = false;
                }
            });
            // Initialize Firebase
        }
    }
}
