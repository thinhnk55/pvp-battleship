using Firebase;
using Firebase.Crashlytics;
using Firebase.Extensions;
using System.Threading.Tasks;
using UnityEngine;
namespace FirebaseIntegration
{
    public class FirebaseInitialization
    {
        static public bool initialized = false;
        public static Callback OnInitialized;
        public static Task InitTask;
        public static void Initialize()
        {
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
