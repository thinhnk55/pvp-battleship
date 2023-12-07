using Firebase;
using Firebase.Crashlytics;
using Firebase.Extensions;
using Framework;
using System.Threading.Tasks;
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
                    PDebug.Log("Firebase Initialize successfully");
                }
                else
                {
                    PDebug.LogError(System.String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                    initialized = false;
                }
            });
            // Initialize Firebase
        }
    }
}
